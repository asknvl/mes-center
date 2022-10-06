using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.scanner
{
    public interface IScanner
    {
        void OnScan(string text);                
    }
}
