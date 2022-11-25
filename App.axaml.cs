using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using mes_center.arm_breakdown.ViewModels;
using mes_center.arm_regmeter.ViewModels;
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

#if REGMETER_DEBUG
                main = new regmeterVM();
#elif BREAKDOWN_DEBUG
                main = new breakdownVM();
#elif CENTER_DEBUG
                main = new mainVM();
#elif REPAIR_DEBUG
                main = new repairVM();
#endif

                ws.ShowWindow(main);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
