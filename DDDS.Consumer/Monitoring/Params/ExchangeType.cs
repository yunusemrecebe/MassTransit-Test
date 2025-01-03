namespace LGW.MessageDistributor.MessageBus.Core.Helpers.Monitoring.Params;

public struct ExchangeType
{
    public const string Direct = "direct";
    public const string Fanout = "fanout";
    public const string Headers = "headers";
    public const string Topic = "topic";
}