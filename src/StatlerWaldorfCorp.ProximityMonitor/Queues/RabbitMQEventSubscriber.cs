using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StatlerWaldorfCorp.ProximityMonitor.Events;

namespace StatlerWaldorfCorp.ProximityMonitor.Queues
{
    public class RabbitMQEventSubscriber : IEventSubscriber
    {
        public event ProximityDetectedEventReceivedDelegate ProximityDetectedEventReceived;

        private IConnectionFactory connectionFactory;
        private QueueOptions queueOptions;
        private EventingBasicConsumer consumer;   
        private IModel channel;   
        private string consumerTag;  
        private ILogger logger;

        public RabbitMQEventSubscriber(ILogger<RabbitMQEventSubscriber> logger,
            ConnectionFactory connectionFactory,
            IOptions<QueueOptions> queueOptions,
            EventingBasicConsumer consumer)
        {
            this.connectionFactory = connectionFactory;
            this.queueOptions = queueOptions.Value;
            this.consumer = consumer;
            this.channel = consumer.Model;
            this.logger = logger;

            logger.LogInformation("Created RabbitMQ event subscriber.");
            Initialize();
        }

        private void Initialize()
        {
             channel.QueueDeclare(
                queue: queueOptions.ProximityDetectedEventQueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );    
            consumer.Received += (ch, ea) => {
                var body = ea.Body;
                var msg = Encoding.UTF8.GetString(body);
                var evt = JsonConvert.DeserializeObject<ProximityDetectedEvent>(msg);
                logger.LogInformation($"Received incoming event, {body.Length} bytes.");
                if (ProximityDetectedEventReceived != null) {
                    ProximityDetectedEventReceived(evt);
                }
                channel.BasicAck(ea.DeliveryTag, false);
            };
        }

        public void Subscribe()
        {
            consumerTag = channel.BasicConsume(queueOptions.ProximityDetectedEventQueueName, false, consumer);
            logger.LogInformation($"Subscribed to queue {queueOptions.ProximityDetectedEventQueueName}, ctag = {consumerTag}");
        }

        public void Unsubscribe()
        {
            channel.BasicCancel(consumerTag);
            logger.LogInformation($"Stopped subscription on queue {queueOptions.ProximityDetectedEventQueueName}");
        }
    }
}