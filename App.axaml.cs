using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using mes_center.arm_acceptorder.ViewModels;
using mes_center.arm_breakdown.ViewModels;
using mes_center.arm_packing.ViewModels;
using mes_center.arm_regmeter.ViewModels;
using mes_center.arm_regorder.ViewModels;
using mes_center.arm_repair.ViewModels;
using mes_center.ViewModels;
using mes_center.Views;
using mes_center.WS;

namespace mes_center
{
    public partial class App : Application
    {

        IWindowServeice ws = WindowService.getInstance();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                LifeCycleViewModelBase main = null;



#if (REGMETER_DEBUG || REGMETER_RELEASE)
                main = new regmeterVM();    
#elif (BREAKDOWN_DEBUG || BREAKDOWN_RELEASE)
                main = new breakdownVM();
#elif (CENTER_DEBUG || CENTER_RELEASE)
                main = new mainVM();
#elif (REPAIR_DEBUG || REPAIR_RELEASE)
                main = new repairVM();
#elif (PACKING_DEBUG || PACKING_RELEASE)
                main = new packingVM();
#elif (REGORDER_DEBUG || REGORDER_RELEASE)
                main = new regorderVM();
#elif (ACCEPTORDER_DEBUG || ACCEPTORDER_RELEASE)
                main = new acceptorderVM();
#endif

                ws.ShowWindow(main);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
