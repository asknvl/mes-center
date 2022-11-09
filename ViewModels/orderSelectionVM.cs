using mes_center.Models.rest.server_dto;
using mes_center.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.ViewModels
{
    public class orderSelectionVM : LifeCycleViewModelBase
    {
        #region properies
        ordersListVM orders;
        public ordersListVM Orders
        {
            get => orders;
            set => this.RaiseAndSetIfChanged(ref orders, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> selectOrderCmd { get; }
        public ReactiveCommand<Unit, Unit> cancelCmd { get; }
        #endregion

        public orderSelectionVM()
        {

            Orders = new ordersListVM();
            Orders.OrderStatuses = new OrderDTO.OrderStatus[] { OrderDTO.OrderStatus.READY_TO_EXECUTE };

            #region commands
            selectOrderCmd = ReactiveCommand.Create(() => {
                if (Orders.Order != null)
                    OrderSelectedEvent?.Invoke(Orders.Order);
            });

            cancelCmd = ReactiveCommand.Create(() => {
                Close();
            });
            #endregion
        }

        #region override
        public override void OnStarted()
        {
            base.OnStarted();
            Orders.Reload();
        }
        #endregion

        #region events
        public event Action<OrderDTO> OrderSelectedEvent;
        #endregion
    }
}
