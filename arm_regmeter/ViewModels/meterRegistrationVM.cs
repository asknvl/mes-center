﻿using kafka = mes_center.Models.kafka;
using mes_center.Models.rest.server_dto;
using mes_center.Models.scanner;
using mes_center.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using mes_center.ViewModels.dialogs;
using mes_center.WS;
using Avalonia.Threading;

namespace mes_center.arm_regmeter.ViewModels
{
    public class meterRegistrationVM : LifeCycleViewModelBase, IScanner
    {
        #region const
        int clickUpdatePeriod = 1300;
        #endregion

        #region vars
        Timer timer = new Timer();
        string text = "";
        int counter = 0;
        List<ComponentDTO> meterComponents;        
        List<componentItemVM> componentsList;
        int SessionID = 0;

        //kafka.consumer meter_created_response;

        DateTime regStartTime;

        IWindowServeice ws = WindowService.getInstance();
        #endregion

        #region properties
        object content;
        public object Content
        {
            get => content;
            set => this.RaiseAndSetIfChanged(ref content, value);
        }

        int currentAmount;
        public int CurrentAmount
        {
            get => currentAmount;
            set => this.RaiseAndSetIfChanged(ref currentAmount, value);
        }

        int totalAmount;
        public int TotalAmount
        {
            get => totalAmount;
            set => this.RaiseAndSetIfChanged(ref totalAmount, value);
        }

        bool allowButtons;
        public bool AllowButtons
        {
            get => allowButtons;
            set => this.RaiseAndSetIfChanged(ref allowButtons, value);
        }

        ObservableCollection<componentItemVM> OrderComponentsList { get; set; } = new();       
        public OrderDTO Order { get; set; }
        public ModificationDTO Modification { get; set; }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> completeOrderCmd { get; }
        public ReactiveCommand<Unit, Unit> trashOrderCmd { get; }
        public ReactiveCommand<Unit, Unit> cancelOrderCmd { get; }
        #endregion

        public meterRegistrationVM()
        {
            timer.Interval = clickUpdatePeriod;
            timer.AutoReset = false;
            timer.Elapsed += Timer_Elapsed; ;

            CurrentAmount = 0;
            TotalAmount = 0;

            //meter_created_response = new kafka.consumer(this.GetType().Name);
            //meter_created_response.TopicUpdatedEvent -= Meter_created_response_TopicUpdatedEvent;
            //meter_created_response.TopicUpdatedEvent += Meter_created_response_TopicUpdatedEvent;  

            #region commands
            completeOrderCmd = ReactiveCommand.CreateFromTask(async () => {

                try
                {

                    await markMeterAssembled(true);

                    if (CurrentAmount == TotalAmount)
                        Close();

                    AllowButtons = false;
                } catch (Exception ex)
                {
                    showError(ex.Message);
                }

            });
            trashOrderCmd = ReactiveCommand.CreateFromTask(async () => {

                try
                {

                    await markMeterAssembled(false);

                    if (CurrentAmount == TotalAmount)
                        Close();

                    AllowButtons = false;

                } catch (Exception ex)
                {
                    showError(ex.Message);
                }

            });
            cancelOrderCmd = ReactiveCommand.CreateFromTask(async () => {

                try
                {

                    await prodApi.CloseSession(SessionID);

                    Close();
                }
                catch (Exception ex)
                {
                    showError(ex.Message);
                }

            });
            #endregion

        }

        //private async void Meter_created_response_TopicUpdatedEvent(string message)
        //{
        //    var res = JsonConvert.DeserializeObject<kafka.kafka_dto.MeterCreatedDTO>(message);
        //    logger.dbg("<" + message);

        //    if (res.error != 0)
        //    {
        //        showError(res.message);
        //    }

        //    if (res.meters_amount - 1 > 0)
        //    {
        //        TotalAmount = res.meters_amount;
        //        await startRegistration();
        //    }
        //    else
        //    {
        //        await serverApi.CloseSession(SessionID);

        //        await Dispatcher.UIThread.InvokeAsync(() => {
        //            Close();    
        //        });

                
        //    }
        //}

