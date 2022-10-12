using mes_center.ViewModels;
using System;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mes_center.Models.rest.server_dto;
using mes_center.Models.scanner;
using System.Diagnostics;

namespace mes_center.arm_regmeter.ViewModels
{
    public class regmeterVM : LifeCycleViewModelBase, IScanner
    {
        #region vars
        loginVM login;        
        #endregion

        #region properties        
        object? content;

        public event Action<string> OnScanEvent;

        public object? Content
        {
            get => content;
            set {

                var c = content as LifeCycleViewModelBase;
                if (c != null)
                    c.OnStopped();

                var lcc = value as LifeCycleViewModelBase;                
                if (lcc != null)
                {                    
                    lcc.OnStarted();
                }

                this.RaiseAndSetIfChanged(ref content, value);
            }
        }      
        #endregion
        public regmeterVM()
        {
            login = new loginVM();
            login.LoginSucceededEvent += () => {
                showOrderSelection();
            };

            Content = login;

            //Content = new meterRegistrationVM();

        }
        #region helpers
        void showOrderSelection()
        {
            var os = new orderSelectionVM();
            os.CloseRequestEvent += () => {
                Content = login;
            };

            os.OrderSelectedEvent += (order) => {                
                showMeterRegistration(order);
            };
            Content = os;
        }

        async void showMeterRegistration(OrderDTO order)
        {
            var total_amount = await serverApi.GetMetersAmount(order.order_num, 1);
            if (total_amount > 0)
            {
                var mr = new meterRegistrationVM();
                mr.CloseRequestEvent += () =>
                {
                    showOrderSelection();
                };
                mr.Order = order;
                Content = mr;
            }
            else
                showError("Данное задание уже выполнено");
        }

        public void OnScan(string text)
        {
            if (Content is meterRegistrationVM)
            {
                IScanner scanner = (IScanner)Content;
                scanner.OnScan(text);
            }
        } 
        #endregion
    }
}
