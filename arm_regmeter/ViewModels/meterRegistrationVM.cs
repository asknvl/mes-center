using kafka = mes_center.Models.kafka;
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
        List<ComponentDTO> components;        
        List<componentItemVM> componentsList;
        int SessionID = 0;

        kafka.producer meter_created;
        kafka.consumer meter_created_response;

        DateTime regStartTime;
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

            meter_created = new kafka.producer();
            kafka.consumer meter_created_response;

            #region commands
            completeOrderCmd = ReactiveCommand.CreateFromTask(async () => {

                List<(int, string)> components = new();
                for (int i = 0; i < componentsList.Count - 1; i++) {
                    var c = componentsList[i];
                    components.Add((c.Id, c.SerialNumber));
                }
                string SerialNumber = componentsList[componentsList.Count - 1].SerialNumber;
                kafka.kafka_dto.MeterDTO meterDTO = new kafka.kafka_dto.MeterDTO(SessionID,"1", true, regStartTime, DateTime.Now, SerialNumber, components);

                await meter_created.produceAsync("meter_created", meterDTO.ToString());

                if (CurrentAmount == TotalAmount)
                    Close();

                AllowButtons = false;

                await startRegistration();

            });
            trashOrderCmd = ReactiveCommand.CreateFromTask(async () => {

                if (CurrentAmount == TotalAmount)
                    Close();

                AllowButtons = false;
                await startRegistration();

            });
            cancelOrderCmd = ReactiveCommand.CreateFromTask(async () => {
                Close();

                //kafka.kafka_dto.MeterDTO meterDTO = new kafka.kafka_dto.MeterDTO(SessionID, "1", true, regStartTime, DateTime.Now, "123", new List<(string, string)>());
                //await meter_created.produceAsync("meter_created", meterDTO.ToString());


            });
            #endregion

        }

        #region helpers        
        #endregion

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {

            switch (text)
            {
                case "255012255":
                    logger.dbg("register");
                    text = "";
                    return;
                case "255012256":
                    logger.dbg("trash");
                    text = "";
                    return;
                case "255012257":
                    logger.dbg("cancel");
                    text = "";
                    return;

            }

            var scanned = componentsList[counter];
            scanned.SerialNumber = text;            
            text = "";

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
            } else
                Content = componentsList[counter];

            Debug.WriteLine(counter);

        }

        async Task startRegistration()
        {
            await Task.Run(async () =>
            {
                var order = serverApi.GetOrder(Order.order_num);
                components = await serverApi.GetComponents(order.model);

                componentsList = new();
                foreach (var dto in components)
                    componentsList.Add(new componentItemVM(dto) { ActionName = "Отсканируйте штрих код компонента:", Id = dto.id});
                componentsList.Add(new componentItemVM("Прибор учета") { ActionName = "Отсканируйте штрих код изделия:" });

                Content = componentsList[0];
                counter = 0;
                TotalAmount = await serverApi.GetMetersAmount(order.order_num, 1);
                CurrentAmount++;
                OrderComponentsList.Clear();
                regStartTime = DateTime.Now;
            });
        }

        public override async void OnStarted()
        {
            base.OnStarted();

            regStartTime = DateTime.Now;

            try
            {
                SessionID = await serverApi.OpenSession(Order.order_num, AppContext.User.Login, 0);

                meter_created_response = new kafka.consumer(this.GetType().Name);
                

                //kafka.consumer meter_created_check = new kafka.consumer("testmeter");
                //meter_created_check.start("meter_created");
                //meter_created_check.TopicUpdatedEvent += (msg) => {                    
                //    logger.dbg(msg);                                //};


                meter_created_response.TopicUpdatedEvent += (message) => {

                    var res = JsonConvert.DeserializeObject<kafka.kafka_dto.MeterCreatedDTO>(message);
                    Debug.WriteLine(message);

                };
                meter_created_response.start("meter_created_response");

                await startRegistration();                

            } catch (Exception ex)
            {

            }
        }

        public void OnScan(string text)
        {
            timer.Start();
            this.text += text;
        }

    }
}
