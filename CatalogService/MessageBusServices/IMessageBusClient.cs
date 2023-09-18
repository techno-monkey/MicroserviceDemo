using CatalogService.Database;
using CatalogService.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace CatalogService.MessageBusServices
{
    public interface IMessageBusClient
    {
        void Publish(CategoryPublishDto category);
    }

    public class MessageBusClient : IMessageBusClient, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IOptions<AppOptions> options)
        {
            var factory = new ConnectionFactory() { HostName = options.Value?.RabbitMQHost, Port = int.Parse(options.Value?.RabbitMQPort) };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionClose;
                Console.WriteLine("RabbitMq is up");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void RabbitMQ_ConnectionClose(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("RabbitMQ connection is closed.");
        }

        public void Publish(CategoryPublishDto category)
        {
            var message = JsonSerializer.Serialize(category);
            if (_connection.IsOpen)
            {
                Console.WriteLine("Message sending");
                SendMessage(message);
            }
            else
                Console.WriteLine("Message connection is closed");
        }

        public void Dispose()
        {
            if(_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);
            Console.WriteLine($"Message is sent: {message}");
        }
    }
}
