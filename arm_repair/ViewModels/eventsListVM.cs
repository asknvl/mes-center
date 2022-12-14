using mes_center.Models.rest.server_dto;
using mes_center.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.arm_repair.ViewModels
{
    public class eventsListVM : ViewModelBase
    {
        #region vars
        string SN;
        #endregion

        #region properties
        ObservableCollection<eventListItem> Events { get; } = new();
        #endregion

        #region commands
        #endregion

        public eventsListVM(string sn)
        {
            SN = sn;
        }

        public async Task Update()
        {
            Events.Clear();

            try
            {
                var eventDTOs = await serverApi.GetMeterEvents(SN);
                var stages = await serverApi.GetStages();
                stages.Add(new StageDTO()
                {
                    code = 255,
                    name = "Ремонт"
                });

                foreach (var dto in eventDTOs)
                {
                    Events.Add(new eventListItem()
                    {
                        date = dto.finish_dt,
                        stage = stages.FirstOrDefault(s => s.code == dto.stagecode)?.name,
                        data = dto.data,
                        comment = dto.comment,
                        isOk = dto.is_ok,
                        equipmentName = dto.equipment_name,
                        operatorName = dto.operator_name,
                        operatorPhone = dto.operator_phone
                    });
                }

            } catch (Exception ex)
            {
                showError(ex.Message);
            }
        }

    }
}
