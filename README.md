#nuget
> Install-Package Cow.io.ServiceBus -Version 1.0.0 -> abstractions <br />
> Install-Package Cow.io.AzureServiceBus -Version 1.0.0 -> implementation

# Cow.io.ServiceBus
Provides simple structure for publishing/subscribing messages through AzureServiceBus for .net core

#Usage 

1. Create your own saga class that represents the message you want to publish/subscribe, extending MessageBody from Cow.io.ServiceBus.Queue namespace

```
public class PersonCreated : Cow.io.ServiceBus.Queue.MessageBody
{
  public string Firstname { get; set; }
  public string Surname { get; set; }
}
```

2. Use the IServiceCollection extension method from Cow.io.AzureServiceBus namespace to bind the message with an existing Azure ServiceBus queue:

```
 services.AddAzureServiceBusDependency(options =>
          options
          .WithConnectionString(Configuration.GetConnectionString("your-connection-string-goes-here"))
          .WithQueue<PersonCreated>("your-queue-name")
          .WithQueue<PersonUpdated>("your-queue-name")
          .WithQueue<PersonDeleted>("your-queue-name")
  );
```

<b>Publishing a message to the queue</b>

All you need to do is inject IPublisher<> interface in the class you want to publish the message and call Handle() method.

```
public class PersonController
{
   private readonly IPublisher<PersonCreated> _publisher;
   public PersonController(IPublisher<PersonCreated> publisher)
   {
      _publisher = publisher;
   }
   ...
   public async Task<IActionResult> Create(PersonDto person)
   {
       var saga = person.MapTo<PersonCreated>();
       await _publisher.Handle(saga);
   }
}

```

<b>Subscribing</b>

1. Create a class that will handle the message implementing ISubscribe<> interface

```
public class FooSubscriber : ISubscribe<PersonCreated>
{
    public async Task Handle(PersonCreated message)
    {
        await Task.FromResult("Hello from subscriber!");
    }
}
```

That's it!
