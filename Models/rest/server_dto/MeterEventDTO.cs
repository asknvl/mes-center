using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.rest.server_dto
{
    public class MeterEventDTO
    {
        public int? stagecode { get; set; }
        public string? finish_dt { get; set; }
        public string? data { get; set; }
        public string? comment { get; set; }
        public bool? is_ok { get; set; }
        public string? equipment_name { get; set; }  
        public string? operator_name { get; set; }
        public string? operator_phone { get; set; }    
    }
}
