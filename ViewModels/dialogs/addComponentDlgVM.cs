using mes_center.Models.rest.server_dto;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.ViewModels.dialogs
{
    public class addComponentDlgVM : ScannerViewModelBase
    {
        #region vars
        List<MeterComponentDTO> existing;
        List<ComponentDTO> available;
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> closeCmd { get; }
        #endregion

        #region properties
        public ObservableCollection<ComponentDTO> Components { get; }

        ComponentDTO selected;
        public ComponentDTO Component
        {
            get => selected;
            set => this.RaiseAndSetIfChanged(ref selected, value);

        }
        #endregion

        public addComponentDlgVM(List<MeterComponentDTO> existing, List<ComponentDTO> available)
        {
            this.existing = existing;
            this.available = available;

            //var avids = available.Select(a => a.id).ToList();
            //var selected = available.Where(a => !avids.Contains(a.id));

            Components = new ObservableCollection<ComponentDTO>(available);
            Component = Components[0];

            #region commands
            closeCmd = ReactiveCommand.Create(() => {
                Close();
            });
            #endregion

        }

        protected override void OnData(string data)
        {
            try
            {
                //var found = existing.FirstOrDefault(e => e.sn.Equals(data));
                MeterComponentDTO found = null;
                if (found == null)
                {
                    var newComponent = new MeterComponentDTO()
                    {
                        sn = data,
                        status = true,
                        componentInfo = new ComponentInfoDTO()
                        {
                            id = Component.id,
                            name = Component.name
                        }
                    };

                    ComponentAddedEvent?.Invoke(newComponent);

                } else
                    showError("Компонент с таким серийным номером уже установлен");                
            }
            catch (Exception ex)
            {
                showError(ex.Message);                
            }
            finally
            {
                Close();
            }
        }

        #region callbacks
        public event Action<MeterComponentDTO> ComponentAddedEvent;
        #endregion
    }
}
