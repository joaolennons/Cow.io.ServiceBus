using System;
using Cow.io.ServiceBus;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddSingleton(configuration);

            foreach (var queue in configuration.Queues)
            {
                var queueType = typeof(Queue<>).MakeGenericType(queue.Key);
                var queueInstance = Activator.CreateInstance(queueType, queue.Value);
                services.AddSingleton(queueType, queueInstance);
            }

            services.AddTransient(typeof(IAzureQueueListener<>), typeof(AzureQueueListener<>));
            services.AddTransient(typeof(IPublisher<>), typeof(AzureServiceBusQueuePublisher<>));

            return services;
        }
    }
}
