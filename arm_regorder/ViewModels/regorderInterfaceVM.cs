using mes_center.Models.rest.server_dto;
using mes_center.ViewModels;
using mes_center.ViewModels.dialogs;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.arm_regorder.ViewModels
{
    public class regorderInterfaceVM : LifeCycleViewModelBase
    {
        #region properties
        List<ProductionZoneDTO> productionZones = new();
        public List<ProductionZoneDTO> ProductionZones {
            get => productionZones;
            set => this.RaiseAndSetIfChanged(ref productionZones, value);
        }

        ProductionZoneDTO productionZone;
        public ProductionZoneDTO ProductionZone
        {
            get => productionZone;
            set => this.RaiseAndSetIfChanged(ref productionZone, value);
        }

        string orderNumber;
        public string OrderNumber
        {
            get => orderNumber;
            set => this.RaiseAndSetIfChanged(ref orderNumber, value);
        }

        List<ModelDTO> models = new();
        public List<ModelDTO> Models
        {
            get => models;
            set
            {
                this.RaiseAndSetIfChanged(ref models, value);
                if (models.Count > 0)
                    Model = models[0];
            }
        }


        ModelDTO model = null;
        public ModelDTO Model
        {
            get => model;
            set
            {
                try
                {
                    Task.Run(async () =>
                    {

                        AvailableModifications.Clear();
                        Nomenclatures.Clear();

                        await updateModels();

                        this.RaiseAndSetIfChanged(ref model, value);

                    });

                } catch (Exception ex)
                {
                    showError(ex.Message);
                }                
            }
        }

        List<ConfigurationDTO> configurations;
        public List<ConfigurationDTO> Configurations
        {
            get => configurations;
            set => this.RaiseAndSetIfChanged(ref configurations, value);
        }

        ConfigurationDTO configuration;
        public ConfigurationDTO Configuration
        {
            get => configuration;
            set => this.RaiseAndSetIfChanged(ref configuration, value);
        }

        public ObservableCollection<ModificationDTO> AvailableModifications { get; } = new();

        ModificationDTO modification;
        public ModificationDTO Modification
        {
            get => modification;
            set => this.RaiseAndSetIfChanged(ref modification, value);  
        }
        public ObservableCollection<NomenclatureDTO> Nomenclatures { get; } = new();

        NomenclatureDTO nomenclature;
        public NomenclatureDTO Nomenclature
        {
            get => nomenclature;
            set => this.RaiseAndSetIfChanged(ref nomenclature, value);
        }

        string comment;
        public string Comment
        {
            get => comment;
            set => this.RaiseAndSetIfChanged(ref comment, value);
        }

        int totalAmount;
        public int TotalAmount
        {
            get => totalAmount;
            set => this.RaiseAndSetIfChanged(ref totalAmount, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> addModificationCmd { get; }
        public ReactiveCommand<Unit, Unit> removeModificationCmd { get; }
        public ReactiveCommand<Unit, Unit> registerCmd { get; }
        public ReactiveCommand<Unit, Unit> cancelCmd { get; }
        #endregion

        public regorderInterfaceVM()
        {

            addModificationCmd = ReactiveCommand.Create(() =>
            {

                if (Modification != null)
                {
                    var tmp = Modification;
                    //AvailableModifications.Remove(Modification);

                    if (!Nomenclatures.Any(n => n.id.Equals(tmp.id)))
                    {
                        var newItem = new NomenclatureDTO()
                        {
                            id = tmp.id,
                            decimalNumber = tmp.decimalNumber,
                            amount = 0
                        };

                        newItem.AmountChanged += NewItem_AmountChanged;

                        Nomenclatures.Add(newItem);
                    }

                    TotalAmount = getTotalAmount();
                }

            });

            removeModificationCmd = ReactiveCommand.Create(() => { 

                if (Nomenclature != null)
                {
                    var tmp = Nomenclature;
                    Nomenclatures.Remove(Nomenclature);
                    //AvailableModifications.Add(new ModificationDTO() { 
                    //    id = tmp.id,
                    //    decimalNumber = tmp.decimalNumber,
                    //    modificationCode = tmp.modificationCode,
                    //    weight = tmp.weight
                    //});

                    TotalAmount = getTotalAmount();
                }

            });

            registerCmd = ReactiveCommand.CreateFromTask(async () => { 
            
                try
                {

                    RestOrderDTO order = new RestOrderDTO()
                    {
                        order_num = OrderNumber,
                        pz_code = ProductionZone.code,
                        meter_modelid = Model.id,
                        configurationid = Configuration.id,
                        comment = Comment,
                        customer_name = "???",
                        nomenclature = new List<NomenclatureDTO>(Nomenclatures)
                    };

                    await centrApi.OrderCreate(order);

                    Nomenclatures.Clear();

                    var vm = new msgVM();
                    vm.Title = "Сообщение";
                    vm.Message = "Задание создано";

                    var dlg = new dialogVM(vm);
                    ws.ShowDialog(dlg);


                } catch (Exception ex)
                {
                    showError(ex.Message);
                }
            
            });

            cancelCmd = ReactiveCommand.Create(() => {
                Close();
            });

        }

        private void NewItem_AmountChanged(int delta)
        {
            TotalAmount += delta;
        }

        #region helpers
        async Task updateModels()
        {
            if (Model != null)
            {
                var modifications = await prodApi.GetModifications(Model.id);
                foreach (var item in modifications)
                    AvailableModifications.Add(item);
            }
        }

        int getTotalAmount()
        {
            var amounts = Nomenclatures.Select(n => n.amount);
            return amounts.Sum();
        }
        #endregion

        #region override
        public override async void OnStarted()
        {
            base.OnStarted();

            DateTime dt = DateTime.Now;
            OrderNumber = $"{11}_{dt.Year}." +
                          $"{dt.Month.ToString("00.##")}." +
                          $"{dt.Day.ToString("00.##")}_{dt.Hour.ToString("00.##")}." +
                          $"{dt.Minute.ToString("00.##")}"; //"11_YYYY.MM.DD_HH.MM"

            try
            {
                await Task.Run(async () => {

                    ProductionZones = await centrApi.GetProductionZones();
                    if (ProductionZones.Count > 0)
                        ProductionZone = ProductionZones[0];

                    Models = await centrApi.GetModels();
                    if (Models.Count > 0)
                        Model = Models[0];

                    Configurations = await centrApi.GetConfigurations();
                    if (Configurations.Count > 0)
                        Configuration = Configurations[0];

                    await updateModels();

                });

            } catch (Exception ex)
            {
                showError(ex.Message);
            }
        }
        #endregion
    }
}
