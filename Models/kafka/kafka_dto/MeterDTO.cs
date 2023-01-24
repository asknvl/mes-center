using mes_center.Models.rest.server_dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.kafka.kafka_dto
{

    public class SN_DTO
    {
        public int id { get; set; }
        public string sn { get; set; }
    }

    public class MeterDTO : BaseDTO
    {     
        [JsonProperty]
        public bool is_ok { get; set; }
        [JsonProperty]
        public string modificationCode { get; set; }
        [JsonProperty]
        public string start_dt { get; set; }
        [JsonProperty]
        public string finish_dt { get; set; }
        [JsonProperty]
        public string? comment { get; set; }
        [JsonProperty]
        public int? componentid { get; set; }
        [JsonProperty]
        public int? defect_typeid { get; set; }
        [JsonProperty]
        public string? data { get; set; }
        [JsonProperty]
        public int? next_stagecode { get; set; } = null;
        [JsonProperty]
        public List<SN_DTO> components { get; set; } = new();

        [JsonIgnore]
        public int sessionid { get; set; }
        [JsonIgnore]
        public int? stagecode { get; set; }
        [JsonIgnore]
        public string sn { get; set; }

        public MeterDTO() { }
        public MeterDTO(int sessionID,
                        int stageCode,
                        string modificationcode,
                        bool isOk,
                        DateTime beginTime,
                        DateTime endTime,
                        string serialNumber,
                        List<(int, string)> components,
                        int? defect_component_id,
                        int? defect_type_id,
                        string? comment) 
        {
            //version = 1;
            sessionid = sessionID;
            stagecode = stageCode;
            modificationCode = modificationcode;
            start_dt = beginTime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
            finish_dt = endTime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
            is_ok = isOk;
            sn = serialNumber;
            this.components = new();

            foreach (var item in components)
            {
                SN_DTO snd = new();
                snd.id = item.Item1;
                snd.sn = item.Item2;
                this.components.Add(snd);
            }

            componentid = defect_component_id;
            defect_typeid = defect_type_id;

            this.comment = comment;
        }

        public MeterDTO(int sessionID,
                        int stageCode,
                        bool isOk,
                        DateTime beginTime,
                        DateTime endTime,
                        string serialNumber)
        {
            //version = 1;
            sessionid = sessionID;
            stagecode = stageCode;
            start_dt = beginTime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
            finish_dt = endTime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
            is_ok = isOk;
            sn = serialNumber;            
        }
    }

}
