using MassTransit;

namespace Asis.Framework.Monitoring.Decorators;

public class AsisMassTransitConsumerDecorator<TConsumer, TMessage> : IConsumer<TMessage>
    where TConsumer : class, IConsumer<TMessage>
    where TMessage : class
{
    private readonly TConsumer _innerConsumer;

    public AsisMassTransitConsumerDecorator(TConsumer innerConsumer)
    {
        _innerConsumer = innerConsumer;
    }

    //Mesaj geldiğinde MassTransit Consumer event fırlatılırsa bu class direkt kaldırlabilir. MessageBusDecorator event ile mesajı okuyabiliyor.
    public async Task Consume(ConsumeContext<TMessage> context)
    {
        Console.WriteLine($"[Decorator] Before consuming {typeof(TMessage).Name}");

        await _innerConsumer.Consume(context);
        
        Console.WriteLine($"[Decorator] After consuming {typeof(TMessage).Name}");
    }
}