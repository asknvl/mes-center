using mes_center.Models.scanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace mes_center.ViewModels
{
    public abstract class ScannerViewModelBase : LifeCycleViewModelBase, IScanner
    {
        #region vars
        int clickUpdatePeriod = 1300;
        protected Timer timer = new Timer();
        string text = "";
        #endregion

        public ScannerViewModelBase()
        {
            timer.Interval = clickUpdatePeriod;
            timer.AutoReset = false;
            timer.Elapsed += ScannerTimer_Elpased; ;
        }

        #region protected
        protected virtual void ScannerTimer_Elpased(object? sender, ElapsedEventArgs e)
        {
            switch (text)
            {
                case "255012255":
                    OnOk();
                    break;
                case "255012256":
                    OnTrash();
                    break;
                case "255012257":
                    OnFinish();
                    break;
                default:
                    OnData(text);                    
                    break;
            }

            text = "";
        }

        protected virtual void OnOk()
        {
            logger.dbg("Ok");
        }

        protected virtual void OnTrash()
        {
            logger.dbg("Trash");
        }

        protected virtual void OnFinish()
        {
            logger.dbg("Finish");
        }

        protected abstract void OnData(string data);
        #endregion

        #region public
        public void OnScan(string text)
        {
            if (!timer.Enabled)
                timer.Start();

            this.text += text;

            //logger.dbg(text);
        }
        #endregion
    }
}
