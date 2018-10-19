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
                var instance = Activator.CreateInstance(queueType, queue.Value);
                services.AddSingleton(queueType, instance);
            }

            services.AddTransient(typeof(IAzureServiceQueue<>), typeof(AzureServiceQueue<>));
            services.AddTransient(typeof(IPublisher<>), typeof(AzureServiceBusPublisher<>));

            return services;
        }
    }
}
