using mes_center.Models;
using mes_center.Models.logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.ViewModels
{
    public abstract class LifeCycleViewModelBase : ViewModelBase
    {
        #region vars
        protected ILogger logger = Logger.getInstance();
        protected ApplicationContext AppContext = ApplicationContext.getInstance();
        #endregion

        public LifeCycleViewModelBase()
        {
        }
        #region public
        public virtual void OnStarted() {
            logger.dbg(Tags.INTF, $"{this.GetType().Name} started");
        }

        public virtual void OnStopped()
        {
            logger.dbg(Tags.INTF, $"{this.GetType().Name} close");
        }

        public void Close()
        {
            CloseRequestEvent?.Invoke();
        }        
        #endregion

        #region callbacks
        public event Action CloseRequestEvent;
        #endregion
    }
}
