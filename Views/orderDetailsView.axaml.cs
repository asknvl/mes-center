using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace mes_center.Views
{
    public partial class orderDetailsView : UserControl
    {
        public orderDetailsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
