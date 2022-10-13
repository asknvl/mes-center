using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.ViewModels.dialogs
{
    public class dialogVM : DialogViewModelBase
    {
        #region properties
        object? content;
        public object? Content
        {
            get => content;
            set => this.RaiseAndSetIfChanged(ref content, value);
        }
        #endregion

        public dialogVM(LifeCycleViewModelBase vm) {
            Content = vm;        
        }

        public override void OnStarted()
        {
            base.OnStarted();
            var content = Content as LifeCycleViewModelBase;
            content.CloseRequestEvent += () => {
                Close();
            };
            content?.OnStarted();
        }

        #region public
        #endregion
    }
}
