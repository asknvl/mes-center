using mes_center.Models.rest.server_dto;
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
    public class componentsListVM : ViewModelBase
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

        #region public
        public void Update(List<MeterComponentDTO> dtos)
        {
            Components.Clear();
            foreach (var dto in dtos)
            {
                var component = new componentListItem()
                {
                    name = dto.name,
                    sn = dto.sn,
                    status = dto.status
                };
                Components.Add(component);
            }
        }
        #endregion
    }
}
