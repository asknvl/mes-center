using mes_center.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.arm_repair.ViewModels
{
    public class componentsListVM : ViewModelBase, IReloadable
    {
        #region properties
        public ObservableCollection<componentListItem> Components { get; } = new();

        componentListItem componentListItem;
        public componentListItem Component
        {
            get => componentListItem;
            set => this.RaiseAndSetIfChanged(ref componentListItem, value);
        }
        #endregion

        public Task Reload()
        {
            throw new NotImplementedException();
        }
    }
}
