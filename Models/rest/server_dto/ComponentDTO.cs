using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.rest.server_dto
{
    public class DefectDTO
    {
        [JsonProperty]
        public int id { get; set; }
        [JsonProperty]
        public string name { get; set; }
    }
    public class ComponentDTO
    {
        [JsonProperty]
        public int id { get; set; }
        [JsonProperty]
        public string name { get; set; }
        [JsonProperty]
        public List<DefectDTO> defects { get; set; }
    }
}
