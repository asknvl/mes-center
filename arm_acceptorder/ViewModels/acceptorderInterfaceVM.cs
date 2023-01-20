using mes_center.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.arm_acceptorder.ViewModels
{
    public class acceptorderInterfaceVM : LifeCycleViewModelBase
    {
        #region properties
        public ordersListVM orders { get; set; }
        public orderDetailsVM orderDetails { get; set; }
        public strategiesListVM strategies { get; set; }

        #endregion

        public acceptorderInterfaceVM()
        {
            orders = new ordersListVM();
            orderDetails = new orderDetailsVM(orders);
            strategies = new strategiesListVM();
        }

        public override void OnStarted()
        {
            orders.Reload();
            strategies.Reload();
            base.OnStarted();
        }

    }
}
