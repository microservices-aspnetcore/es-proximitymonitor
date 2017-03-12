using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace StatlerWaldorfCorp.ProximityMonitor.Queues
{
    public class RabbitMQEventingConsumer : EventingBasicConsumer
    {
        public RabbitMQEventingConsumer(IConnectionFactory factory) : base(factory.CreateConnection().CreateModel())
        {            
        }
    }
}