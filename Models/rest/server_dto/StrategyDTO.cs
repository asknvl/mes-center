using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.rest.server_dto
{
    public class StrategyDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<StageDTO> stages { get; set; }
    }
}
