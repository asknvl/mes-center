using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.kafka.kafka_dto
{
    public class OrderEvent : BaseDTO
    {
        [JsonProperty]
        public string version { get; set; }
        [JsonProperty]
        string order_num { get; set; }
        [JsonProperty]
        string date { get; set; }
        [JsonProperty]
        public int event_code { get; set; }
        [JsonProperty]
        public int error { get; set; }
        [JsonProperty]
        public string message { get; set; }

    }
}
