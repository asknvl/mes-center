using mes_center.Models.rest.server_dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static mes_center.Models.rest.server_dto.OrderDTO;

namespace mes_center.Models.rest
{
    public interface IServerApi
    {
        Task<List<ModelDTO>> GetModels();
        Task<List<ConfigurationDTO>> GetConfigurations();
        Task<List<OrderDTO>> GetOrders(OrderDTO.OrderStatus[] statuses);
        OrderDTO GetOrder(string order_num);
        Task<string> GenerateSerialNumber(string order_num, int amount_aux, string comment);
        Task SetOrderStatus(string order_num, OrderStatus status, string comment);
        Task<List<ComponentDTO>> GetComponents(ModelDTO model);
        Task<int> OpenSession(string order_num, string login, int equipmentid);
        Task<int> GetMetersAmount(string order_num, int stage);
        Task<List<StageDTO>> GetStages();
        Task<List<StrategyDTO>> GetStrategies();
        Task CreateStrategy(string name, List<StageDTO> stages);
        Task DeleteStrategy(int id);
    }

    public class ServerApiException : Exception
    {
        public ServerApiException(string msg) : base(msg) { }
    }
}
