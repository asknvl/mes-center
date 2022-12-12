using mes_center.Models.kafka.kafka_dto;
using mes_center.Models.logger;
using mes_center.Models.rest.server_dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static mes_center.Models.rest.server_dto.OrderDTO;

namespace mes_center.Models.rest
{
    public class ServerApi : IServerApi
    {

        #region vars
        string url;
        ILogger logger = Logger.getInstance();
        #endregion

        public ServerApi(string url)
        {
            this.url = url;
        }

        #region public
        public async Task<List<ConfigurationDTO>> GetConfigurations()
        {
            logger.inf(Tags.SAPI, "GetConfigurations request");

            List<ConfigurationDTO> res = new();
            await Task.Run(() => {
                var client = new RestClient($"{url}/configurations");
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    string msg = $"GetConfigurations FAIL (stasus code={response.StatusCode} response={response.Content})";
                    logger?.err(Tags.SAPI, msg);
                    throw new ServerApiException(msg);                    
                }                
                res = JsonConvert.DeserializeObject<List<ConfigurationDTO>>(response.Content);
                logger.inf(Tags.SAPI, "GetConfigurations OK");

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
            logger.inf(Tags.SAPI, "GetModels request");
            List<ModelDTO> res = new();
            await Task.Run(() => {
                var client = new RestClient($"{url}/models");
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    string msg = $"GetModels request fail (stasus code={response.StatusCode} response={response.Content})";
                    logger?.err(Tags.SAPI, msg);
                    throw new ServerApiException(msg);                    
                }                
                res = JsonConvert.DeserializeObject<List<ModelDTO>>(response.Content);
                logger.inf(Tags.SAPI, "GetModels OK");
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

            logger.inf(Tags.SAPI, $"GetOrders request, statuses = {statusParam}");

            await Task.Run(() =>
            {
                var client = new RestClient($"{url}/orders");
                var request = new RestRequest(Method.GET);
                request.AddQueryParameter("status", statusParam);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    string msg = $"GetOrders request fail (stasus code={response.StatusCode} response={response.Content})";
                    logger?.err(Tags.SAPI, msg);
                    throw new ServerApiException(msg);                    
                }                
                res = JsonConvert.DeserializeObject<List<OrderDTO>>(response.Content);
                logger.inf(Tags.SAPI, "GetOrders OK");
            });

            return res;
        }

        public OrderDTO GetOrder(string order_num)
        {
            logger.inf(Tags.SAPI, $"GetOrder request");
            OrderDTO res = new();

            //await Task.Run(() =>
            //{
                var client = new RestClient($"{url}/orders/{order_num}");
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new ServerApiException($"GetOrders request fail (stasus code={response.StatusCode} response={response.Content})");
                res = JsonConvert.DeserializeObject<OrderDTO>(response.Content);
                if (res == null)
                {
                    string msg = $"GetOrders returned null";
                    logger?.err(Tags.SAPI, msg);
                    throw new ServerApiException(msg);
                }
                logger.inf(Tags.SAPI, "GetOrder OK");
            //});
            return res;
        }

        class jupdate
        {
            public int amount_aux { get; set; }
            public int status { get; set; }
        }
        public async Task<OrderDTO> OrderUpdate(string order_num, int amount_aux, OrderStatus status)
        {
            logger.inf(Tags.SAPI, $"OrderUpdate request, order_num={order_num} amount_aux={amount_aux} status={status.ToString()}");

            OrderDTO res = new();
            var client = new RestClient($"{url}/orders/{order_num}");
            var request = new RestRequest(Method.PATCH);

            jupdate param = new jupdate()
            {
                amount_aux = amount_aux,
                status = (int)status
            };
            var sparam = JsonConvert.SerializeObject(param);

            request.AddParameter("application/json", sparam, ParameterType.RequestBody);

            await Task.Run(() => { 
                IRestResponse response = client.Execute(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {                    
                    string msg = $"OrderUpdate request fail (stasus code={response.StatusCode} response={response.Content})";
                    logger?.err(Tags.SAPI, msg);
                    throw new ServerApiException(msg);
                }
                res = JsonConvert.DeserializeObject<OrderDTO>(response.Content);            
            });

            if (res == null)
                throw new ServerApiException($"OrderUpdate returned null");

            logger.inf(Tags.SAPI, "OrderUpdate OK");
            return res;
        }

        class jcommentupdate
        {
            public string comment { get; set; }
        }
        public async Task<OrderDTO> OrderUpdate(string order_num, string comment)
        {
            logger.inf(Tags.SAPI, $"OrderUpdate request, order_num={order_num} comment={comment}");

            OrderDTO res = new();
            var client = new RestClient($"{url}/orders/{order_num}");
            var request = new RestRequest(Method.PATCH);

            jcommentupdate param = new jcommentupdate()
            {
                comment = comment
            };
            var sparam = JsonConvert.SerializeObject(param);

            request.AddParameter("application/json", sparam, ParameterType.RequestBody);

            await Task.Run(() => {
                IRestResponse response = client.Execute(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    string msg = $"OrderUpdate request fail (stasus code={response.StatusCode} response={response.Content})";
                    logger?.err(Tags.SAPI, msg);
                    throw new ServerApiException(msg);
                }
                res = JsonConvert.DeserializeObject<OrderDTO>(response.Content);
            });

            if (res == null)
                throw new ServerApiException($"OrderUpdate returned null");

            logger.inf(Tags.SAPI, "OrderUpdate OK");
            return res;
        }

        class jorderstatus
        {
            public int status { get; set; }
            public string comment { get; set; }
        }            
        public async Task SetOrderStatus(string order_num, OrderStatus status, string comment)
        {
            logger.inf(Tags.SAPI, $"SetOrderStatus request, order_num={order_num} status={status.ToString()} comment={comment}");

            var client = new RestClient($"{url}/orders/{order_num}");
            var request = new RestRequest(Method.PATCH);

            jorderstatus orderstatus = new();
            orderstatus.status = (int)status;
            orderstatus.comment = Encoding.UTF8.GetString(Encoding.Default.GetBytes(comment));

            string sparam = JsonConvert.SerializeObject(orderstatus);

            request.AddParameter("application/json", sparam, ParameterType.RequestBody);

            await Task.Run(() => { 
              
                IRestResponse response = client.Execute(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {   
                    string msg = $"SetOrderStatus request fail (stasus code={response.StatusCode} response={response.Content})";
                    logger?.err(Tags.SAPI, msg);
                    throw new ServerApiException(msg);
                }
            });

            logger.inf(Tags.SAPI, "SetOrderStatus OK");
        }

        class serialparam
        {           
            public int amount_aux { get; set; }
            public string comment { get; set; }
        }
       
        class sessionparam
        {
            public string order_num { get; set; }
            public string operatorlogin { get; set; }
            public int? equipmentid { get; set; }
        }

        public async Task<List<ComponentDTO>> GetComponents(ModelDTO model)
        {
            logger.inf(Tags.SAPI, $"GetComponents request, model.name={model.name} model.id={model.id}");

            List<ComponentDTO> res = new();
            var client = new RestClient($"{url}/models/{model.id}/components");
            var request = new RestRequest(Method.GET);
            await Task.Run(() =>
            {
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    res = JsonConvert.DeserializeObject<List<ComponentDTO>>(response.Content);
                }
                else
                {
                    string msg = $"GetComponents {model.name} request fail (stasus code={response.StatusCode} response={response.Content})";
                    logger?.err(Tags.SAPI, msg);
                    throw new ServerApiException(msg);                    
                }
            });
            logger.inf(Tags.SAPI, "GetComponents OK");
            return res;
        }

        public async Task<List<MeterComponentDTO>> GetComponents(string sn)
        {
            logger.inf(Tags.SAPI, $"GetMeterComponents request, sn={sn}");

            List<MeterComponentDTO> res = new();
            var client = new RestClient($"{url}/meter/{sn}/components");
            var request = new RestRequest(Method.GET);
            await Task.Run(() =>
            {
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    res = JsonConvert.DeserializeObject<List<MeterComponentDTO>>(response.Content);
                }
                else
                {
                    string msg = $"GetMeterComponents sn={sn} request fail (stasus code={response.StatusCode} response={response.Content})";
                    logger?.err(Tags.SAPI, msg);
                    throw new ServerApiException(msg);
                }
            });
            logger.inf(Tags.SAPI, "GetMeterComponents OK");
            return res;
        }


        class meteraddcomponentinfo
        {
            public int componentid { get; set; }
            public string sn { get; set; }
        }

        public async Task AddComponent(int session_id, string meter_sn, int stage, int componentid, string component_sn)
        {
            logger.inf(Tags.SAPI, $"AddComponent request, session={session_id} sn={meter_sn} stage={stage} componentid={componentid} component_sn={component_sn}");
            var client = new RestClient($"{url}/meter/{session_id}/{meter_sn}/{stage}/component");
            var request = new RestRequest(Method.POST);

            meteraddcomponentinfo inf = new meteraddcomponentinfo()
            {
                componentid = componentid,
                sn = component_sn
            };

            string sparam = JsonConvert.SerializeObject(inf);
            request.AddParameter("application/json", sparam, ParameterType.RequestBody);

            await Task.Run(() => {
                IRestResponse response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                } else
                {
                    string msg = $"AddComponent request fail (stasus code={response.StatusCode} response={response.Content})";
                    logger?.err(Tags.SAPI, msg);
                    throw new ServerApiException(msg);
                }

            });

            logger.inf(Tags.SAPI, $"AddComponent OK");
        }

