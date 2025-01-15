namespace Asis.Framework.Monitoring.Interfaces;

public interface IMessageBus
{
    Task SendAsync<T>(T message) where T : class;
    Task StartConsuming();
    Task Consume();
    Task Consume<T>(T message);
}
