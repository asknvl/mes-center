using mes_center.Models.rest.server_dto;
using mes_center.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.arm_regmeter.ViewModels
{
    public class meterRegistrationVM : LifeCycleViewModelBase
    {
        #region properties
        object content;
        public object Content
        {
            get => content;
            set => this.RaiseAndSetIfChanged(ref content, value);
        }

        ObservableCollection<componentItemVM> ComponentsList { get; set; } = new();

        public Order Order { get; set; }
        #endregion

        public meterRegistrationVM()
        {
            Content = new componentItemVM();
        }
        public override async void OnStarted()
        {
            base.OnStarted();

            try
            {

                //var order = serverApi.GetOrder(Order.order_num);
                //var components = await serverApi.GetComponents(order.model);

            } catch (Exception ex)
            {

            }
        }
    }
}
