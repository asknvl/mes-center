using mes_center.arm_regmeter.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.ViewModels
{
    public class loginVM : LifeCycleViewModelBase
    {
        #region properties
        string login;
        public string Login
        {
            get => login;
            set => this.RaiseAndSetIfChanged(ref login, value); 
        }

        string password;
        public string Password
        {
            get => password;
            set => this.RaiseAndSetIfChanged(ref password, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> loginCmd { get; }
        #endregion
        public loginVM() {

            Login = "tester_login";
            Password = "tester_password";

            #region commands
            loginCmd = ReactiveCommand.CreateFromTask(async () => {
                AppContext.User = new User { 
                    Login = Login,
                    Password = Password
                };
                LoginSucceededEvent?.Invoke();
            });
            #endregion

        }

        #region events
        public event Action LoginSucceededEvent;
        #endregion

    }
}
