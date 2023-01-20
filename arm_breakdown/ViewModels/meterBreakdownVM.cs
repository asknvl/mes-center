using mes_center.Models.kafka.kafka_dto;
using mes_center.Models.logger;
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

        public meterBreakdownVM() : base()
        {
            #region commands
            okCmd = ReactiveCommand.CreateFromTask(async () =>
            {
                try
                {
                    var c = Content as userActionVM;
                    if (c != null)
                    {
                        await markMeterBreakdown(SessionID, c.SN, true);
                    }
                }
                catch (Exception ex)
                {
                    showError(ex.Message);
                }
                nextScanRequest();
            });

            trashCmd = ReactiveCommand.CreateFromTask(async () =>
            {
                try
                {
                    var c = Content as userActionVM;
                    if (c != null)
                    {
                        await markMeterBreakdown(SessionID, c.SN, false);
                    }

                } catch (Exception ex)
                {
                    showError(ex.Message);
                }
                
                nextScanRequest();
            });

            finishCmd = ReactiveCommand.CreateFromTask( async () =>
            {
                Close();
            });
            #endregion
        }

        #region heplers
        void nextScanRequest()
        {
            Content = new userActionVM() { Text = "Отсканируйте серийный номер счетчика", SN = "" };

            startTime = DateTime.UtcNow;

            AllowButtons = false;
        }

        async Task markMeterBreakdown(int sessionID, string sn, bool isOk)
        {
            MeterDTO meterDTO = new MeterDTO(SessionID,
                                                      2, //TODO +enum
                                                      isOk,
                                                      startTime,
                                                      DateTime.UtcNow,
                                                      sn);

            await prodApi.SetMeterStagePassed(sessionID, meterDTO);
        }
        #endregion

        #region override
        public override async void OnStarted()
        {
            base.OnStarted();
            startTime = DateTime.UtcNow;
            try
            {
                SessionID = await prodApi.OpenSession(Order.order_num, AppContext.User.Login, null);
                nextScanRequest();
            }
            catch (Exception ex)
            {
                showError(ex.Message);
            }
        }

        public override async void OnStopped()
        {
            base.OnStopped();
            try
            {
                await prodApi.CloseSession(SessionID);
            }
            catch (Exception ex)
            {
                showError(ex.Message);
            }
        }

        protected override void OnOk()
        {
            base.OnOk();
            okCmd.Execute();
            AllowButtons = false;
        }

        protected override void OnTrash()
        {
            base.OnTrash();
            trashCmd.Execute();
            AllowButtons = false;
        }

        protected override void OnFinish()
        {
            base.OnFinish();
            finishCmd.Execute();
            AllowButtons = false;
        }

        protected override void OnData(string data)
        {

            if (AllowButtons)
                return;

            logger.inf(Tags.SCAN, data);

            AllowButtons = true;

            var ua = new userActionVM()
            {
                Text = "Прибор исправен?",
                SN = data
            };

            Content = ua;

            AllowButtons = true;
        }
        #endregion
    }
}