        public async Task<List<MeterEventDTO>> GetMeterEvents(string sn)
        {
            logger.inf(Tags.SAPI, $"GetMeterEvents request, sn={sn}");

            List<MeterEventDTO> res = new();
            var client = new RestClient($"{url}/meter/{sn}/events");
            var request = new RestRequest(Method.GET);
            await Task.Run(() =>
            {
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    res = JsonConvert.DeserializeObject<List<MeterEventDTO>>(response.Content);
                }
                else
                {
                    string msg = $"GetMeterEvents sn={sn} request fail (stasus code={response.StatusCode} response={response.Content})";
                    logger?.err(Tags.SAPI, msg);
                    throw new ServerApiException(msg);
                }
            });
            logger.inf(Tags.SAPI, "GetMeterEvents OK");
            return res;
        }

        class meterdeletecomponentinfo
        {
            public bool status { get; set; }
            public int defect_typeid { get; set; }
            public string comment { get; set; }
        }
        public async Task DeleteComponent(int session_id, string uuid, int defect_typeid, string comment)
        {
            logger.inf(Tags.SAPI, $"UpdateComponent request, session={session_id} uuid={uuid} defect_typeid={defect_typeid} comment={comment}");
            var client = new RestClient($"{url}/meterComponent/{session_id}/{uuid}");
            var request = new RestRequest(Method.PATCH);

            meterdeletecomponentinfo inf = new meterdeletecomponentinfo()
            {
                status = false,
                defect_typeid = defect_typeid,
                comment = comment
            };

            string sparam = JsonConvert.SerializeObject(inf);
            request.AddParameter("application/json", sparam, ParameterType.RequestBody);

            await Task.Run(() => {
                IRestResponse response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                }
                else
                {
                    string msg = $"DeleteComponent request fail (stasus code={response.StatusCode} response={response.Content})";
                    logger?.err(Tags.SAPI, msg);
                    throw new ServerApiException(msg);
                }

            });

