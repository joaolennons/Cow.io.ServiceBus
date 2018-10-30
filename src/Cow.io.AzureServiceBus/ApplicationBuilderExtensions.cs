using Cow.io.ServiceBus.Queue;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Cow.io.AzureServiceBus
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseAzureServiceBus(this IApplicationBuilder app)
        {
            var configuration = app.ApplicationServices.GetService<IQueueConfiguration>();

            var listenerHandler = new AzureServiceBusListenerHandler(app.ApplicationServices);

            foreach (var queue in configuration.Queues)
            {
                var queueType = typeof(IAzureQueueListener<>).MakeGenericType(queue.Key);
                var listenerInstance = app.ApplicationServices.GetService(queueType);
                listenerHandler.AddListener((IAzureQueueListener)listenerInstance);
            }
        }
    }
}
