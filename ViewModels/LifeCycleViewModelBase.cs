﻿using mes_center.arm_regmeter.Models;
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
            logger.dbg($"{this.GetType().Name} started");
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
