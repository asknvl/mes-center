using mes_center.Models.rest.server_dto;
using mes_center.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.arm_breakdown.ViewModels
{
    public class meterBreakdownVM : ScannerViewModelBase
    {
        #region vars
        DateTime startTime;
        int SessionID = 0;
        #endregion

        #region properties
        object content;
        public object Content
        {
            get => content;
            set => this.RaiseAndSetIfChanged(ref content, value);
        }

        bool allowButtons;
        public bool AllowButtons
        {
            get => allowButtons;
            set => this.RaiseAndSetIfChanged(ref allowButtons, value);
        }
        public OrderDTO Order { get; set; }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> okCmd { get; }
        public ReactiveCommand<Unit, Unit> trashCmd { get; }
        public ReactiveCommand<Unit, Unit> finishCmd { get; }
        #endregion

        public meterBreakdownVM() : base() {
            #region commands
            okCmd = ReactiveCommand.CreateFromTask(async () => { 
            });

            trashCmd = ReactiveCommand.CreateFromTask(async () => {
            });

            finishCmd = ReactiveCommand.CreateFromTask(async () => {
            });
            #endregion
        }

        #region override
        public override async void OnStarted()
        {
            base.OnStarted();

            startTime = DateTime.UtcNow;

            try
            {
                SessionID = await serverApi.OpenSession(Order.order_num, AppContext.User.Login, 1);

                Content = new userActionVM() { Text = "Отсканируйте серийный номер счетчика" };

            } catch (Exception ex)
            {
                showError(ex.Message);
            }


        }

        protected override void OnOk()
        {
            base.OnOk();
        }

        protected override void OnTrash()
        {
            base.OnTrash();
        }

        protected override void OnFinish()
        {
            base.OnFinish();
        }
        #endregion
    }
}
