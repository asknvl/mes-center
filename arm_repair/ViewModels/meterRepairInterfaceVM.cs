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
        string order_num;        
        int SessionID = 0;
        #endregion

        #region properties
        componentsListVM meterComponents;
        public componentsListVM MeterComponents
        {
            get => meterComponents;
            set => this.RaiseAndSetIfChanged(ref meterComponents, value);
        }
        #endregion        
        public meterRepairInterfaceVM(int session, string sn, string order_num)
        {
            SessionID = session;
            SN = sn;
            this.order_num = order_num;
            MeterComponents = new componentsListVM(sn, order_num);
        }

        #region private      
        #endregion

        #region public
        public async Task OnStarted()
        {
            base.OnStarted();
            logger.inf(Tags.INTF, $"Meter SN={SN} order_num={order_num} repairing started");
            await Update();
        }


        public async Task Update()
        {
            try
            {
                var components = await serverApi.GetComponents(SN);
                MeterComponents.Update(components);
            } catch (Exception ex)
            {
                showError(ex.Message);
            }
        }
        #endregion

    }
}
