using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.rest.server_dto
{
    public class StageDTO
    {
        #region const
        public enum Codes : int
        {
            assembly = 1,
            packing = 8,
            repair = 255
        }
        #endregion

        public int code { get; set; }
        public string name { get; set; }
    }
}
