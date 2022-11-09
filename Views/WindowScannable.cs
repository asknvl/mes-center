using Avalonia.Controls;
using Avalonia.Input;
using mes_center.Models.scanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Views
{
    public class WindowScannable : Window
    {        
        protected override void OnTextInput(TextInputEventArgs e)
        {
            IScanner scanner = (IScanner)DataContext;
            scanner.OnScan(e.Text);
            base.OnTextInput(e);
        }
    }
}
