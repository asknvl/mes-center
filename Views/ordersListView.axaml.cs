using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace mes_center.Views
{
    public partial class ordersListView : UserControl
    {
        public ordersListView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

    }
}
