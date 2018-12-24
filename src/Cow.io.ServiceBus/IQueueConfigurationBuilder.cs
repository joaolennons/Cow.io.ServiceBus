namespace Cow.io.ServiceBus
{
    public interface IQueueConfigurationBuilder
    {
        IQueueConfigurationBuilder WithConnectionString(string connectionString);
        IQueueConfigurationBuilder WithQueue<T>(string queueName);
    }
}
