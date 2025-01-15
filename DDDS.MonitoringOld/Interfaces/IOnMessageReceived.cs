namespace Asis.Framework.Monitoring.Interfaces;

public interface IOnMessageReceived
{
    event Func<string, Task> OnMessageReceivedAsync;
}