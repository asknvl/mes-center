using Avalonia.Controls;
using Avalonia.Input;
using System.Diagnostics;
using System.Timers;

namespace mes_center.arm_regmeter.Views
{
    public partial class regmeterWnd : Window
    {

        #region vars
        Timer timer = new Timer();
        #endregion

        public regmeterWnd()
        {
            InitializeComponent();
            timer.Interval = 1000;
            timer.AutoReset = false;
            timer.Elapsed += Timer_Elapsed;
            
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Debug.WriteLine(text);
            text = "";
        }

        string text = "";
        protected override void OnTextInput(TextInputEventArgs e)
        {

            timer.Start(); 
            text += e.Text;
            Debug.WriteLine(e.Text);
            base.OnTextInput(e);
        }
    }
}
