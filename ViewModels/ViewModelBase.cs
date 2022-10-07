using mes_center.Models.kafka;
using mes_center.Models.logger;
using mes_center.Models.rest;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace mes_center.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject
    {
        #region vars
        protected ILogger logger = Logger.getInstance();
        protected IServerApi serverApi = new ServerApi("http://172.16.118.105:8080/assppu-1.0.7");        
        #endregion

        public ViewModelBase()
        {

        }
        public virtual void showError(string message) {
            logger.err(message);
        }
    }
}
