using Avalonia.Controls;
using mes_center.arm_regmeter.ViewModels;
using mes_center.arm_regmeter.Views;
using mes_center.ViewModels;
using mes_center.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.WS
{
    public class WindowService : IWindowServeice
    {
        #region vars
        static WindowService instance;

        #endregion

        private WindowService()
        {
        }

        #region public
        public static WindowService getInstance()
        {
            if (instance == null)
                instance = new WindowService();
            return instance;
        }

        public void ShowDialog(LifeCycleViewModelBase vm)
        {
            
        }

        public void ShowWindow(LifeCycleViewModelBase vm)
        {

            Window wnd = null;

            switch (vm)
            {
                case mainVM wvm:
                    wnd = new mainWnd();
                    break;

                case regmeterVM rvm:
                    wnd = new regmeterWnd();
                    break;                   
            }

            vm.CloseRequestEvent += () =>
            {
                if (wnd != null)
                    wnd.Close();
            };

            wnd.DataContext = vm;            
            wnd.Show();

            vm.OnStarted();
        }
        #endregion
    }
}
