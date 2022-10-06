using mes_center.Models.rest.server_dto;
using mes_center.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.arm_regmeter.ViewModels
{
    public class componentItemVM : ViewModelBase, IReloadable
    {
        #region properties       
        int id;
        public int Id
        {
            get => id;
            set => this.RaiseAndSetIfChanged(ref id, value);
        }

        string name;
        public string Name
        {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value);
        }

        string serialNumber;
        public string SerialNumber
        {
            get => serialNumber;
            set => this.RaiseAndSetIfChanged(ref serialNumber, value);
        }

        string actionName;
        public string ActionName
        {
            get => actionName;
            set => this.RaiseAndSetIfChanged(ref actionName, value);
        }
        #endregion

        static int cntr = 0;

        public componentItemVM(ComponentDTO dto)
        {
            Name = $"{dto.name}";
            
        }

        public componentItemVM(string name)
        {
            Name = $"{name}";
        }

        #region public
        public Task Reload()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region events
        public event Action<bool> ComponentDoneEvent;
        #endregion
    }
}
