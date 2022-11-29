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
        #endregion

        #region properties
        object content;
        public object Content
        {
            get => content;
            set => this.RaiseAndSetIfChanged(ref content, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> cancelCmd { get; }
        #endregion

        public meterRepairVM()
        {
            cancelCmd = ReactiveCommand.Create(() => {
                Close();
            });

            showGetNextSN(); 
        }

        #region helpers
        void showGetNextSN()
        {
            state = State.waitingMeterSN;

            Content = new userMessageVM()
            {
                Message = "Отсканируйте прибор учета"
            };
        }

        void startMeterRepair(string sn)
        {
            meterRepairInterfaceVM vm = new meterRepairInterfaceVM(sn);
            Content = vm;

            vm.OnStarted();

            state = State.meterRepairing;
        }
        #endregion

        #region public
        protected override void OnData(string sn)
        {
            switch (state)
            {
                case State.waitingMeterSN:
                    startMeterRepair(sn);
                    break;

                case State.waitingComponentSN:
                    break;

                default:
                    break;
            }
        }
        #endregion
    }
}
