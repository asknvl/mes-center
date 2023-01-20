using Avalonia.Threading;
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

namespace mes_center.arm_repair.ViewModels
{
    public class componentsListVM : ViewModelBase
    {
        #region vars
        string SN, order_num;
        List<MeterComponentDTO> componentDTOs = new();
        #endregion

        #region properties
        public ObservableCollection<componentListItem> Components { get; } = new();

        componentListItem componentListItem;
        public componentListItem Component
        {
            get => componentListItem;
            set => this.RaiseAndSetIfChanged(ref componentListItem, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> addCmd { get; }
        public ReactiveCommand<Unit, Unit> removeCmd { get; }
        #endregion

        public componentsListVM(int session, string sn, string order_num)
        {
            SN = sn;
            this.order_num = order_num;

            #region commands
            addCmd = ReactiveCommand.CreateFromTask(async () => {
                try
                {
                    var order = prodApi.GetOrder(order_num);
                    var avaliable_components = await prodApi.GetComponents(order.model);

                    var dlg = new addComponentDlgVM(componentDTOs, avaliable_components);
                    dlg.ComponentAddedEvent += async (component) => {

                        var found = Components.Any(c => c.id == component.componentInfo.id);
                        found = false;

                        if (!found)
                        {
                            try
                            {
                                await prodApi.AddComponent(session, sn, 255, component.componentInfo.id, component.sn);
                                await Update();
                            } catch (Exception ex)
                            {
                                await Dispatcher.UIThread.InvokeAsync(() =>
                                {
                                    ws.ShowDialog(dlg);
                                });
                                
                            }

                            //var newComponentListItem = new componentListItem()
                            //{
                            //    id = component.componentInfo.id,
                            //    sn = component.sn,
                            //    name = component.componentInfo.name,
                            //    status = component.status
                            //};
                            //Components.Add(newComponentListItem);

                         

                        } else
                        {
                            showError("Такой компонент уже установлен в прибор");
                        }
                    };
                    ws.ShowDialog(dlg);

                } catch (Exception ex)
                {
                    showError(ex.Message);
                }
            });

            removeCmd = ReactiveCommand.CreateFromTask(async () => { 

                try
                {
                    var order = prodApi.GetOrder(order_num);
                    var avaliable_components = await prodApi.GetComponents(order.model);

                    var defectComponentAvailable = avaliable_components.FirstOrDefault(c => c.id == Component.id);

                    var componentToUpdate = componentDTOs.FirstOrDefault(c => c.uuid == Component.uuid);
                    
                    if (defectComponentAvailable != null && componentToUpdate != null)
                    {
                        var dlg = new removeComponentDialogVM(defectComponentAvailable.defects);
                        dlg.ComponentUpdateEvent += async (defect, comment) => {

                            await prodApi.DeleteComponent(session, componentToUpdate.uuid, defect.id, comment);
                            await Update();

                        };
                        ws.ShowDialog(dlg);
                    }
                    else
                    {
                        //TODO
                    }
                    
                } catch (Exception ex)
                {
                    showError(ex.Message);
                } 

            });
            #endregion
        }

        #region public
        public async Task Update()
        {
            componentDTOs = await prodApi.GetComponents(SN);
            Components.Clear();

            foreach (var dto in componentDTOs)
            {
                var component = new componentListItem()
                {
                    id = dto.componentInfo.id,
                    uuid = dto.uuid,
                    name = dto.componentInfo.name,
                    sn = dto.sn,
                    status = dto.status
                };
                Components.Add(component);
            }

            var defectComponent = Components.FirstOrDefault(c => c.status == false);
            Component = (defectComponent != null) ? defectComponent : Components[0];
        }
        #endregion

        #region events
        public event Action UpdatedEvent;
        #endregion
    }
}
