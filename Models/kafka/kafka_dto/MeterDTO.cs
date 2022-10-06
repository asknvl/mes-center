using mes_center.Models.rest.server_dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.kafka.kafka_dto
{

    public class SN_DTO
    {
        public int componentid { get; set; }
        public string sn { get; set; }
    }

    public class MeterDTO : BaseDTO
    {
        public int version { get; set; }
        public int sessionid { get; set; }
        public string production_stageid { get; set; }
        public string start_dt { get; set; }
        public string finish_dt { get; set; }
        public string comment { get; set; }
        public int componentid { get; set; }
        public int defect_typeid { get; set; }
        public int is_ok { get; set; }
        public string sn { get; set; }
        public List<SN_DTO> components { get; set; }

        public MeterDTO() { }
        public MeterDTO(int sessionID,
                        string stageid, 
                        bool isOk,
                        DateTime beginTime,
                        DateTime endTime,
                        string serialNumber,
                        List<(int, string)> components) 
        {
            version = 1;
            sessionid = sessionID;
            production_stageid = stageid;
            start_dt = beginTime.ToString("o");
            finish_dt = endTime.ToString("o");
            is_ok = (isOk) ? 1 : 0;
            sn = serialNumber;
            this.components = new();

            foreach (var item in components)
            {
                SN_DTO snd = new();
                snd.componentid = item.Item1;
                snd.sn = item.Item2;
                this.components.Add(snd);
            }
        }
    }

}
