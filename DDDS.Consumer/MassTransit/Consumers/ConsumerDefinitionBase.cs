namespace DDDS.Consumer.MassTransit.Consumers
{
    // public class ConsumerDefinitionBase<TConsumer> : ConsumerDefinition<TConsumer> where TConsumer : class, IConsumer
    // {
    //     private new int? ConcurrentMessageLimit { get; }
    //     private string ExchangeType { get; }
    //     private string BindingExchangeName { get; }
    //     private string? RoutingKey { get; }
    //     private bool IsDurable { get; }
    //     private bool UseRetryPolicy { get; }
    //     private int RetryCount { get; }
    //     private TimeSpan? RetryInterval { get; }
    //     private bool UseErrorTransport { get; }
    //
    //     public ConsumerDefinitionBase(string exchangeType,
    //                                   string bindingExchangeName,
    //                                   string? routingKey = null,
    //                                   bool isDurable = false,
    //                                   int? concurrentMessageLimit = null,
    //                                   string? endPoint = null,
    //                                   bool useRetryPolicy = false,
    //                                   int retryLimit = 0,
    //                                   TimeSpan? retryInterval = null,
    //                                   bool useErrorTransport = false)
    //     {
    //         ExchangeType = exchangeType;
    //         BindingExchangeName = bindingExchangeName;
    //         IsDurable = isDurable;
    //         UseRetryPolicy = useRetryPolicy;
    //         RetryCount = retryLimit;
    //         RetryInterval = retryInterval;
    //         UseErrorTransport = useErrorTransport;
    //
    //         if (!string.IsNullOrEmpty(routingKey))
    //             RoutingKey = routingKey;
    //
    //         if (concurrentMessageLimit.HasValue)
    //             ConcurrentMessageLimit = concurrentMessageLimit.Value;
    //
    //         if (!string.IsNullOrEmpty(endPoint))
    //             EndpointName = endPoint;
    //     }
    //
    //     protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<TConsumer> consumerConfigurator,
    //         IRegistrationContext context)
    //     {
    //         endpointConfigurator.ConfigureConsumeTopology = false;
    //
    //         if (UseErrorTransport)
    //         {
    //             //endpointConfigurator.ConfigureError(settings =>
    //             //{
    //             //    if (settings is RabbitMqErrorSettings rabbitMqErrorSettings)
    //             //    {
    //             //        rabbitMqErrorSettings.QueueName = $"{typeof(TConsumer).Name.Replace("Consumer", "")}Error";
    //             //        rabbitMqErrorSettings.ExchangeName = $"{typeof(TConsumer).Name.Replace("Consumer", "")}Error";
    //             //    }
    //             //});
    //             endpointConfigurator.ConfigureDefaultErrorTransport();
    //         }
    //
    //         if (UseRetryPolicy)
    //         {
    //             if (RetryInterval.HasValue)
    //                 endpointConfigurator.UseMessageRetry(x => x.Interval(RetryCount, RetryInterval.Value));
    //             else
    //                 throw new ArgumentNullException("RetryInterval cannot be null when RetryPolicy is enabled!");
    //         }
    //
    //         if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
    //         {
    //             rmq.ExchangeType = ExchangeType;
    //             rmq.Durable = IsDurable;
    //
    //             if (ConcurrentMessageLimit.HasValue)
    //                 rmq.ConcurrentMessageLimit = ConcurrentMessageLimit;
    //
    //             rmq.Bind(BindingExchangeName, e =>
    //             {
    //                 if (!string.IsNullOrEmpty(RoutingKey))
    //                     e.RoutingKey = RoutingKey;
    //
    //                 e.ExchangeType = ExchangeType;
    //             });
    //         }
    //     }
    //
    //     // protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
    //     //     IConsumerConfigurator<TConsumer> consumerConfigurator)
    //     // {
    //     //     endpointConfigurator.ConfigureConsumeTopology = false;
    //     //
    //     //     if (UseErrorTransport)
    //     //     {
    //     //         //endpointConfigurator.ConfigureError(settings =>
    //     //         //{
    //     //         //    if (settings is RabbitMqErrorSettings rabbitMqErrorSettings)
    //     //         //    {
    //     //         //        rabbitMqErrorSettings.QueueName = $"{typeof(TConsumer).Name.Replace("Consumer", "")}Error";
    //     //         //        rabbitMqErrorSettings.ExchangeName = $"{typeof(TConsumer).Name.Replace("Consumer", "")}Error";
    //     //         //    }
    //     //         //});
    //     //         endpointConfigurator.ConfigureDefaultErrorTransport();
    //     //     }
    //     //
    //     //     if (UseRetryPolicy)
    //     //     {
    //     //         if (RetryInterval.HasValue)
    //     //             endpointConfigurator.UseMessageRetry(x => x.Interval(RetryCount, RetryInterval.Value));
    //     //         else
    //     //             throw new ArgumentNullException("RetryInterval cannot be null when RetryPolicy is enabled!");
    //     //     }
    //     //
    //     //     if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
    //     //     {
    //     //         rmq.ExchangeType = ExchangeType;
    //     //         rmq.Durable = IsDurable;
    //     //
    //     //         if (ConcurrentMessageLimit.HasValue)
    //     //             rmq.ConcurrentMessageLimit = ConcurrentMessageLimit;
    //     //
    //     //         rmq.Bind(BindingExchangeName, e =>
    //     //         {
    //     //             if (!string.IsNullOrEmpty(RoutingKey))
    //     //                 e.RoutingKey = RoutingKey;
    //     //
    //     //             e.ExchangeType = ExchangeType;
    //     //         });
    //     //     }
    //     // }
    // }

}

