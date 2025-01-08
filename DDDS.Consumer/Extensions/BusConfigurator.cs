using System;
using DDDS.Test.WebAPI.Constants;
using MassTransit;

namespace DDDS.Test.WebAPI.Extensions
{
    public static class BusConfigurator
    {
        public static IBusControl ConfigureBus(Action<IRabbitMqBusFactoryConfigurator> registrationAction = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(configuration =>
            {
                configuration.Host(RabbitMQConstants.Uri, hostConfiguration =>
                {
                    hostConfiguration.Username(RabbitMQConstants.Username);
                    hostConfiguration.Password(RabbitMQConstants.Password);
                });

                registrationAction?.Invoke(configuration);
            });
        }
    }
}
