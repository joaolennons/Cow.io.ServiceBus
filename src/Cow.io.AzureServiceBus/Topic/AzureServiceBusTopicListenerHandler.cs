using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cow.io.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Cow.io.AzureServiceBus
{
    internal class AzureServiceBusTopicListenerHandler : IDisposable
    {
        private readonly IServiceProvider _provider;
        private readonly IList<IAzureTopicListener> _services;

        public AzureServiceBusTopicListenerHandler(IServiceProvider provider)
        {
            _provider = provider;
            _services = new List<IAzureTopicListener>();
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
                Debug.WriteLine($"Message of type has been received:{message.Label}");
                var body = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(message.Body), header.MessageType);
                Debug.WriteLine($"With body:{body}");

                var subscribers = _provider.GetServices(typeof(ISubscribe<>).MakeGenericType(header.MessageType));
                var listener = _services.FirstOrDefault(lst => typeof(IAzureQueueListener<>).MakeGenericType(header.MessageType).IsAssignableFrom(lst.GetType()));

                foreach (var subscriber in subscribers)
                {
                    try
                    {
                        Debug.WriteLine($"And will be sent to:{subscriber}");
                        await ((dynamic)subscriber).Handle((dynamic)body);
                        await listener.Client.CompleteAsync(message.SystemProperties.LockToken);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"An exception has occurred:{ex.Message}");
                        await listener.Client.AbandonAsync(message.SystemProperties.LockToken);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception has occurred:{ex.Message}");
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
