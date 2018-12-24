namespace Cow.io.ServiceBus
{
    public interface ISubscribe<T> : IMessagingHandler<T> where T : IMessage
    {
    }
}
