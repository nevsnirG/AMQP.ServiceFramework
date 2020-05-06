# AMQP.ServiceFramework
Framework for attribute-based message routing.

## Introduction
ServiceFramework allows for ASP-like control over message routing. Using attribute-based mapping messages can be routed to handlers methods. It automatically discovers all handler methods in the executing assembly and subscribes to the message queue for you. Dependency injection is integrated using the Microsoft.Extensions.DependencyInjection framework and Logging is integrated using the Microsoft.Extensions.Logging framework.

## Example
The following example uses the [RabbitMQ](https://github.com/nevsnirG/AMQP.Plugin.Abstractions/tree/master/src/AMQP.Plugin.RabbitMQ) implementation of the [AMQP.Plugin.Abstractions](https://github.com/nevsnirG/AMQP.Plugin.Abstractions) framework.

```
public class Program : ServiceBuilder
{
    public Program(IConnection connection) : base(connection)
    {
    }

    private static async Task Main()
    {
        var connectionString = "amqp://guest:guest@localhost:5672/";
        var connectionFactory = new RabbitMQConnectionFactory(connectionString);
        using var connection = connectionFactory.CreateConnection(nameof(Instagram));
        using var application = new Program(connection);
        application.EnsureInitialization();

        await Task.Delay(Timeout.Infinite);
    }

    protected override void Setup(IConfiguration config)
    {
        config.Services.AddLogging((builder) =>
        {
            builder.AddConsole();
        });

        config.Services.AddTransient<IDependency, Dependency>();
    }
}
```

Handler classes need to be attributed with the TopicClient attribute and need to implement the ISubscription interface. Its handler methods need to be attributed with the TopicSusbcription attribute, with the routing key and type of MessageParser as parameter.
```
[TopicClient]
internal sealed class MessageHandler : ISubscription<string>
{
    [TopicSubscription("routing_key", typeof(StringMessageParser)]
    public void Handle(string input)
    {
        Console.WriteLine(input);
    }
}

internal sealed class StringMessageParser : MessageParser
{
    public override Type Type => typeof(string);

    public override object Parse(byte[] body)
    {
        return Encoding.UTF8.GetString(body);
    }
}
```
