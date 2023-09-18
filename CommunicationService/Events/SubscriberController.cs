using CommunicationService.Models.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;

namespace CommunicationService.Events
{
    public class SubscriberController : BackgroundService, IDisposable
    {
        private readonly AppOption _appOption;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;
        private string _queue;

        public SubscriberController(IOptions<AppOption> options, IEventProcessor eventProcessor)
        {
            _appOption = options.Value;
            _eventProcessor = eventProcessor;
            IntializeRabbitMQ();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (handler, e) =>
            {
                Console.WriteLine("Msg Recieved!!");
                var body = e.Body;
                var msg = Encoding.UTF8.GetString(body.ToArray());
                _eventProcessor.PorocessEvent(msg);
            };
            _channel.BasicConsume(_queue, autoAck: true, consumer);
            return Task.CompletedTask;
        }

        private void IntializeRabbitMQ()
        {
            var factory = new ConnectionFactory() { HostName = _appOption.RabbitMQHost, Port = int.Parse(_appOption.RabbitMQPort) };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _queue = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _queue, exchange: "trigger", routingKey: "");
            Console.WriteLine("Listening to Msg Bus");
            _connection.ConnectionShutdown += RabbitMQ_CloseConnection;
        }
        private void RabbitMQ_CloseConnection(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("Shutting Down Service Bus");
        }

        public void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }


}
