using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.ViewModels.dialogs
{
    public class addStrategyDlgVM : DialogViewModelBase
    {
        #region properties
        public addstrategyVM addstrategy { get; set; }
        #endregion

        public addStrategyDlgVM() { 
            addstrategy = new addstrategyVM();
        }

        #region public
        #endregion

        public override async void OnStarted()
        {
            base.OnStarted();
            await addstrategy.Reload();
        }
    }
}
