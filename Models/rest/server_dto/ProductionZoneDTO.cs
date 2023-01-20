using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.rest.server_dto
{
    public class ProductionZoneDTO
    {
        [JsonProperty]
        public int code { get; set; }
        [JsonProperty]
        public string name { get; set; }
        [JsonProperty]
        public string post_address { get; set; }
        [JsonProperty]
        public string legal_address { get; set; }
        [JsonProperty]
        public string comment { get; set; }
    }
}
