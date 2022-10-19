using Avalonia.Threading;
using mes_center.Models.rest.server_dto;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.ViewModels
{
    public class addstrategyVM : LifeCycleViewModelBase, IReloadable
    {
        #region properties        
        string name;
        public string Name
        {
            get => name;
            set
            {
               if (string.IsNullOrEmpty(value))
                    AddError(nameof(Name), "Укажите название стратегии");
                else
                    RemoveError(nameof(Name));

                updateValidity(value);

                this.RaiseAndSetIfChanged(ref name, value);
            }
        }

        bool isInputValid;
        public bool IsInputValid
        {
            get => isInputValid;
            set => this.RaiseAndSetIfChanged(ref isInputValid, value);
        }

        public ObservableCollection<StageDTO> DestinationStages { get; } = new();
        public ObservableCollection<StageDTO> SourceStages { get; } = new();

        StageDTO destinationStage;
        public StageDTO DestinationStage
        {
            get => destinationStage;
            set => this.RaiseAndSetIfChanged(ref destinationStage, value);
        }

        StageDTO sourceStage;
        public StageDTO SourceStage
        {
            get => sourceStage;
            set => this.RaiseAndSetIfChanged(ref sourceStage, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> addCmd { get; }
        public ReactiveCommand<Unit, Unit> removeCmd { get; }
        public ReactiveCommand<Unit,Unit> okCmd { get; }
        public ReactiveCommand<Unit, Unit> cancelCmd { get; }
        #endregion
        public addstrategyVM()
        {

            #region commands
            addCmd = ReactiveCommand.Create(() => {
                if (SourceStage != null && !excludedStage(SourceStage))
                {                    
                    DestinationStages.Insert(DestinationStages.Count - 1, SourceStage);
                    SourceStages.Remove(SourceStage);
                }
            });
            removeCmd = ReactiveCommand.Create(() => {
                if (DestinationStage != null && !excludedStage(DestinationStage))
                {                    
                    SourceStages.Add(DestinationStage);
                    DestinationStages.Remove(DestinationStage);
                }
            });
            okCmd = ReactiveCommand.CreateFromTask(async () => {

                StrategyDTO strategy = new StrategyDTO()
                {
                    name = Name,
                    stages = DestinationStages.ToList()
                };

                try
                {
                    await serverApi.CreateStrategy(strategy);
                } catch (Exception ex)
                {
                    showError(ex.Message);
                }

                StrategyCreatedEvent?.Invoke();
                Close();
            });

            cancelCmd = ReactiveCommand.Create(() => {
                Close();
            });
            #endregion
        }

        #region helpers
        bool excludedStage(StageDTO stage)
        {
            switch ((StageDTO.Codes)stage.code)
            {
                case StageDTO.Codes.assemby:
                case StageDTO.Codes.packing:
                    return true;                    
            }
            return false;
        }

        void updateValidity(string name)
        {
            IsInputValid = !string.IsNullOrEmpty(name);
        }
        #endregion

        #region public
        public async Task Reload()
        {
            await Dispatcher.UIThread.InvokeAsync(() => {
                SourceStages.Clear();
            });

            var stages = await serverApi.GetStages();            

            foreach (var stage in stages)
            {
                if (!SourceStages.Contains(stage) && !excludedStage(stage))
                    SourceStages.Add(stage);
            }

            var st_0 = stages.FirstOrDefault(s => s.code == (int)StageDTO.Codes.assemby); //сборка
            if (st_0 != null)
                DestinationStages.Add(st_0);

            var st_1 = stages.FirstOrDefault(s => s.code == (int)StageDTO.Codes.packing); //упаковка
            if (st_1 != null)
                DestinationStages.Add(st_1);
        }

        public override async void OnStarted()
        {
            base.OnStarted();
            await Reload();
        }
        #endregion

        #region callbacks
        public event Action StrategyCreatedEvent;        
        #endregion
    }
}
