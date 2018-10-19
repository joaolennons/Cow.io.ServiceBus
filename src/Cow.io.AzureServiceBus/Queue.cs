namespace Cow.io.AzureServiceBus
{
    internal class Queue<T>
    {
        public Queue(string queueName)
        {
            QueueName = queueName;
        }

        public string QueueName { get; }
    }
}
