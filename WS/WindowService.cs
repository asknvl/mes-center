using Avalonia.Controls;
using mes_center.arm_breakdown.ViewModels;
using mes_center.arm_breakdown.Views;
using mes_center.arm_regmeter.ViewModels;
using mes_center.arm_regmeter.Views;
using mes_center.arm_repair.ViewModels;
using mes_center.arm_repair.Views;
using mes_center.ViewModels;
using mes_center.ViewModels.dialogs;
using mes_center.Views;
using mes_center.Views.dialogs;
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
        Window mainWindow;

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

        public void ShowDialog(DialogViewModelBase vm)
        {
            Window wnd = null;

            switch (vm)
            {
                case addStrategyDlgVM:
                    wnd = new addStrategyDlg();
                    break;

                case dialogVM:
                    wnd = new containerDlg();
                    break;
            }

            vm.CloseRequestEvent += () =>
            {
                if (wnd != null)
                    wnd.Close();
            };

            wnd.DataContext = vm;

            wnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            wnd.ShowDialog(mainWindow);
            vm.OnStarted();
        }

        public void ShowWindow(LifeCycleViewModelBase vm)
        {

            Window wnd = null;

            switch (vm)
            {
                case mainVM:
                    wnd = new mainWnd();                    
                    break;

                case regmeterVM:
                    wnd = new regmeterWnd();                    
                    break;

                case breakdownVM:
                    wnd = new breakdownWnd();
                    break;

                case repairVM:
                    wnd = new repairWnd();
                    break;
            }

            mainWindow = wnd;
            wnd.Closing += (s, e) => {
                vm.OnStopped();
            };

            vm.CloseRequestEvent += () =>
            {
                if (wnd != null)
                {                  
                    wnd.Close();
                }                
            };

            wnd.DataContext = vm;            
            wnd.Show();

            vm.OnStarted();
        }
        #endregion
    }
}
