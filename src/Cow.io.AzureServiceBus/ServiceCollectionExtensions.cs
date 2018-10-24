using Cow.io.ServiceBus.Queue;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Cow.io.AzureServiceBus
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureServiceBusDependency(this IServiceCollection services, Action<IQueueConfigurationBuilder> options)
        {
            var builder = new QueueConfigurationBuilder();
            options.Invoke(builder);
            var configuration = builder.Build();

            services.AddSingleton<IHasConnectionString>(configuration);

            foreach (var queue in configuration.Queues)
            {
                var queueType = typeof(Queue<>).MakeGenericType(queue.Key);
                var queueInstance = Activator.CreateInstance(queueType, queue.Value);
                services.AddSingleton(queueType, queueInstance);
            }

            services.AddTransient(typeof(IAzureQueueListener<>), typeof(AzureQueueListener<>));
            services.AddTransient(typeof(IPublisher<>), typeof(AzureServiceBusPublisher<>));

            var provider = services.BuildServiceProvider();

            var listenerHandler = new AzureServiceBusListenerHandler(provider);

            foreach (var queue in configuration.Queues)
            {
                var queueType = typeof(IAzureQueueListener<>).MakeGenericType(queue.Key);
                var listenerInstance = provider.GetService(queueType);
                listenerHandler.AddListener((IAzureQueueListener)listenerInstance);
            }

            services.AddSingleton(listenerHandler);

            return services;
        }
    }
}
