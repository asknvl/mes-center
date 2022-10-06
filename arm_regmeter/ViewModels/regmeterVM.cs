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
                this.RaiseAndSetIfChanged(ref content, value);                
                var lcc = content as LifeCycleViewModelBase;
                if (lcc != null)
                    lcc.OnStarted();                                      
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

        void showMeterRegistration(OrderDTO order)
        {
            var mr = new meterRegistrationVM();
            mr.CloseRequestEvent += () =>
            {
                showOrderSelection();
            };
            mr.Order = order;
            Content = mr;
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
