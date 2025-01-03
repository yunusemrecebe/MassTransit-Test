using Asis.Framework.Monitoring.Interfaces;

namespace Asis.Framework.Monitoring.Decorators
{
    public abstract class MessageBusDecorator : IMessageBus
    {
        protected readonly IMessageBus InnerBus;

        protected MessageBusDecorator(IMessageBus innerBus)
        {
            InnerBus = innerBus;
        }

        public virtual Task SendAsync<T>(T message) where T : class
        {
            return InnerBus.SendAsync(message);
        }

        public virtual Task StartConsuming()
        {
            return InnerBus.StartConsuming();
        }

        public virtual Task Consume()
        {
            return InnerBus.Consume();
        }

        public virtual Task Consume<T>(T message)
        {
            return InnerBus.Consume(message);
        }
    }
}
