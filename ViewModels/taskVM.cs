﻿using mes_center.Models.kafka;
using mes_center.Models.logger;
using mes_center.Models.rest;
using mes_center.Models.rest.server_dto;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using kafka_dto = mes_center.Models.kafka.kafka_dto;

namespace mes_center.ViewModels
{

    public class ModelItem
    {
        public uint id { get; set; }
        public string name { get; set; }

    }
    public class ConfigurationItem
    {
        public uint id { get; set; }
        public string name { get; set; }
    }

    public class taskVM : ViewModelBase, IReloadable
    {
        #region vars                
        producer kafkaProducer = new producer("172.16.118.105");
        consumer kafkaConsumer = new consumer("test");
        #endregion

        #region properties
        int version;
        public int Version
        {
            get => version;
            set => this.RaiseAndSetIfChanged(ref version, value);   
        }

        string orderNumber;
        public string OrderNumber
        {
            get => orderNumber;
            set => this.RaiseAndSetIfChanged(ref orderNumber, value);
        }

        int zoneCode;
        public int ZoneCode
        {
            get => zoneCode;
            set => this.RaiseAndSetIfChanged(ref zoneCode, value);
        }

        List<Model> models = new();
        public List<Model> Models
        {
            get => models;
            set
            {
                this.RaiseAndSetIfChanged(ref models, value);            
                if (models.Count > 0)
                    Model = models[0];
            }
        }

        Model model;
        public Model Model
        {
            get => model;
            set => this.RaiseAndSetIfChanged(ref model, value);
        }

        List<Configuration> configurations = new();
        public List<Configuration> Configurations
        {
            get => configurations;
            set {
                this.RaiseAndSetIfChanged(ref configurations, value);
                Configuration = configurations[0];
            }
        }

        Configuration configuration;
        public Configuration Configuration
        {
            get => configuration;
            set => this.RaiseAndSetIfChanged(ref configuration, value);
        }

        int amount = 10;
        public int Amount
        {
            get => amount;
            set => this.RaiseAndSetIfChanged(ref amount, value);
        }

        string fwv;
        public string Fwv
        {
            get => fwv;
            set => this.RaiseAndSetIfChanged(ref fwv, value);   
        }

        string comment;
        public string Comment
        {
            get => comment;
            set => this.RaiseAndSetIfChanged(ref comment, value);
        }

        bool isDataReady;
        public bool IsDataReady
        {
            get => isDataReady;
            set => this.RaiseAndSetIfChanged(ref isDataReady, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> createTaskCmd { get; set; }
        #endregion

        public taskVM()
        {
            #region commands
            createTaskCmd = ReactiveCommand.CreateFromTask(async () => {
                logger.dbg("createTaskCmd executed");

                kafka_dto.Order order = new kafka_dto.Order();
                order.version = Version;
                order.order_num = OrderNumber;
                order.pz_code = ZoneCode;
                order.meter_modelid = Model.id;
                order.configurationid = Configuration.id;
                order.amount = Amount;
                order.fwv = Fwv;
                order.comment = Comment;

                try
                {
                    await kafkaProducer.produceAsync("order_new", order.ToString());
                    
                } catch (Exception ex)
                {
                    logger.dbg(ex.Message);
                }
                
            });

            #endregion
        }

        #region public
        public async Task Reload()
        {

            //kafkaConsumer.addTopic("order_new");
            //kafkaConsumer.addTopic("order_event");
            kafkaConsumer.start("order_new");

            Version = 1;
            OrderNumber = "0";
            ZoneCode = 11;
            Fwv = "0.0.0";
            Comment = "Test task";

            try
            {
                await Task.Run(async () => {
                    IsDataReady = false;
                    Models = await serverApi.GetModels();
                    Configurations = await serverApi.GetConfigurations();
                    IsDataReady =true;
                });
            } catch (Exception ex)
            {
                logger.dbg(ex.Message);
                showError(ex.Message);
            }
        }
        #endregion
    }

}
