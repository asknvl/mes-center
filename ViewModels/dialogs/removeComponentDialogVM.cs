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
    public class removeComponentDialogVM : LifeCycleViewModelBase
    {
        #region properties
        List<DefectDTO> defects;
        public List<DefectDTO> Defects
        {
            get => defects;
            set => this.RaiseAndSetIfChanged(ref defects, value);
        }

        DefectDTO defect;
        public DefectDTO Defect
        {
            get => defect;
            set => this.RaiseAndSetIfChanged(ref defect, value);
        }

        string comment;
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
        public removeComponentDialogVM(List<DefectDTO> defects)
        {
            Defects = defects;
            if (Defects.Count > 0)
                Defect = Defects[0];

            #region commands
            okCmd = ReactiveCommand.Create(() => {
                ComponentUpdateEvent?.Invoke(Defect, Comment);
                Close();
            });

            closeCmd = ReactiveCommand.Create(() => {
                Close();
            });
            #endregion
        }

        #region events
        public event Action<DefectDTO, string> ComponentUpdateEvent;
        #endregion
    }
}
