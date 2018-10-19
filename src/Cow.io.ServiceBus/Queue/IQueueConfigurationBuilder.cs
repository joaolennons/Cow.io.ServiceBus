namespace Cow.io.ServiceBus.Queue
{
    public interface IQueueConfigurationBuilder
    {
        IQueueConfigurationBuilder WithConnectionString(string connectionString);
        IQueueConfigurationBuilder WithQueue<T>(string queueName);
    }
}
