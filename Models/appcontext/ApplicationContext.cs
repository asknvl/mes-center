using mes_center.Models.rest;
using mes_center.Models.scanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models
{
    public class ApplicationContext
    {
        #region properties
        public User User { get; set; }
        public IServerApi ServerApi { get; set; }                  
        #endregion

        #region singletone
        static ApplicationContext instance;
        private ApplicationContext() { }

        public static ApplicationContext getInstance()
        {
            if (instance == null)
                instance = new ApplicationContext();
            return instance;
        }
        #endregion

    }
}
