using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.kafka.kafka_dto
{
    public class Order : BaseDTO
    {
        [JsonProperty]
        public int version { get; set; }
        [JsonProperty]
        public string? order_num { get; set; }
        [JsonProperty]
        public int pz_code { get; set; }
        [JsonProperty]
        public int meter_modelid { get; set; }
        [JsonProperty]
        public int configurationid { get; set; }
        [JsonProperty]
        public int amount { get; set; }
        [JsonProperty]
        public string? fwv { get; set; } = "0.0.0";
        [JsonProperty]
        public string? comment { get; set; }

    }
}
