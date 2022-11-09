using mes_center.Models.rest.server_dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.rest.server_dto
{
    public class OrderDTO 
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
        [JsonProperty]
        public int amount { get; set; }
        [JsonProperty]
        public int amount_aux { get; set; }
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
        [JsonProperty]
        public string? comment { get; set; }

        public string text_status { get; set; }

        public string text_reg_date { get; set; }
        public OrderDTO()
        {
            order_num = String.Empty;
            fwv = String.Empty;
            comment = String.Empty;
        }

        #region public
        static int counter = 0;
        public static OrderDTO GetTestOrder()
        {
            OrderDTO order = new OrderDTO()
            {
                //version = 1,
                order_num = $"Test#{counter++}",
                model = new ModelDTO() { id = 1, code = 1 },
                config = new ConfigurationDTO() { id = 1, name = $"Test config", comment = "Test comment", data = "Test data" },
                amount = 123,
                amount_aux = 0,
                fwv = "0.0.0",
                reg_date = DateTime.Now.ToString("o"),
                status = 3,
                status_cd = DateTime.Now.ToString("o"),
                pz_code = 11,
                comment = "Test comment"
            };

            return order;
        }
        #endregion

    }

}
