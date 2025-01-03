using Asis.Framework.Monitoring.Interfaces;
using MassTransit;

namespace Asis.Framework.Monitoring.MessageBuses
{
    public class MassTransitMessageBus : IMessageBus
    {
        private readonly IBus _bus;

        public MassTransitMessageBus(IBus bus)
        {
            _bus = bus;
        }

        public Task SendAsync<T>(T message) where T : class
        {
            return _bus.Publish(message);
        }

        public Task StartConsuming()
        {
            return Task.CompletedTask;
        }

        public Task Consume()
        {
            return Task.CompletedTask;
        }

        public Task Consume<T>(T message)
        {
            return Task.CompletedTask;
        }
    }
}