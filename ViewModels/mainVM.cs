using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.ViewModels
{
    public class mainVM : LifeCycleViewModelBase
    {
        #region properties
        public taskVM task { get; set; }
        public ordersListVM orders { get; set; }
        public orderDetailsVM orderDetails { get; set; }
        public strategiesListVM strategies { get; set; }
        #endregion

        public mainVM()
        {
            task = new taskVM();
            orders = new ordersListVM();
            orderDetails = new orderDetailsVM(orders);
            strategies = new strategiesListVM();
        }

        public override void OnStarted()
        {
            task.Reload();
            orders.Reload();
            strategies.Reload();
            base.OnStarted();
        }
    }
}
