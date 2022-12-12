using mes_center.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.arm_repair.ViewModels
{
    public class eventListItem : ViewModelBase
    {
        string? _date;
        public string? date
        {
            get => DateTime.Parse(_date).ToString("dd.MM.yy HH:mm:ss");
            set => this.RaiseAndSetIfChanged(ref _date, value);
        }

        string? _stage;
        public string? stage
        {
            get => _stage;
            set => this.RaiseAndSetIfChanged(ref _stage, value);
        }

        string? _data;
        public string? data
        {
            get => _data;
            set => this.RaiseAndSetIfChanged(ref _data, value);
        }

        string? _comment;
        public string? comment
        {
            get => _comment;
            set => this.RaiseAndSetIfChanged(ref _comment, value);
        }

        bool? _isOk;
        public bool? isOk
        {
            get => _isOk;
            set => this.RaiseAndSetIfChanged(ref _isOk, value);
        }

        string? _equipmentName;
        public string? equipmentName
        {
            get => _equipmentName;
            set => this.RaiseAndSetIfChanged(ref _equipmentName, value);
        }

        string? _operatorName;
        public string? operatorName
        {
            get => _operatorName;
            set => this.RaiseAndSetIfChanged(ref _operatorName, value);
        }

        string? _operatorPhone;
        public string? operatorPhone
        {
            get => _operatorPhone;
            set => this.RaiseAndSetIfChanged(ref _operatorPhone, value);
        }
    }
}
