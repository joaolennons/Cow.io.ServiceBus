using System;
using Cow.io.ServiceBus;
using Microsoft.Extensions.DependencyInjection;

namespace Cow.io.AzureServiceBus
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureServiceBusDependency(this IServiceCollection services, Action<IServiceBusConfigurationBuilder> options)
        {
            var builder = new AzureServiceBusConfigurationBuilder();
            options.Invoke(builder);
            var configuration = builder.Build();

            services.AddSingleton<IHasConnectionString>(configuration);
            services.AddSingleton(configuration);

            foreach (var queue in configuration.Queues)
            {
                var queueType = typeof(Queue<>).MakeGenericType(queue.Key);
                var queueInstance = Activator.CreateInstance(queueType, queue.Value);
                services.AddSingleton(queueType, queueInstance);
            }

            foreach (var topic in configuration.Topics)
            {
                var topicType = typeof(Queue<>).MakeGenericType(topic.Key);
                var topicInstance = Activator.CreateInstance(topicType, topic.Value);
                services.AddSingleton(topicType, topicInstance);
            }

            services.AddTransient(typeof(IAzureQueueListener<>), typeof(AzureQueueListener<>));
            services.AddTransient(typeof(IAzureTopicListener<>), typeof(AzureTopicListener<>));
            services.AddTransient(typeof(IAzureTopicWriter<>), typeof(AzureTopicWriter<>));
            services.AddTransient(typeof(IPublisher<>), typeof(AzureServiceBusTopicPublisher<>));
            services.AddTransient(typeof(IPublisher<>), typeof(AzureServiceBusQueuePublisher<>));

            return services;
        }
    }
}
