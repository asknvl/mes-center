using mes_center.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.arm_repair.ViewModels
{
    public class componentListItem : ViewModelBase
    {
        #region properies
        int _id;
        public int id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        string _uuid;
        public string uuid
        {
            get => _uuid;
            set => this.RaiseAndSetIfChanged(ref _uuid, value);
        }

        string _name;
        public string name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        string _sn;
        public string sn
        {
            get => _sn;
            set => this.RaiseAndSetIfChanged(ref _sn, value);
        }

        bool _status;
        public bool status
        {
            get => _status;
            set => this.RaiseAndSetIfChanged(ref _status, value);   
        }
        #endregion
    }
}
