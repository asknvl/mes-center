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
        int SessionID;
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
            login.LoginSucceededEvent += async () => {
                SessionID = await serverApi.OpenSession(null, login.Login, null) //TODO
                showMeterRepair(SessionID);
            };

            Content = login;
        }

        #region helpers
        void showMeterRepair(int sessionid)
        {
            var vm = new meterRepairVM(sessionid);
            vm.CloseRequestEvent += () => {
                Content = login;
            };            
            Content = vm;
        }
        #endregion

        #region public
        public void OnScan(string text)
        {
            var c = Content as IScanner;
            if (c != null)
                c.OnScan(text);
        }

        public override void OnStopped()
        {
            base.OnStopped();
        }
        #endregion
    }
}
