﻿using mes_center.Models.kafka;
using mes_center.Models.rest.server_dto;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.ViewModels
{

    public class ordersListVM : ViewModelBase, IReloadable
    {
        #region vars        
        string lastCheckedOrder = "";        
        #endregion

        #region properties
        List<Order> ordersList = new();
        public List<Order> OrdersList
        {
            get => ordersList;
            set => this.RaiseAndSetIfChanged(ref ordersList, value);
        }

        Order order;
        public Order Order
        {
            get => order;
            set
            {
                this.RaiseAndSetIfChanged(ref order, value);

                if (order != null)
                {
                    Order rdOrder = serverApi.GetOrder(order.order_num);
                    OrderCheckedAction?.Invoke(rdOrder);
                    lastCheckedOrder = rdOrder.order_num;
                }
            }
        }

        public Order.OrderStatus[] OrderStatuses { get; set; } = new Order.OrderStatus[] { Order.OrderStatus.RECEIVED, Order.OrderStatus.READY_TO_EXECUTE };
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> refreshCmd { get; }
        #endregion

        public ordersListVM()
        {

            consumer order_events = new consumer("orderslist_" + DateTime.Now);
            order_events.TopicUpdatedEvent += async (msg) =>
            {
                await Reload();
            };
            order_events.start("order_event");

            #region commands
            refreshCmd = ReactiveCommand.CreateFromTask(async () =>
            {
                await Reload();
            });
            #endregion
        }

        #region public
        public async Task Reload()
        {
            try
            {
                await Task.Run(async () =>
                {

                    var orders = await serverApi.GetOrders(OrderStatuses);
                    orders = orders.OrderByDescending(o => DateTime.Parse(o.reg_date)).ToList();
                    OrdersList = orders;

                    var found = OrdersList.FirstOrDefault(o => o.order_num.Equals(lastCheckedOrder));
                    Order = (found != null) ? found : OrdersList[0];

                });

            }
            catch (Exception ex)
            {
                logger.dbg(ex.Message);
                showError(ex.Message);
            }

            //List<Order> orders = new();
            //orders.Add(Order.GetTestOrder());

            //OrdersList = orders;


        }
        #endregion

        #region callbacks
        public Action<Order> OrderCheckedAction;
        #endregion
    }
}
