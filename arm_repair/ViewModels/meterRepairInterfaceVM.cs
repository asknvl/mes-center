using mes_center.Models.logger;
using mes_center.Models.rest.server_dto;
using mes_center.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.arm_repair.ViewModels
{
    public class meterRepairInterfaceVM : LifeCycleViewModelBase
    {
        #region vars
        string SN;
        #endregion

        #region properties
        componentsListVM meterComponents = new();
        public componentsListVM MeterComponents
        {
            get => meterComponents;
            set => this.RaiseAndSetIfChanged(ref meterComponents, value);
        }
        #endregion        
        public meterRepairInterfaceVM(string sn)
        {
            SN = sn;
        }
        #region public
        public async Task OnStarted()
        {
            base.OnStarted();
            logger.inf(Tags.INTF, $"Meter SN={SN} repairing started");

            var components = await serverApi.GetComponents(SN);
            MeterComponents.Update(components);

        }
        #endregion
    }
}
