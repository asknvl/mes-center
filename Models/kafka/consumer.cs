using Confluent.Kafka;
using mes_center.Models.logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mes_center.Models.kafka
{
    public class consumer
    {
        #region vars        
        ConsumerConfig config;
        CancellationTokenSource cts;
        IConsumer<Ignore, string> consumerBuilder;
        ILogger logger = Logger.getInstance();
        #endregion
        public consumer(string groupid)
        {

            string url = "172.16.118.105";

            config = new ConsumerConfig
            {
                GroupId = groupid,
                BootstrapServers = url,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build();
        }

        public void start(string topic)
        {
            
            consumerBuilder.Subscribe(topic);            

            cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        var cr = consumerBuilder.Consume(cts.Token);

                        TopicUpdatedEvent?.Invoke($"consumer: Message={cr.Value} at: {cr.Topic} {cr.TopicPartitionOffset}");

                        logger.dbg($"consumer: Message={cr.Value} at: {cr.Topic} {cr.TopicPartitionOffset}");
                    }
                }
                catch (Exception ex)
                {
                    logger.dbg($"consumer: {ex.Message}");
                }
                finally
                {
                    consumerBuilder.Close();
                }

            });
        }

        public event Action<string> TopicUpdatedEvent;

        public void stop()
        {

        }
    }
}
