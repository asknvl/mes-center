using mes_center.Models.rest.server_dto;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.ViewModels
{    
    public class trashComponentsSelectVM : LifeCycleViewModelBase
    {
        #region properties
        List<ComponentDTO> components = new();
        public List<ComponentDTO> Components
        {
            get => components;
            set => this.RaiseAndSetIfChanged(ref components, value);
        }

        ComponentDTO selectedComponent;
        public ComponentDTO SelectedComponent
        {
            get => selectedComponent;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedComponent, value);
                IsInputValid = true;
                Defects = selectedComponent.defects;
            }
        }

        List<DefectDTO> defects = new();
        public List<DefectDTO> Defects
        {
            get => defects;
            set => this.RaiseAndSetIfChanged(ref defects, value);
        }

        DefectDTO selectetDefect;
        public DefectDTO SelectedDefect
        {
            get => selectetDefect;
            set => this.RaiseAndSetIfChanged(ref selectetDefect, value);
        }

        string? comment;
        public string? Comment
        {
            get => comment;
            set => this.RaiseAndSetIfChanged(ref comment, value);
        }

        bool isInputValid;
        public bool IsInputValid
        {
            get => isInputValid;
            set => this.RaiseAndSetIfChanged(ref isInputValid, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> okCmd { get; }
        public ReactiveCommand<Unit, Unit> cancelCmd { get; }
        #endregion

        public trashComponentsSelectVM(List<ComponentDTO> components)
        {
            Components = components;            

            #region commands
            okCmd = ReactiveCommand.CreateFromTask(async () => {
                DefectSelectedEvent?.Invoke(SelectedComponent.id, SelectedDefect.id, Comment);
                Close();
            });
            cancelCmd = ReactiveCommand.Create(() => {
                Close();
            });
            #endregion
        }

        #region callbacks
        public event Action<int?, int?, string?> DefectSelectedEvent;
        #endregion
    }
}
