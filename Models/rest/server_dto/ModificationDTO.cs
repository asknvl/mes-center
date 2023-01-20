using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.rest.server_dto
{
    public class ModificationDTO
    {
        [JsonProperty]
        public int id { get; set; }
        [JsonProperty]
        public string decimalNumber { get; set; }
        [JsonProperty]
        public string modificationCode { get; set; }
        [JsonProperty]
        public int weight { get; set; }
    }
}