            logger.inf(Tags.SAPI, $"DeleteComponent OK");
        }

        
        
        public async Task<int> OpenSession(string order_num, string login, int? equipmentid)
        {
            logger.inf(Tags.SAPI, $"OpenSession request, order_num={order_num} login={login} equipment={equipmentid}");
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

                }
                else
                {
                    string msg = $"OpenSession request fail (stasus code={response.StatusCode} response={response.Content})";
                    logger?.err(Tags.SAPI, msg);
                    throw new ServerApiException(msg);                    
                }

            });
            logger.inf(Tags.SAPI, $"OpenSession OK, id={id}");
            return id;
        }

        public async Task CloseSession(int id)
        {
            logger.inf(Tags.SAPI, $"CloseSession request, id={id}");

            var client = new RestClient($"{url}/sessions/{id}");
            var request = new RestRequest(Method.PATCH);

            await Task.Run(() =>
            {
                IRestResponse response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                }
                else
                {
                    string msg = $"CloseSession {id} request fail (stasus code={response.StatusCode} response={response.Content})";
                    logger?.err(Tags.SAPI, msg);
                    throw new ServerApiException(msg);                    
                }

            });
            logger.inf(Tags.SAPI, $"CloseSession OK, id={id}");
        }

        public async Task<int> GetMetersAmount(string order_num, int stage)
        {
            logger.inf(Tags.SAPI, $"GetMetersAmount request, order_num={order_num} stage={stage}");
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
                {
                    string msg = $"GetMetersAmount {order_num} request fail (stasus code={response.StatusCode} response={response.Content})";
                    logger?.err(Tags.SAPI, msg);
                    throw new ServerApiException(msg);                    
                }
            });
            logger.inf(Tags.SAPI, $"GetMetersAmount OK, amount={res}");
            return res;            
        }

        public async Task<List<StageDTO>> GetStages()
        {
            logger.inf(Tags.SAPI, $"GetStages request");
            List<StageDTO> res = new();
            var client = new RestClient($"{url}/productionStage");
            var request = new RestRequest(Method.GET);
            await Task.Run(() =>
            {
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    //JObject json = JObject.Parse(response.Content);
                    //res = json.ToObject<List<StrategyDTO>>();
                    res = JsonConvert.DeserializeObject<List<StageDTO>>(response.Content);
                }
                else
                {                   
                    string msg = $"GetStages request fail (stasus code={response.StatusCode} response={response.Content})";
                    logger?.err(Tags.SAPI, msg);
                    throw new ServerApiException(msg);
                }
            });

            string stages = "";
            foreach (var stage in res)
            {
                stages += $"{stage.name} ";
            }
            logger.inf(Tags.SAPI, $"GetStages OK, stages={stages}");
            return res;
        }

        public async Task SetMeterStagePassed(int sessionid, MeterDTO meter)
        {

            logger.inf(Tags.SAPI, $"SetMeterStagePassed request, sessionid={sessionid} meter.sn={meter.sn} meter.stage={meter.stagecode}");

            var client = new RestClient($"{url}/meter/{sessionid}/{meter.sn}/{meter.stagecode}/pass");
            var request = new RestRequest(Method.PATCH);

            string sparam = JsonConvert.SerializeObject(meter);
            request.AddParameter("application/json", sparam, ParameterType.RequestBody);

            await Task.Run(() =>
            {
                IRestResponse response = client.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                        logger.inf(Tags.SAPI, $"SetMeterStagePassed OK");
                        break;

                    default:
                        string msg = $"SetMeterStagePassed request fail (stasus code={response.StatusCode} response={response.Content})";
                        logger?.err(Tags.SAPI, msg);
                        throw new ServerApiException(msg);                       
                }
    
            });

        }

        class repairfinish
        {
            public string start_dt { get; set; }
            public string finish_dt { get; set; }
            public int? next_stagecode { get; set; }
            public string comment { get; set; }
        }

        public async Task SetMeterStagePassed(int sessionid, string sn, DateTime start_dt, int next_stage, string comment) 
        {

            logger.inf(Tags.SAPI, $"SetMeterStagePassed request, sessionid={sessionid} sn={sn} next_stage={next_stage} comment={comment}");

            var client = new RestClient($"{url}/meter/{sessionid}/{sn}/{255}/pass");
            var request = new RestRequest(Method.PATCH);

            repairfinish param = new repairfinish()
            {
                start_dt = start_dt.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
                finish_dt = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
                next_stagecode = next_stage,
                comment = comment
            };

            string sparam = JsonConvert.SerializeObject(param);
            request.AddParameter("application/json", sparam, ParameterType.RequestBody);

            await Task.Run(() =>
            {
                IRestResponse response = client.Execute(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NoContent:
                        logger.inf(Tags.SAPI, $"SetMeterStagePassed OK");
                        break;

                    default:
                        string msg = $"SetMeterStagePassed request fail (stasus code={response.StatusCode} response={response.Content})";
                        logger?.err(Tags.SAPI, msg);
                        throw new ServerApiException(msg);
                }

            });

        }

        public async Task<List<StrategyDTO>> GetStrategies()
        {
            logger.inf(Tags.SAPI, $"GetStrategies request");

            List<StrategyDTO> res = new();
            var client = new RestClient($"{url}/productionStrategy");
            var request = new RestRequest(Method.GET);
            await Task.Run(() =>
            {
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    //JObject json = JObject.Parse(response.Content);
                    //res = json.ToObject<List<StrategyDTO>>();
                    res = JsonConvert.DeserializeObject<List<StrategyDTO>>(response.Content);
                }
                else
                {
                    string msg = $"GetStrategies request fail (stasus code={response.StatusCode} response={response.Content})";
                    logger?.err(Tags.SAPI, msg);
                    throw new ServerApiException(msg);                    
                }
            });

            logger.inf(Tags.SAPI, $"GetStrategies OK");
            return res;
        }

        public async Task CreateStrategy(StrategyDTO strategy)
        {
            logger.inf(Tags.SAPI, $"CreateStrategy request");

            int id = 0;
            var client = new RestClient($"{url}/productionStrategy");
            var request = new RestRequest(Method.POST);            
            string sparam = JsonConvert.SerializeObject(strategy);            
            request.AddParameter("application/json", sparam, ParameterType.RequestBody);

            await Task.Run(() => {
                IRestResponse response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                }
                else
                {
                    string msg = $"CreateStrategy request fail (stasus code={response.StatusCode} response={response.Content})";
                    logger?.err(Tags.SAPI, msg);
                    throw new ServerApiException(msg);                    
                }
            });

            logger.inf(Tags.SAPI, $"CreateStrategy OK");
        }

        public async Task DeleteStrategy(int id)
        {
            logger.inf(Tags.SAPI, $"DeleteStrategy request");

            var client = new RestClient($"{url}/productionStrategy/{id}");
            var request = new RestRequest(Method.DELETE);                        

            await Task.Run(() => {
                IRestResponse response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                }
                else
                {
                    string msg = $"DeleteStrategy request fail (stasus code={response.StatusCode} response={response.Content})";
                    logger?.err(Tags.SAPI, msg);
                    throw new ServerApiException(msg);                    
                }
            });

            logger.inf(Tags.SAPI, $"DeleteStrategy OK");
        }

        public async Task<MeterInfoDTO> GetMeterInfo(string sn, int stage)
        {
            logger.inf(Tags.SAPI, $"GetMeterInfo request, sn={sn} stage={stage}");
            MeterInfoDTO res = null;
            var client = new RestClient($"{url}/meter/{sn}/{stage}/info");
            var request = new RestRequest(Method.GET);
            await Task.Run(() =>
            {
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    res = JsonConvert.DeserializeObject<MeterInfoDTO>(response.Content);
                }
                else
                {
                    string msg = $"GetMeterInfo sn={sn} request fail (stasus code={response.StatusCode} response={response.Content})";
                    logger?.err(Tags.SAPI, msg);
                    throw new ServerApiException(msg);
                }
            });
            logger.inf(Tags.SAPI, $"GetMeterInfo OK, amount={res}");
            return res;
        }
        #endregion
    }
}
