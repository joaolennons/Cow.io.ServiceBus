namespace Cow.io.ServiceBus
{
    public interface IServiceBusConfigurationBuilder
    {
        IServiceBusConfigurationBuilder WithConnectionString(string connectionString);
        IServiceBusConfigurationBuilder WithQueue<T>(string queueName);
        IServiceBusConfigurationBuilder WithTopic<T>(string topicName);
        IServiceBusConfigurationBuilder WithTopic<T>(string topicName, string subscriptionName);
    }
}
