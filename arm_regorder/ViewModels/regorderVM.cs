using mes_center.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.arm_regorder.ViewModels
{
    public class regorderVM : LifeCycleViewModelBase
    {
        #region vars
        loginVM login;
        #endregion

        #region properties
        object? content;
        public object? Content
        {
            get => content;
            set
            {
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

        public regorderVM()
        {
            login = new loginVM();
            login.LoginSucceededEvent += async () => {

                try
                {
                    showRegOrder();
                }
                catch (Exception ex)
                {
                    showError(ex.Message);
                }
            };

            Content = login;
        }

        #region helpers
        void showRegOrder()
        {
            regorderInterfaceVM ro = new regorderInterfaceVM();
            ro.CloseRequestEvent += () => {
                Content = login;
            };
            Content = ro;
        }
        #endregion        
    }
}
