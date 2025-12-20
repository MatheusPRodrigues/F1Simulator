using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace F1Simulator.RaceControlService.Messaging
{
    public class RabbitMQPublish : IPublishService
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;

        public RabbitMQPublish()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        }

        public async Task Publish(object data, string routingKey)
        {

            await _channel.QueueDeclareAsync(
                queue: routingKey,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var json = JsonSerializer.Serialize(data);
            var body = Encoding.UTF8.GetBytes(json);

            await _channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: routingKey,
                body: body
            );
        }
    }
}
