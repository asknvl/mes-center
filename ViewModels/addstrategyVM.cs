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
    public class addstrategyVM : ViewModelBase, IReloadable
    {
        #region properties
        public ObservableCollection<StageDTO> DestinationStages { get; } = new();
        public ObservableCollection<StageDTO> SourceStages { get; } = new();

        StageDTO destinationStage;
        public StageDTO DestinationStage
        {
            get => DestinationStage;
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
        public ReactiveCommand<Unit, Unit> removeCmd { get; set; }
        public ReactiveCommand<Unit,Unit> okCmd { get; set; }
        public ReactiveCommand<Unit, Unit> cancelCmd { get; set; }
        #endregion
        public addstrategyVM()
        {
            #region commands
            #endregion
        }



        public Task Reload()
        {
            throw new NotImplementedException();
        }
    }
}
