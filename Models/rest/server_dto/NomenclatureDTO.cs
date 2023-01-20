using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.rest.server_dto
{
    public class NomenclatureDTO : ModificationDTO
    {
        int _amount;
        [JsonProperty]
        public int amount
        {
            get => _amount;
            set
            {
                int delta = value - _amount;
                _amount = value;
                AmountChanged?.Invoke(delta);
            }
        }

        public event Action<int> AmountChanged;
    }
}
