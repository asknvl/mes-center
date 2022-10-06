using Avalonia.Controls;
using Avalonia.Input;
using mes_center.arm_regmeter.ViewModels;
using mes_center.Models.scanner;
using System.Diagnostics;
using System.Timers;

namespace mes_center.arm_regmeter.Views
{
    public partial class regmeterWnd : Window
    {

        #region vars                      
        #endregion

        public regmeterWnd()
        {
            InitializeComponent();
        }

        string text = "";
        protected override void OnTextInput(TextInputEventArgs e)
        {
            IScanner scanner = (IScanner)DataContext;
            scanner.OnScan(e.Text);
            base.OnTextInput(e);
        }
    }
}
