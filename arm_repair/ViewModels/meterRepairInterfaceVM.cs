using mes_center.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.arm_repair.ViewModels
{
    public class meterRepairInterfaceVM : LifeCycleViewModelBase
    {
        #region vars
        string SN;
        #endregion

        #region properties
        #endregion
        public meterRepairInterfaceVM(string sn)
        {
            SN = sn;
        }
        #region public
        public async Task OnStarted()
        {
            base.OnStarted();
            


        }
        #endregion
    }
}
