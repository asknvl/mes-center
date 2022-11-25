using Confluent.Kafka;
using mes_center.Models.logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.Models.kafka
{
    internal class producer
    {
        #region vars
        ProducerConfig config;
        IProducer<Null, string> producerBuilder;
        ILogger logger = Logger.getInstance();
        #endregion
        public producer()
        {
            string url = "172.16.118.105";
            config = new ProducerConfig { BootstrapServers = url };
            producerBuilder = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task<bool> produceAsync(string topic, string msg)
        {
            bool res = false;
            try
            {
                var message = new Message<Null, string> { Value = msg };
                var deliv = await producerBuilder.ProduceAsync(topic, message);
                logger.inf(Tags.KAFK, $"producer: Message={message} Topic={topic}");

            } catch (Exception ex)
            {
                logger.err(Tags.KAFK, $"producer: {ex.Message}");                
            }
            return res;
        }

        
    }
}
