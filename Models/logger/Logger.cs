using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace mes_center.Models.logger
{
    public class Logger : ILogger
    {

        #region vars
        private static Logger instance;
        string logPath;
        Queue<string> messages = new();
        Timer timer = new Timer();               
        #endregion


        private Logger()
        {
            string logDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
            logPath = Path.Combine(Directory.GetCurrentDirectory(), "logs", $"{DateTime.Now.ToString("yyyy-MM-dd_HHmmss")}.txt");

            timer.Interval = 3000;
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Start();

        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            while (messages.Count > 0)
            {
                string msg = messages.Dequeue();
                File.AppendAllText(logPath, $"{msg}\n");
            }
        }

        public static Logger getInstance()
        {
            if (instance == null)
                instance = new Logger();
            return instance;
        }

        public void dbg(Tags tag, string message)
        {
            string msg = $"{DateTime.Now} DBG > {tag} : {message}";            
            messages.Enqueue(msg);
            Debug.WriteLine(msg);
        }

        public void err(Tags tag, string message)
        {
            string msg = $"{DateTime.Now} ERR > {tag} : {message}";            
            messages.Enqueue(msg);
            Debug.WriteLine(msg);
        }

        public void inf(Tags tag, string message)
        {
            string msg = $"{DateTime.Now} INF > {tag} : {message}";            
            messages.Enqueue(msg);
            Debug.WriteLine(msg);
        }

    }
}
