using Common.MessageBusServices;
using Microsoft.Azure.ServiceBus;
using System.Text;

namespace Common.Services
{
    public class AzureBusService : IAzureBusService
    {
        private readonly IQueueClient _queueClient;

        public AzureBusService(string connStr, string name)
        {
            _queueClient = new QueueClient(connStr, name);
        }

        public async Task SendMessageAsync(string message)
        {
            var encodedMessage = new Message(Encoding.UTF8.GetBytes(message));
            await _queueClient.SendAsync(encodedMessage);
        }

        public async Task ReceiveMessageAsync()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            _queueClient.RegisterMessageHandler(ProcessMessageAsync, messageHandlerOptions);
            await Task.CompletedTask; // Let the method run continuously
        }

        private async Task ProcessMessageAsync(Message message, CancellationToken token)
        {
            var messageBody = Encoding.UTF8.GetString(message.Body);
            Console.WriteLine($"Received message: {messageBody}");

            // You can process the message here as per your application's needs.

            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception: {exceptionReceivedEventArgs.Exception}");
            return Task.CompletedTask;
        }
    }
}
