using mes_center.Models.rest.server_dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static mes_center.Models.rest.server_dto.OrderDTO;

namespace mes_center.Models.rest
{
    public class ServerApi : IServerApi
    {


        #region vars
        string url;
        #endregion

        public ServerApi(string url)
        {
            this.url = url;
        }

        #region public
        public async Task<List<ConfigurationDTO>> GetConfigurations()
        {
            List<ConfigurationDTO> res = new();
            await Task.Run(() => {
                var client = new RestClient($"{url}/configurations");
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new ServerApiException($"GetConfigurations request fail (stasus code={response.StatusCode})");
                res = JsonConvert.DeserializeObject<List<ConfigurationDTO>>(response.Content);

            });
            return res;
        }

        public async Task<ConfigurationDTO> GetConfiguration(int id)
        {
            ConfigurationDTO res = new();
            await Task.Run(() => { 
            });
            return res;
        }

        public async Task<List<ModelDTO>> GetModels()
        {
            List<ModelDTO> res = new();
            await Task.Run(() => {
                var client = new RestClient($"{url}/models");
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new ServerApiException($"GetModels request fail (stasus code={response.StatusCode})");
                res = JsonConvert.DeserializeObject<List<ModelDTO>>(response.Content);
            });
            return res;
        }


        public async Task<List<OrderDTO>> GetOrders(OrderDTO.OrderStatus[] statuses)
        {
            List<OrderDTO> res = new();
            string statusParam = "";

            foreach (var status in statuses) {
                statusParam += $"{(int)status},";
            }
            statusParam = statusParam.Remove(statusParam.Length - 1);

            await Task.Run(() =>
            {
                var client = new RestClient($"{url}/orders");
                var request = new RestRequest(Method.GET);
                request.AddQueryParameter("status", statusParam);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new ServerApiException($"GetOrders request fail (stasus code={response.StatusCode})");

                res = JsonConvert.DeserializeObject<List<OrderDTO>>(response.Content);
            });

            return res;
        }

        public OrderDTO GetOrder(string order_num)
        {
            OrderDTO res = new();

            var client = new RestClient($"{url}/orders/{order_num}");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new ServerApiException($"GetOrders request fail (stasus code={response.StatusCode})");            
            res = JsonConvert.DeserializeObject<OrderDTO>(response.Content);
            if (res == null)
                throw new ServerApiException($"GetOrders returns null");
            return res;
        }


        class jorderstatus
        {
            public int status { get; set; }
            public string comment { get; set; }
        }            
        public async Task SetOrderStatus(string order_num, OrderStatus status, string comment)
        {            
            var client = new RestClient($"{url}/orders/{order_num}/status");
            var request = new RestRequest(Method.PATCH);

            jorderstatus orderstatus = new();
            orderstatus.status = (int)status;
            orderstatus.comment = Encoding.UTF8.GetString(Encoding.Default.GetBytes(comment));

            string sparam = JsonConvert.SerializeObject(orderstatus);

            request.AddParameter("application/json", sparam, ParameterType.RequestBody);

            await Task.Run(() => { 
              
                IRestResponse response = client.Execute(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new ServerApiException($"SetOrderStatus request fail (stasus code={response.StatusCode})");
            });

        }

        class serialparam
        {           
            public int amount_aux { get; set; }
            public string comment { get; set; }
        }
        public async Task<string> GenerateSerialNumber(string order_num, int amount_aux, string comment)
        {
            string res = "";

            var client = new RestClient($"{url}/orders/{order_num}/first_serial");
            var request = new RestRequest(Method.PATCH);

            serialparam param = new serialparam()
            {                
                amount_aux = amount_aux,
                comment = Encoding.UTF8.GetString(Encoding.Default.GetBytes(comment))
            };

            string sparam = JsonConvert.SerializeObject(param);

            request.AddParameter("application/json", sparam, ParameterType.RequestBody);

            await Task.Run(async () => {
                IRestResponse response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JObject json = JObject.Parse(response.Content);
                    string snum = json["serial_num"].ToObject<string>();

                    await SetOrderStatus(order_num, OrderStatus.READY_TO_EXECUTE, "");


                } else
                    throw new ServerApiException($"SetOrderStatus request fail (stasus code={response.StatusCode})");
            });



            return res;
        }

        class sessionparam
        {
            public string order_num { get; set; }
            public string operatorlogin { get; set; }
            public int equipmentid { get; set; }
        }

        public async Task<List<ComponentDTO>> GetComponents(ModelDTO model)
        {
            List<ComponentDTO> res = new();
            var client = new RestClient($"{url}/models/{model.id}/components");
            var request = new RestRequest(Method.GET);
            await Task.Run(() =>
            {
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    res = JsonConvert.DeserializeObject<List<ComponentDTO>>(response.Content);
                } else
                    throw new ServerApiException($"GetComponents {model.name} request fail (stasus code={response.StatusCode})");
            });
            return res;
        }

        public async Task<int> OpenSession(string order_num, string login, int equipmentid)
        {
            int id = 0;
            var client = new RestClient($"{url}/session");
            var request = new RestRequest(Method.POST);
            sessionparam param = new sessionparam()
            {
                order_num = order_num,
                operatorlogin = login,
                equipmentid = equipmentid
            };
            string sparam = JsonConvert.SerializeObject(param);

            request.AddParameter("application/json", sparam, ParameterType.RequestBody);

            await Task.Run(() => {
                IRestResponse response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JObject json = JObject.Parse(response.Content);
                    id = json["id"].ToObject<int>();

                } else
                    throw new ServerApiException($"OpenSession request fail (stasus code={response.StatusCode})");
            });
            return id;
        }

        public async Task CloseSession(int id)
        {
            var client = new RestClient($"{url}/session/{id}");
            var request = new RestRequest(Method.PATCH);

            await Task.Run(() =>
            {
                IRestResponse response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                }
                else
                    throw new ServerApiException($"CloseSession {id} request fail (stasus code={response.StatusCode})");
            });
        }

        public async Task<int> GetMetersAmount(string order_num, int stage)
        {
            int res = 0;

            var client = new RestClient($"{url}/orders/{order_num}/meters_for_stage/{stage}");
            var request = new RestRequest(Method.GET);
            await Task.Run(() =>
            {
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    JObject json = JObject.Parse(response.Content);
                    res = json["metersAmount"].ToObject<int>();
                }
                else
                    throw new ServerApiException($"GetMetersAmount {order_num} request fail (stasus code={response.StatusCode})");
            });
            return res;

            return res;
        }

        public Task<List<StageDTO>> GetStages()
        {
            throw new NotImplementedException();
        }

        public Task<List<StrategyDTO>> GetStrategies()
        {
            throw new NotImplementedException();
        }

        public Task CreateStrategy(string name, List<StageDTO> stages)
        {
            throw new NotImplementedException();
        }

        public Task DeleteStrategy(int id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
