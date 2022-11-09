using mes_center.Models.rest.server_dto;
using mes_center.Models.scanner;
using mes_center.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.arm_breakdown.ViewModels
{
    public class breakdownVM : LifeCycleViewModelBase, IScanner
    {
        #region vars
        loginVM login;
        #endregion

        #region properties
        object? content;
        public object? Content
        {
            get => content;
            set {
                var c = content as LifeCycleViewModelBase;
                if (c != null)
                    c.OnStopped();

                var lcc = value as LifeCycleViewModelBase;
                if (lcc != null)                
                    lcc.OnStarted();

                this.RaiseAndSetIfChanged(ref content, value);
            }
        }
        #endregion

        public breakdownVM()
        {
            login = new loginVM();
            login.LoginSucceededEvent += () => {
                showOrderSelection();
            };

            Content = login;
        }

        #region helpers
        void showOrderSelection()
        {
            var os = new orderSelectionVM();
            os.CloseRequestEvent += () => {
                Content = login;
            };

            os.OrderSelectedEvent += (order) => {
                showMeterBreakdown(order);
            };
            Content = os;
        }

        async void showMeterBreakdown(OrderDTO order)
        {

            var vm = new meterBreakdownVM();
            vm.CloseRequestEvent += () => {
                showOrderSelection();
            };
            vm.Order = order;
            Content = vm;

            //var total_amount = await serverApi.GetMetersAmount(order.order_num, 2);
            //if (total_amount > 0)
            //{
            //    var mr = new meterBreakdownVM();
            //    mr.CloseRequestEvent += () =>
            //    {
            //        showOrderSelection();
            //    };
            //    mr.Order = order;
            //    Content = mr;
            //}
            //else
            //    showError("Данное задание уже выполнено");
        }
        #endregion

        #region public
        public void OnScan(string text)
        {
            var scanner = Content as IScanner;
            if (scanner != null)
                scanner.OnScan(text);
        }
        #endregion

    }
}
