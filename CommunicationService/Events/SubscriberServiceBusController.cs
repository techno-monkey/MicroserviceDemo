
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using CommunicationService.Models.Options;
using Azure.Messaging.ServiceBus;
using RabbitMQ.Client.Events;
using System.Threading.Channels;

namespace CommunicationService.Events
{
    public class SubscriberServiceBusController : BackgroundService, IDisposable
    {
        private readonly AppOption _appOptions;
        private readonly IEventProcessor _eventProcessor;
        private ServiceBusClient _serviceBusClient;
        private ServiceBusProcessor _processor;
        private readonly ILogger _logger;


        public SubscriberServiceBusController(IOptions<AppOption> appOptions, IEventProcessor eventProcessor, ILogger<SubscriberServiceBusController> logger)
        {
            _appOptions = appOptions?.Value ?? throw new ArgumentNullException(nameof(AppOption));
            _serviceBusClient = new ServiceBusClient(_appOptions.ServiceBusConnectionString);
            _eventProcessor = eventProcessor;
             _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            ServiceBusProcessorOptions serviceBusProcessorOptions = new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false,
            };

            _processor = _serviceBusClient.CreateProcessor(_appOptions.ServiceBusQueueName, serviceBusProcessorOptions);
            _processor.ProcessMessageAsync += ProcessMessagesAsync;
            _processor.ProcessErrorAsync += ProcessErrorAsync;

            _processor.StartProcessingAsync().ConfigureAwait(false);
            return Task.CompletedTask;
        }

        private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
        {
            var myPayload = args.Message.Body.ToString();
            _eventProcessor.PorocessEvent(myPayload);
            await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            _logger.LogError(arg.Exception, "Message handler encountered an exception");
            _logger.LogDebug($"- ErrorSource: {arg.ErrorSource}");
            _logger.LogDebug($"- Entity Path: {arg.EntityPath}");
            _logger.LogDebug($"- FullyQualifiedNamespace: {arg.FullyQualifiedNamespace}");

            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            if (_processor != null)
            {
                await _processor.DisposeAsync().ConfigureAwait(false);
            }

            if (_serviceBusClient != null)
            {
                await _serviceBusClient.DisposeAsync().ConfigureAwait(false);
            }
        }

        public async Task CloseSubscriptionAsync()
        {
            await _processor.CloseAsync().ConfigureAwait(false);
        }
    }
}
