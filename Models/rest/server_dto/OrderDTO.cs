using mes_center.Models.rest.server_dto;
using mes_center.ViewModels;
using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.rest.server_dto
{
    public class OrderDTO : ViewModelBase
    {
        #region const
        public enum OrderStatus : int
        {
            CREATED = 0,
            READY_TO_SEND,
            SENT,
            RECEIVED,
            READY_TO_EXECUTE,            
            COMPLETED,
            REJECTED
        }

        public Dictionary<int, string> OrderStatuses = new Dictionary<int, string>
        {
            {(int)OrderStatus.CREATED, "Создано, ожидает уточнения параметров"},
            {(int)OrderStatus.READY_TO_SEND, "Создано, готово к передаче АССППУ Производство"},
            {(int)OrderStatus.SENT, "Отправлено АССППУ Производство"},
            {(int)OrderStatus.RECEIVED, "Получено АССППУ Производство"},            
            {(int)OrderStatus.READY_TO_EXECUTE, "Готово к выполнению"},            
            {(int)OrderStatus.COMPLETED, "Выполнение задания завершено"},
            {(int)OrderStatus.REJECTED, "Отклонено АССППУ Производство"}
        };
        #endregion

        //[JsonProperty]
        //public int version { get; set; }
        [JsonProperty]
        public string? order_num { get; set; }
        [JsonProperty]
        public ModelDTO model { get; set; }
        [JsonProperty]
        public ConfigurationDTO config { get; set; }
        [JsonProperty]        
        public string? first_serial { get; set; }

        int _amount;
        [JsonProperty]
        public int amount {
            get => _amount;
            set
            {
                _amount = value;

                switch (model.phases)
                {
                    case 1:
                        aux_amount_complete = (24 - _amount % 24);
                        break;
                    case 3:
                        aux_amount_complete = (16 - _amount % 16);
                        break;
                    default:
                        break;
                }

                sum_amount = amount + amount_aux;
            }
        }
        int _amount_aux;
        [JsonProperty]
        public int amount_aux {
            get => _amount_aux;
            set => this.RaiseAndSetIfChanged(ref _amount_aux, value);
        }
        [JsonProperty]
        public string? fwv { get; set; }
        string _reg_date;
        [JsonProperty]
        public string? reg_date
        {
            get => _reg_date;
            set
            {
                _reg_date = value;
                text_reg_date = DateTime.Parse(_reg_date).ToString("dd.MM.yy HH:mm:ss");
            }
        }

        int _status;
        [JsonProperty]
        public int status {
            get => _status;
            set
            {
                _status = value;
                if (OrderStatuses.ContainsKey(_status))
                    text_status = OrderStatuses[_status];
                else
                    text_status = "Не определен";
            }
        }
        [JsonProperty]
        public string? status_cd { get; set; }
        [JsonProperty]
        public int pz_code { get; set; }

        string _comment = null;
        [JsonProperty]
        public string? comment {
            get => _comment;
            set
            {
                IsSaveCommentVisible = !(_comment == null || _comment == "") && status == (int)OrderStatus.READY_TO_EXECUTE;
                _comment = value;
            }
        }
        [JsonIgnore]
        public string text_status { get; set; }
        [JsonIgnore]
        public string text_reg_date { get; set; }
        [JsonIgnore]
        public int aux_amount_complete { get; set; }

        bool _need_aux_autocomplete;
        [JsonIgnore]
        public bool need_aux_autocomplete {
            get => amount_aux > 0;
            set
            {
                if (value)
                    amount_aux = aux_amount_complete;
                else
                    amount_aux = 0;

                sum_amount = amount + amount_aux;

                this.RaiseAndSetIfChanged(ref _need_aux_autocomplete, value);
            }
        }
        [JsonIgnore]
        int _sum_amount;
        public int sum_amount
        {
            get
            {
                _sum_amount = amount + amount_aux;
                return _sum_amount;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _sum_amount, value);
            }
        }

        bool isSaveCommentVisible = false;
        public bool IsSaveCommentVisible
        {
            get => isSaveCommentVisible;
            set => this.RaiseAndSetIfChanged(ref isSaveCommentVisible, value);
        }
        public OrderDTO()
        {
            order_num = String.Empty;
            fwv = String.Empty;
            comment = String.Empty;
        }

        #region public
        public event Action<string> CommentChangedEvent;
        #endregion

    }

}
