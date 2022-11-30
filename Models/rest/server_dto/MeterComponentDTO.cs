using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.rest.server_dto
{
    public class MeterComponentDTO
    {
        [JsonProperty]
        public string uuid { get; set; }
        [JsonProperty]
        public string name { get; set; }
        [JsonProperty]
        public string sn { get; set; }
        [JsonProperty]
        public bool status { get; set; }
             
    }
}
