using mes_center.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.arm_regmeter.ViewModels
{
    public class userMessageVM : ViewModelBase
    {
        #region properties
        string message;
        public string Message
        {
            get => message;
            set => this.RaiseAndSetIfChanged(ref message, value);
        }
        #endregion

        public userMessageVM()
        {
        }
    }
}
