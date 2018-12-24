using Cow.io.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace Cow.io.AzureServiceBus
{
    internal class Message : Microsoft.Azure.ServiceBus.Message
    {
        public Message(IMessage body)
        {
            this.Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body));
            this.Label = JsonConvert.SerializeObject(new Header
            {
                MessageType = body.GetType()
            });
        }
    }
}
