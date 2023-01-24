using mes_center.ViewModels;
using System;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mes_center.Models.rest.server_dto;
using mes_center.Models.scanner;
using System.Diagnostics;
using mes_center.ViewModels.dialogs;

namespace mes_center.arm_regmeter.ViewModels
{
    public class regmeterVM : LifeCycleViewModelBase, IScanner
    {
        #region vars
        loginVM login;        
        #endregion

        #region properties        
        object? content;
        public object? Content
        {
            get => content;
            set {

                var c = content as LifeCycleViewModelBase;
                if (c != null)
                    c.OnStopped();

                var lcc = value as LifeCycleViewModelBase;                
                if (lcc != null)                
                    lcc.OnStarted();               

                this.RaiseAndSetIfChanged(ref content, value);
            }
        }      
        #endregion
        public regmeterVM()
        {
            login = new loginVM();
            login.LoginSucceededEvent += () => {
                showOrderSelection();
            };

            Content = login;

            //Content = new meterRegistrationVM();

        }
        #region helpers
        void showOrderSelection()
        {
            var os = new orderSelectionVM();
            os.CloseRequestEvent += () => {
                Content = login;
            };

            os.OrderSelectedEvent += (_order) => {

                try
                {
                    var order = prodApi.GetOrder(_order.order_num);

                    var sm = new selectModificationDlgVM(order);
                    sm.ModificationSelectedEvent += (modification) => {
                        showMeterRegistration(order, modification);                        
                    };

                    ws.ShowDialog(sm);

                } catch (Exception ex)
                {
                    showError(ex.Message);
                }

            };
            Content = os;
        }

        async void showMeterRegistration(OrderDTO order, ModificationDTO modification)
        {
            //var total_amount = await prodApi.GetMetersAmount(order.order_num, 1);

            var nomenclature = order.nomenclature.FirstOrDefault(n => n.decimalNumber.Equals(modification.decimalNumber));
            var total_amount = nomenclature.amount;

            if (total_amount > 0)
            {
                var mr = new meterRegistrationVM();
                mr.CloseRequestEvent += () =>
                {
                    showOrderSelection();
                };
                mr.Order = order;
                mr.Modification = modification;

                Content = mr;
            }
            else
                showError("Данное задание уже выполнено");
        }
        #endregion

        #region override
        public void OnScan(string text)
        {
            var scanner = Content as IScanner;
            if (scanner != null)
                scanner.OnScan(text);
        }

        public override void OnStopped()
        {
            base.OnStopped();
            var c = Content as LifeCycleViewModelBase;
            c?.OnStopped();
        }
        #endregion
    }
}
