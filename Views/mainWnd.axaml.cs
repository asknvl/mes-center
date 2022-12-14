using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace mes_center.Views
{
    public partial class mainWnd : Window
    {
        public mainWnd()
        {
            InitializeComponent();
#if DEBUG
            //this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
