namespace Cow.io.ServiceBus
{
    public interface IServiceBusSerializer<T>
    {
        string Serialize(T value);
        T Deserialize(string value);
    }
}
