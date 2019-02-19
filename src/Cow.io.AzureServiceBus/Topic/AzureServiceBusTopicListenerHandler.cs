using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cow.io.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Cow.io.AzureServiceBus
{
    internal class AzureServiceBusTopicListenerHandler : IDisposable
    {
        private readonly ILogger<IMessage> _logger;
        private readonly IServiceProvider _provider;
        private readonly IList<IAzureTopicListener> _services;

        public AzureServiceBusTopicListenerHandler(IServiceProvider provider)
        {
            _provider = provider;
            _services = new List<IAzureTopicListener>();
            _logger = provider.GetService<ILogger<IMessage>>();
        }

        public void AddListener(IAzureTopicListener subscriber)
        {
            subscriber.Client.RegisterMessageHandler(DispatchPackage, new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            });
            _services.Add(subscriber);
        }

        private async Task DispatchPackage(Microsoft.Azure.ServiceBus.Message message, CancellationToken cancellation)
        {
            try
            {
                var header = JsonConvert.DeserializeObject<Header>(message.Label);
                _logger.LogInformation($"[{message.MessageId}] Message received");
                _logger.LogInformation($"[{message.MessageId}] Wity type: {message.Label}");

                var subscribers = _provider.GetServices(typeof(ISubscribe<>).MakeGenericType(header.MessageType));
                var listener = _services.FirstOrDefault(lst => typeof(IAzureTopicListener<>).MakeGenericType(header.MessageType).IsAssignableFrom(lst.GetType()));
                var serializer = _provider.GetService(typeof(IServiceBusSerializer<>).MakeGenericType(header.MessageType));
                var body = serializer == null ? JsonConvert.DeserializeObject(Encoding.UTF8.GetString(message.Body), header.MessageType) : ((dynamic)serializer).Deserialize(Encoding.UTF8.GetString(message.Body));
                _logger.LogInformation($"[{message.MessageId}] With body: {Encoding.UTF8.GetString(message.Body)}");

                foreach (var subscriber in subscribers)
                {
                    try
                    {
                        _logger.LogInformation($"[{message.MessageId}] Will be sent to: {subscriber}");
                        await ((dynamic)subscriber).Handle((dynamic)body);
                        await listener.Client.CompleteAsync(message.SystemProperties.LockToken);
                        _logger.LogInformation($"[{message.MessageId}] Was completed with token: {message.SystemProperties.LockToken}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"[{message.MessageId}] An exception has occurred:{ex.Message}");
                        await listener.Client.AbandonAsync(message.SystemProperties.LockToken);
                        _logger.LogInformation($"[{message.MessageId}] Was abondoned with token:{message.SystemProperties.LockToken}");
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"[{message.MessageId}] An exception has occurred:{ex.Message}");
                throw;
            }
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            StringBuilder builder = new StringBuilder($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            builder.AppendLine("Exception context for troubleshooting:");
            builder.AppendLine($"- Endpoint: {context.Endpoint}");
            builder.AppendLine($"- Entity Path: {context.EntityPath}");
            builder.AppendLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing == false)
                return;

            Parallel.ForEach(_services,
                o => o.Client
                    .CloseAsync()
                    .GetAwaiter()
                    .GetResult());
        }
    }
}
