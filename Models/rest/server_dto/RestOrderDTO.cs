using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.rest.server_dto
{
    public class RestOrderDTO
    {
        public string order_num { get; set; }
        public int pz_code { get; set; }
        public int meter_modelid { get; set; }
        public int configurationid { get; set; }    
        public string comment { get; set; }
        public string customer_name { get; set; }
        public List<NomenclatureDTO> nomenclature { get; set; } = new();

    }
}
