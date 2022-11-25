using mes_center.Models.scanner;
using mes_center.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.arm_repair.ViewModels
{
    public class repairVM : LifeCycleViewModelBase, IScanner
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

        public repairVM()
        {
            login = new loginVM();
            login.LoginSucceededEvent += () => {                
            };

            Content = login;
        }

        #region public

        public void OnScan(string text)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
