using mes_center.Models.rest.server_dto;
using mes_center.ViewModels.dialogs;
using mes_center.WS;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.ViewModels
{
    public class strategiesListVM : ViewModelBase, IReloadable
    {
        #region vars
        WindowService ws = WindowService.getInstance();
        #endregion

        #region properties     
        List<StrategyDTO> strategies;
        public List<StrategyDTO> Strategies
        {
            get => strategies;
            set => this.RaiseAndSetIfChanged(ref strategies, value);
        }

        StrategyDTO strategy;
        public StrategyDTO Strategy
        {
            get => strategy;
            set => this.RaiseAndSetIfChanged(ref strategy, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> addCmd { get; }
        public ReactiveCommand<Unit, Unit> removeCmd { get; }
        #endregion

        public strategiesListVM() {

            #region commands
            addCmd = ReactiveCommand.Create(() => {

                var vm = new addstrategyVM();
                vm.StrategyCreatedEvent += async () => {
                    await Reload();
                };
                var dlg = new dialogVM(vm);

                ws.ShowDialog(dlg);

                //var dlg = new addStrategyDlgVM();
                //ws.ShowDialog(dlg);
            });

            removeCmd = ReactiveCommand.CreateFromTask(async () => {

                try
                {
                    await serverApi.DeleteStrategy(Strategy.id);
                    await Reload();
                } catch (Exception ex)
                {
                    showError(ex.Message);
                }

            });
            #endregion

        }

        public async Task Reload()
        {
            var strategies = await serverApi.GetStrategies();
            Strategies = strategies;
        }
    }
}
