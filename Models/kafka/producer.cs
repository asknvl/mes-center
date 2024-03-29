﻿using Confluent.Kafka;
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
        public producer(string url)
        {
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
            } catch (Exception ex)
            {
                logger.dbg($"producer: {ex.Message}");                
            }
            return res;
        }

        
    }
}
