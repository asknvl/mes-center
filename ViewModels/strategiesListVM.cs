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
        #region properties        
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> addCmd { get; }
        public ReactiveCommand<Unit, Unit> removeCmd { get; }
        #endregion

        public strategiesListVM() {

            #region commands
            addCmd = ReactiveCommand.CreateFromTask(async () => {             
            });

            removeCmd = ReactiveCommand.CreateFromTask(async () => {             
            });
            #endregion

        }

        public Task Reload()
        {
            throw new NotImplementedException();
        }
    }
}
