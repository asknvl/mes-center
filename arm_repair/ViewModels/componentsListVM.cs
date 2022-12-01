using mes_center.Models.rest.server_dto;
using mes_center.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.arm_repair.ViewModels
{
    public class componentsListVM : ViewModelBase
    {
        #region vars
        string SN, order_num;
        #endregion

        #region properties
        public ObservableCollection<componentListItem> Components { get; } = new();

        componentListItem componentListItem;
        public componentListItem Component
        {
            get => componentListItem;
            set => this.RaiseAndSetIfChanged(ref componentListItem, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> addCmd { get; }
        public ReactiveCommand<Unit, Unit> removeCmd { get; }
        #endregion

        public componentsListVM(string sn, string order_num)
        {
            SN = sn;
            this.order_num = order_num;

            #region commands
            addCmd = ReactiveCommand.CreateFromTask(async () => {

                try
                {
                    var order = await serverApi.GetOrder(order_num);
                    var avaliable_components = await serverApi.GetComponents(order.model);

                } catch (Exception ex)
                {
                    showError(ex.Message);
                }

            });

            removeCmd = ReactiveCommand.CreateFromTask(async () => { 
            });
            #endregion
        }

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

        #region callbacks
        public event Action AddComponentRequest;
        #endregion
    }
}
