using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.rest.server_dto
{
    public class MeterInfoDTO
    {
        [JsonProperty]
        public string order_num { get; set; }
        [JsonProperty]
        public int status { get; set; }
        [JsonProperty]
        public string data { get; set; }
    }
}
