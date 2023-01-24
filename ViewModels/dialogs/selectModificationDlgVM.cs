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
    public class selectModificationDlgVM : LifeCycleViewModelBase
    {

        #region properties
        List<NomenclatureDTO> nomenclatures;
        public List<NomenclatureDTO> Nomenclatures
        {
            get => nomenclatures;
            set => this.RaiseAndSetIfChanged(ref nomenclatures, value);
        }

        NomenclatureDTO nomenclature;
        public NomenclatureDTO Nomenclature
        {
            get => nomenclature;
            set => this.RaiseAndSetIfChanged(ref nomenclature, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> okCmd { get; }
        public ReactiveCommand<Unit, Unit> cancelCmd { get; }
        #endregion

        public selectModificationDlgVM(OrderDTO order)
        {
            Nomenclatures = order.nomenclature;

            #region commands
            okCmd = ReactiveCommand.Create(() => {
                if (Nomenclature != null)
                    ModificationSelectedEvent?.Invoke(Nomenclature);
                Close();
            });
            cancelCmd = ReactiveCommand.Create(() => {
                Close();
            });
            #endregion
        }

        #region events
        public event Action<ModificationDTO> ModificationSelectedEvent; 
        #endregion
    }
}
