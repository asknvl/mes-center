using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace mes_center.Views
{
    public partial class taskView : UserControl
    {
        public taskView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
