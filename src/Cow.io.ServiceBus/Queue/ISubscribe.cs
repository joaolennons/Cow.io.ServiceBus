namespace Cow.io.ServiceBus.Queue
{
    public interface ISubscribe<T> : IMessagingHandler<T> where T : IMessage
    {
    }
}