        #region helpers     
        int? deffect_component_id = null;
        int? deffect_type_id = null;
        string? comment = null;
        async Task markMeterAssembled(bool isOk)
        {
            List<(int, string)> components = new();
            for (int i = 0; i < componentsList.Count - 1; i++)
            {
                var c = componentsList[i];
                components.Add((c.Id, c.SerialNumber));
            }

            string SerialNumber = componentsList[componentsList.Count - 1].SerialNumber;
            kafka.kafka_dto.MeterDTO meterDTO = null;

            if (!isOk)
            {
                var defselect = new trashComponentsSelectVM(meterComponents);
                defselect.DefectSelectedEvent += async (componentid, defid, cmnt) =>
                {
                    deffect_component_id = componentid;
                    deffect_type_id = defid;
                    comment = cmnt;

                    meterDTO = new kafka.kafka_dto.MeterDTO(SessionID,
                                                                    1,
                                                                    Modification.modificationCode,
                                                                    isOk,
                                                                    regStartTime,
                                                                    DateTime.UtcNow,
                                                                    SerialNumber,
                                                                    components,
                                                                    deffect_component_id,
                                                                    deffect_type_id,
                                                                    comment);

                    try
                    {
                        await prodApi.SetMeterStagePassed(SessionID, meterDTO);
                    } catch (Exception ex)
                    {
                        showError(ex.Message);
                    }

                    await startRegistration();
                };
                var dlg = new dialogVM(defselect);
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    ws.ShowDialog(dlg);
                });
            }
            else
            {
                meterDTO = new kafka.kafka_dto.MeterDTO(SessionID,
                                                                1,
                                                                Modification.modificationCode,
                                                                isOk,
                                                                regStartTime,
                                                                DateTime.UtcNow,
                                                                SerialNumber,
                                                                components,
                                                                deffect_component_id,
                                                                deffect_type_id,
                                                                comment);

                //await meter_created.produceAsync("meter_created", meterDTO.ToString());
                await prodApi.SetMeterStagePassed(SessionID, meterDTO);

                await startRegistration();
            }
        }
        #endregion

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {

            switch (text)
            {
                case "255012255":
                    completeOrderCmd.Execute();                    
                    text = "";
                    return;
                case "255012256":
                    trashOrderCmd.Execute();
                    text = "";
                    return;
                case "255012257":
                    cancelOrderCmd.Execute();                    
                    text = "";
                    return;

            }


            var found = componentsList.FirstOrDefault(c => !string.IsNullOrEmpty(c.SerialNumber) && c.SerialNumber.Equals(text));
            if (found == null)
            {
                var scanned = componentsList[counter];
                scanned.SerialNumber = text;                

                OrderComponentsList.Add(componentsList[counter]);

                counter++;
                counter %= componentsList.Count + 1;

                if (counter == componentsList.Count)
                {
                    Content = new userMessageVM()
                    {
                        Message = "Завершите регистрацию прибора учета"
                    };
                    AllowButtons = true;
                }
                else
                    Content = componentsList[counter];
            }
            else
                showError($"Компонент с серийным номером {text} уже пристуствует в списке");

            text = "";

        }

        async Task startRegistration()
        {
            await Task.Run(async () =>
            {

                //TotalAmount = await prodApi.GetMetersAmount(Order.order_num, 1);

                var order = prodApi.GetOrder(Order.order_num);
                //var nomenclature = order.nomenclature.FirstOrDefault(n => n.decimalNumber.Equals(Modification.decimalNumber));
                //TotalAmount = nomenclature.amount;

                TotalAmount = await prodApi.GetMetersAmount(Order.order_num, Modification.modificationCode);

                if (TotalAmount > 0)
                {
                    //var order = prodApi.GetOrder(Order.order_num);


                    meterComponents = await prodApi.GetComponents(order.model);
                    componentsList = new();
                    foreach (var dto in meterComponents)
                        componentsList.Add(new componentItemVM(dto) { ActionName = "Отсканируйте штрих код компонента:", Id = dto.id });
                    componentsList.Add(new componentItemVM("Прибор учета") { ActionName = "Отсканируйте штрих код изделия:" });
                    //TotalAmount = await serverApi.GetMetersAmount(order.order_num, 1);
                    Content = componentsList[0];
                    counter = 0;
                    CurrentAmount++;
                    OrderComponentsList.Clear();
                    regStartTime = DateTime.UtcNow;
                } else
                {
                    await prodApi.CloseSession(SessionID);

                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        Close();
                    });
                }

            });
        }

        public override async void OnStarted()
        {

            base.OnStarted();
            regStartTime = DateTime.UtcNow;

            try
            {
                SessionID = await prodApi.OpenSession(Order.order_num, AppContext.User.Login, null);
                //meter_created_response.start("meter_created_response");

                //TotalAmount = await prodApi.GetMetersAmount(Order.order_num, 1);

                TotalAmount = await prodApi.GetMetersAmount(Order.order_num, Modification.modificationCode);

                await startRegistration();               

            } catch (Exception ex)
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
                //showError(ex.Message);
            }
        }

        public void OnScan(string text)
        {
            if (!timer.Enabled)
                timer.Start();

            this.text += text;
        }

        
    }
}
