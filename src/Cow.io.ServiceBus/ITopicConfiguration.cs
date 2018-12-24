using System;
using System.Collections.Generic;

namespace Cow.io.ServiceBus
{
    public interface ITopicConfiguration : IHasConnectionString
    {
        Dictionary<Type, Tuple<string, string>> Topics { get; }
    }
}

