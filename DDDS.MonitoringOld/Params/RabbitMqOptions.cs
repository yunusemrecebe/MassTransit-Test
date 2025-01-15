namespace LGW.MessageDistributor.MessageBus.Core.Helpers.Monitoring.Params;

public class RabbitMqOptions
{
    public string HostName { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    public string QueueName { get; set; }
    public string ExchangeName { get; set; } = "";

    public string ExchangeType { get; set; }
    // public string ExchangeType
    // {
    //     get => ExchangeType;
    //     set
    //     {
    //         if (!new string[] { "direct", "fanout", "headers", "topic" }.Contains(value?.ToLower()))
    //         {
    //             throw new InvalidCastException();
    //         }
    //     }
    // }

    public string RoutingKey { get; set; } = "";

    public ushort PrefetchCount { get; set; } = ushort.MaxValue;

    public bool AutomaticRecoveryEnabled { get; set; } = true; // RabbitMQ.Client built-in auto recovery
    public bool TopologyRecoveryEnabled { get; set; } = true; // exchange/queue binding recovery

    public bool EnablePublisherConfirms { get; set; } = true;

    public bool DurableQueue { get; set; } = true;
    public bool PersistentMessages { get; set; } = true;

    public bool AutoAck { get; set; } = false;
}