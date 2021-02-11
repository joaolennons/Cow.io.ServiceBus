using Cow.io.ServiceBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Cow.io.AzureServiceBus
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseAzureServiceBus(this IApplicationBuilder app, IServiceCollection services)
        {
            var configuration = app.ApplicationServices.GetService<IServiceBusConfiguration>();

            var queueListenerHandler = new AzureServiceBusQueueListenerHandler(app.ApplicationServices, services);
            var topicListenerHandler = new AzureServiceBusTopicListenerHandler(app.ApplicationServices, services);

            foreach (var queue in configuration.Queues)
            {
                var queueType = typeof(IAzureQueueListener<>).MakeGenericType(queue.Key);
                var listenerInstance = app.ApplicationServices.GetService(queueType);
                queueListenerHandler.AddListener((IAzureQueueListener)listenerInstance);
            }

            foreach (var topic in configuration.Topics)
            {
                if (topic.Value.Item2 == null)
                    continue;
                var topicType = typeof(IAzureTopicListener<>).MakeGenericType(topic.Key);
                var listenerInstance = app.ApplicationServices.GetService(topicType);
                topicListenerHandler.AddListener((IAzureTopicListener)listenerInstance);
            }
        }
    }
}
