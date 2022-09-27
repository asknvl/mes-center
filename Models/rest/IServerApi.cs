using mes_center.Models.rest.server_dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static mes_center.Models.rest.server_dto.Order;

namespace mes_center.Models.rest
{
    public interface IServerApi
    {
        Task<List<Model>> GetModels();
        Task<List<Configuration>> GetConfigurations();
        Task<List<Order>> GetOrders(Order.OrderStatus[] statuses);
        Order GetOrder(string order_num);
        Task<string> GenerateSerialNumber(string order_num, int amount_aux, string comment);
        Task SetOrderStatus(string order_num, OrderStatus status, string comment);
        Task<List<ComponentDTO>> GetComponents(Model model);
    }

    public class ServerApiException : Exception
    {
        public ServerApiException(string msg) : base(msg) { }
    }
}
