using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.logger
{
    public interface ILogger
    {
        void dbg(string message);
        void err(string message);
    }
}
