using mes_center.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.arm_acceptorder.ViewModels
{
    public class acceptorderVM : LifeCycleViewModelBase
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

        public acceptorderVM()
        {
            login = new loginVM();
            login.LoginSucceededEvent += async () => {

                try
                {
                    showAcceptOrder();
                }
                catch (Exception ex)
                {
                    showError(ex.Message);
                }
            };

            Content = login;

        }
        #endregion

        #region helpers
        void showAcceptOrder()
        {
            acceptorderInterfaceVM vm = new acceptorderInterfaceVM();
            Content = vm;
        }
        #endregion

    }
}
