using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.kafka.kafka_dto
{
    public class MeterCreatedDTO : BaseDTO
    {
        public int sessionid { get; set; }
        public int meters_amount { get; set; }
        public int error { get; set; }
        public string message { get; set; }
    }
}
