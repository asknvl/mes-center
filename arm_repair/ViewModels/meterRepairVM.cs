using mes_center.Models.rest.server_dto;
using mes_center.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.arm_repair.ViewModels
{
    public enum State
    {
        waitingMeterSN,
        meterRepairing,
        waitingComponentSN,
    }

    public class meterRepairVM : ScannerViewModelBase
    {
        #region vars
        State state = State.waitingMeterSN;
        int SessionID = 0;
        #endregion

        #region properties
        object content;
        public object Content
        {
            get => content;
            set => this.RaiseAndSetIfChanged(ref content, value);
        }

        string sn;
        public string SN
        {
            get => sn;
            set => this.RaiseAndSetIfChanged(ref sn, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> cancelCmd { get; }
        #endregion

        public meterRepairVM(int session)
        {
            SessionID = session;

            cancelCmd = ReactiveCommand.CreateFromTask(async () => {
                switch (state)
                {
                    case State.waitingMeterSN:

                        try
                        {
                            await serverApi.CloseSession(SessionID);

                        } catch (Exception ex)
                        {
                            showError(ex.Message);
                        }

                        Close();
                        break;
                    case State.meterRepairing:
                        showGetNextSN();
                        break;
                }
            });

            showGetNextSN(); 
        }

        #region helpers
        void showGetNextSN()
        {
            SN = "";
            state = State.waitingMeterSN;

            Content = new userMessageVM()
            {
                Message = "Отсканируйте прибор учета"
            };
        }

        async Task startMeterRepair(int session, string sn, string order_num)
        {
            meterRepairInterfaceVM vm = new meterRepairInterfaceVM(session, sn, order_num);
            Content = vm;
            await vm.OnStarted();
            SN = sn;
            state = State.meterRepairing;
        }
        #endregion

        #region public
        protected async override void OnData(string sn)
        {
            switch (state)
            {
                case State.waitingMeterSN:

                    MeterInfoDTO info = null;

                    try
                    {
                        info = await serverApi.GetMeterInfo(sn, 255);
                    }
                    catch (Exception ex)
                    {
                        showError("Прибор не может быть отремонтирован");
                        return;
                    }

                    await startMeterRepair(SessionID, sn, info.order_num);
                    break;

                default:
                    break;
            }
        }
        #endregion
    }
}
