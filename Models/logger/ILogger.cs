using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.logger
{
    public interface ILogger
    {
        void dbg(Tags tag, string message);
        void err(Tags tag, string message);
        void inf(Tags tag, string message);
    }

    public enum Tags
    {
        INTF, //Interface actions
        SAPI, //Api requests
        SCAN, //Scanner
        KAFK
    }
}
