using Avalonia.Controls;

namespace mes_center.Views.dialogs
{
    public partial class addComponentDlg : WindowScannable
    {
        public addComponentDlg()
        {
            InitializeComponent();
            var comboBox = this.FindControl<ComboBox>("cbSelectType");
            comboBox.PointerPressed += (s, e) =>
             {
                 Focus();
             };

            //comboBox.SelectionChanged += (s, e) =>
            //{
            //    Focus();
            //};
        }
    }
}
