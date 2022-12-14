using mes_center.Models.kafka.kafka_dto;
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
        Task<OrderDTO> OrderUpdate(string order_num, int amount_aux, OrderStatus status);
        Task<OrderDTO> OrderUpdate(string order_num, string comment);
        Task SetOrderStatus(string order_num, OrderStatus status, string comment);
        Task<List<ComponentDTO>> GetComponents(ModelDTO model);
        Task<List<MeterComponentDTO>> GetComponents(string sn);
        Task AddComponent(int session_id, string meter_sn, int stage, int componentid, string component_sn);
        Task DeleteComponent(int session_id, string uuid, int defect_typeid, string comment);
        Task<List<MeterEventDTO>> GetMeterEvents(string sn);
        Task<int> OpenSession(string order_num, string login, int? equipmentid);
        Task CloseSession(int id);
        Task SetMeterStagePassed(int sessionid, MeterDTO meter);
        Task SetMeterStagePassed(int sessionid, string sn, DateTime start_dt, int next_stage, string comment);
        Task<int> GetMetersAmount(string order_num, int stage);
        Task<List<StageDTO>> GetStages();
        Task<List<StrategyDTO>> GetStrategies();
        Task CreateStrategy(StrategyDTO strategy);
        Task DeleteStrategy(int id);
        Task<MeterInfoDTO> GetMeterInfo(string sn, int stage);
        Task DisposeMeter(int session_id, string sn);
    }

    public class ServerApiException : Exception
    {
        public ServerApiException(string msg) : base(msg) { }
    }
}
