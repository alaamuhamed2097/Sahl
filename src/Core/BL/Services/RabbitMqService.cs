using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BL.Services
{
    public class RabbitMqService
    {
        private readonly string _hostName;
        private readonly string _queueName;

        public RabbitMqService(string hostName = "localhost", string queueName = "testQueue")
        {
            _hostName = hostName;
            _queueName = queueName;
        }

        // Send message asynchronously
        public async Task SendMessageAsync(string message)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: _queueName,
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

            var body = Encoding.UTF8.GetBytes(message);
            await channel.BasicPublishAsync(exchange: "",
                                            routingKey: _queueName,
                                            body: body);

            Console.WriteLine($"[x] Sent: {message}");
        }

        // Receive messages asynchronously
        public async Task ReceiveMessagesAsync(Func<string, Task> onMessageReceived)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: _queueName,
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                await onMessageReceived(message);
            };

            await channel.BasicConsumeAsync(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" [*] Waiting for messages...");
        }
    }
}
