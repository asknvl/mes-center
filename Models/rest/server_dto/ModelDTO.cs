using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.rest.server_dto
{
    public class ModelDTO
    {
        [JsonProperty]
        public int id { get; set; }
        [JsonProperty]
        public string? name { get; set; }
        [JsonProperty]
        public int code { get; set; }
        [JsonProperty]
        public int? lastNum { get; set; }
        [JsonProperty]
        public int? phases { get; set; }
    }
}
