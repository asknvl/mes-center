using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.scanner
{
    public class Scanner : IScanner
    {
        public event Action<string> OnScanEvent;

        public void OnCancel()
        {
            throw new NotImplementedException();
        }

        public void OnRegister()
        {
            throw new NotImplementedException();
        }

        public void OnScan(string text)
        {            
        }

        public void OnTrash()
        {
            throw new NotImplementedException();
        }
    }
}
