using App.Controllers;
using App.Subscribers;
using Cow.io.ServiceBus.Queue;
using Microsoft.Extensions.DependencyInjection;

namespace App
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiDependency(this IServiceCollection services)
        {
            services.AddTransient<ISubscribe<MessageSaga>, SubscriberDefault>();
            return services;
        }
    }
}
