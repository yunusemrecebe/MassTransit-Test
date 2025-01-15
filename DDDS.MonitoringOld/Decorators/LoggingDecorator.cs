using Asis.Framework.Monitoring.Interfaces;
using Asis.Framework.Monitoring.MessageBuses;

namespace Asis.Framework.Monitoring.Decorators
{
    public class LoggingDecorator : MessageBusDecorator
    {
        public LoggingDecorator(IMessageBus innerBus) : base(innerBus)
        {
            if (innerBus is IOnMessageReceived onReceivedBus)
            {
                onReceivedBus.OnMessageReceivedAsync += Consume;
            }
        }

        public override async Task SendAsync<T>(T message)
        {
            Console.WriteLine($"[LoggingDecorator] Sending message of type {typeof(T).Name}...");
            await base.SendAsync(message);
            Console.WriteLine("[LoggingDecorator] Message sent.");
        }

        //Burası kullanılmıyor
        public override async Task StartConsuming()
        {
            Console.WriteLine("[LoggingDecorator] Starting consumer...");
            await base.StartConsuming();
            Console.WriteLine("[LoggingDecorator] Consumer started.");
        }

        //Burası kullanılmıyor
        public override async Task Consume()
        {
            Console.WriteLine("[LoggingDecorator] Consuming...");
            await base.Consume();
            Console.WriteLine("[LoggingDecorator] Consumed...");
        }
        
        public override async Task Consume<T>(T message)
        {
            Console.WriteLine("[LoggingDecorator] Consuming...");
            await base.Consume(message);
            Console.WriteLine("[LoggingDecorator] Consumed...");
        }
    }
}