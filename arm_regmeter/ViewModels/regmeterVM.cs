using mes_center.ViewModels;
using System;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mes_center.Models.rest.server_dto;

namespace mes_center.arm_regmeter.ViewModels
{
    public class regmeterVM : LifeCycleViewModelBase
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

            //Content = login;

            Content = new meterRegistrationVM();

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

        void showMeterRegistration(Order order)
        {
            var mr = new meterRegistrationVM();
            mr.Order = order;
            Content = mr;
        }
        #endregion
    }
}
