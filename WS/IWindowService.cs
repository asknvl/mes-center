using mes_center.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.WS {

    public interface IWindowServeice
    {
        void ShowWindow(LifeCycleViewModelBase vm);
        void ShowDialog(LifeCycleViewModelBase vm);
    }

}

