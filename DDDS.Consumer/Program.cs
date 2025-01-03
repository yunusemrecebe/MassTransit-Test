using DDDS.Consumer.MassTransit.Consumers;
using Asis.Framework.Monitoring.Decorators;
using Asis.Framework.Monitoring.Interfaces;
using Asis.Framework.Monitoring.MessageBuses;
using DDDS.Test.WebAPI.Constants;
using LGW.MessageDistributor.Messagebus.Contract.Events;
using LGW.MessageDistributor.MessageBus.Core.Helpers.Monitoring.Params;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DDDS.Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // MassTransit(services);

                    RabbitMQ().GetAwaiter().GetResult();
                });

        private static void MassTransit(IServiceCollection services)
        {
            services.AddTransient<LoadingInstructionCreatedConsumer>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<AsisMassTransitConsumerDecorator<LoadingInstructionCreatedConsumer,LoadingInstructionCreatedEventModel>,LoadingInstructionCreatedConsumerDefinition>();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    Uri uri = new(
                        $"amqp://{RabbitMQConstants.Host}:{RabbitMQConstants.Port}");
                    cfg.Host(uri, "/", c =>
                    {
                        c.Username(RabbitMQConstants.Username);
                        c.Password(RabbitMQConstants.Password);
                    });

                    cfg.ReceiveEndpoint("LoadingInstructionCreated",e =>
                        {
                            e.ConfigureConsumer<AsisMassTransitConsumerDecorator<LoadingInstructionCreatedConsumer,LoadingInstructionCreatedEventModel>>(ctx);
                        });

                    cfg.ConfigureEndpoints(ctx);
                });
            });

            services.AddSingleton<MassTransitMessageBus>();
            services.AddSingleton<IMessageBus>(sp =>
            {
                var realBus = sp.GetRequiredService<MassTransitMessageBus>();
                return new LoggingDecorator(realBus);
            });
        }

        private static async Task RabbitMQ()
        {
            var options = new RabbitMqOptions
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
                QueueName = "LoadingInstructionCreated",
                ExchangeName = "LoadingInstructionCreated",
                ExchangeType = "fanout",
                RoutingKey = "",
                PrefetchCount = 5,
                AutoAck = true,
                EnablePublisherConfirms = true,
                PersistentMessages = true
            };

            using var rabbitBus = new RabbitMqMessageBus(options);
            IMessageBus bus = new LoggingDecorator(rabbitBus);

            await bus.StartConsuming();

            #region [ Test ]

            Console.WriteLine("Press ENTER to publish a test message, Q to quit.");
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Q) break;
                if (key.Key == ConsoleKey.Enter)
                {
                    var testMessage = new LoadingInstructionCreatedEventModel
                    {
                        CityCode = 33,
                        CorrelationId = Guid.NewGuid().ToString(),
                        DeliveryType = "Instantly",
                        RefNo = Guid.NewGuid().ToString()
                    };
                    await bus.SendAsync(testMessage);
                }
            }

            Console.WriteLine("Exiting...");

            #endregion
        }
    }
}