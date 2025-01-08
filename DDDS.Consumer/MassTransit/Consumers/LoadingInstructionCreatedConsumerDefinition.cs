using System;
using Asis.Framework.Monitoring.Decorators;
using LGW.MessageDistributor.Messagebus.Contract.Events;

namespace DDDS.Consumer.MassTransit.Consumers
{
    public class LoadingInstructionCreatedConsumerDefinition : ConsumerDefinitionBase<LoadingInstructionCreatedConsumer>
    {
        public const string EndPoint = "LoadingInstructionCreated";
        public const string ExchangeType = RabbitMQ.Client.ExchangeType.Fanout;
        public const string BindingExchangeName = "LoadingInstructionCreated";

        public LoadingInstructionCreatedConsumerDefinition() : base(
            exchangeType: ExchangeType,
            bindingExchangeName: BindingExchangeName,
            isDurable: true,
            useRetryPolicy: true,
            retryLimit: 6,
            retryInterval: TimeSpan.FromSeconds(5),
            useErrorTransport: true)
        {
        }
    }
    
    // public class LoadingInstructionCreatedConsumerDefinitionOld : ConsumerDefinitionBase<AsisMassTransitConsumerDecorator<LoadingInstructionCreatedConsumer,LoadingInstructionCreatedEventModel>>
    // {
    //     public const string EndPoint = "LoadingInstructionCreated";
    //     public const string ExchangeType = RabbitMQ.Client.ExchangeType.Fanout;
    //     public const string BindingExchangeName = "LoadingInstructionCreated";
    //
    //     public LoadingInstructionCreatedConsumerDefinition() : base(
    //         exchangeType: ExchangeType,
    //         bindingExchangeName: BindingExchangeName,
    //         isDurable: true,
    //         useRetryPolicy: true,
    //         retryLimit: 6,
    //         retryInterval: TimeSpan.FromSeconds(5),
    //         useErrorTransport: true)
    //     {
    //     }
    // }

}

