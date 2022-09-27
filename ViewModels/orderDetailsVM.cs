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
    public class orderDetailsVM : ViewModelBase
    {

        #region properties
        Order order = new();
        public Order Order
        {
            get => order;
            set => this.RaiseAndSetIfChanged(ref order, value);
        }

        bool isAcceptRejectNeed;
        public bool IsAcceptRejectNeed
        {
            get => isAcceptRejectNeed;
            set => this.RaiseAndSetIfChanged(ref isAcceptRejectNeed, value);
        }

        bool isSerialVisible;
        public bool IsSerialVisible
        {
            get => isSerialVisible;
            set => this.RaiseAndSetIfChanged(ref isSerialVisible, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> acceptOrderCmd { get; }
        public ReactiveCommand<Unit,Unit> rejectOrderCmd { get; }
        #endregion

        public orderDetailsVM() { }

        public orderDetailsVM(ordersListVM orders)
        {

            orders.OrderCheckedAction = (order) =>
            {
                IsAcceptRejectNeed = false;
                IsSerialVisible = false;

                switch ((Order.OrderStatus)order.status)
                {
                    case Order.OrderStatus.RECEIVED:
                        IsAcceptRejectNeed = true;
                        break;

                    case Order.OrderStatus.READY_TO_EXECUTE:
                        IsSerialVisible = true;
                        break;
                }

                Order = order;
            };

            #region commands
            acceptOrderCmd = ReactiveCommand.CreateFromTask(async () => {
                try
                {
                    string serialnumber = await serverApi.GenerateSerialNumber(Order.order_num, Order.amount_aux, Order.comment);
                } catch (Exception ex)
                {                    
                    showError(ex.Message);
                }
            });
            rejectOrderCmd = ReactiveCommand.CreateFromTask(async () => {
                try
                {
                    await serverApi.SetOrderStatus(Order.order_num, Order.OrderStatus.REJECTED, Order.comment);
                } catch (Exception ex)
                {
                    showError(ex.Message);
                }
            });
            #endregion

        }
    }
}
