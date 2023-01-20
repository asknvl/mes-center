using Avalonia.Threading;
using mes_center.Models.kafka;
using mes_center.Models.logger;
using mes_center.Models.rest;
using mes_center.ViewModels.dialogs;
using mes_center.WS;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace mes_center.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject, INotifyDataErrorInfo
    {
        #region vars
        protected ILogger logger = Logger.getInstance();

        protected IServerApi centrApi = new ServerApi("http://172.16.118.105:8080/assppu-cent-1.1.0");
        protected IServerApi prodApi = new ServerApi("http://172.16.118.105:8080/assppu-prod-1.1.0");

        protected IWindowServeice ws = WindowService.getInstance();
        #endregion

        public ViewModelBase()
        {

        }
                
        #region error validattion
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        // get errors by property
        public IEnumerable GetErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
                return _errors[propertyName];
            return null;
        }

        public bool HasErrors => _errors.Count > 0;

        // object is valid
        public bool IsValid => !HasErrors;

        public void AddError(string propertyName, string error)
        {
            // Add error to list
            _errors[propertyName] = new List<string>() { error };
            NotifyErrorsChanged(propertyName);
        }

        public void RemoveError(string propertyName)
        {
            // remove error
            if (_errors.ContainsKey(propertyName))
                _errors.Remove(propertyName);
            NotifyErrorsChanged(propertyName);
        }

        public void NotifyErrorsChanged(string propertyName)
        {
            // Notify
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }
        #endregion        

        public virtual async void showError(string message) {

            //logger.err(message);

            var msg = new msgVM();
            msg.Title = "Îøèáêà!";
            msg.Message = message;
            var dlg = new dialogVM(msg);

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                ws.ShowDialog(dlg);
            });

        }
    }
}
