using mes_center.Models.rest.server_dto;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.ViewModels.dialogs
{
    public class repairFinishDlgVM : LifeCycleViewModelBase
    {
        #region properties
        List<StageDTO> stages;
        public List<StageDTO> Stages
        {
            get => stages;
            set => this.RaiseAndSetIfChanged(ref stages, value);
        }

        StageDTO stage;
        public StageDTO Stage
        {
            get => stage;
            set => this.RaiseAndSetIfChanged(ref stage, value);
        }

        string comment = null;
        public string Comment
        {
            get => comment;
            set => this.RaiseAndSetIfChanged(ref comment, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> okCmd { get; }
        public ReactiveCommand<Unit, Unit> closeCmd { get; }
        #endregion

        public repairFinishDlgVM()
        {
            #region commands
            okCmd = ReactiveCommand.Create(() => {
                RepairFinishedEvent?.Invoke(Stage, Comment);
                Close();
            });

            closeCmd = ReactiveCommand.Create(() => {
                Close();
            });
            #endregion
        }

        #region override
        public override async void OnStarted()
        {
            base.OnStarted();

            try
            {
                Stages = await serverApi.GetStages();
                if (Stages.Count != 0)
                    Stage = Stages[0];

            } catch (Exception ex)
            {
                showError(ex.Message);
            }
            
        }
        #endregion

        #region events
        public event Action<StageDTO, string?> RepairFinishedEvent;
        #endregion
    }
}
