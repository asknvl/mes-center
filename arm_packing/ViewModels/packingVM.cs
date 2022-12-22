using mes_center.Models.scanner;
using mes_center.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.arm_packing.ViewModels
{
    internal class packingVM : LifeCycleViewModelBase, IScanner
    {
        public void OnScan(string text)
        {            
        }
    }
}
