using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.logger
{
    public class Logger : ILogger
    {
        private static Logger instance;

        private Logger()
        {
        }

        public static Logger getInstance()
        {
            if (instance == null)
                instance = new Logger();
            return instance;
        }

        public void dbg(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
