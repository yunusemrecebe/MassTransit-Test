using System.Text.Json;
using Asis.Framework.Monitoring;
using Asis.Framework.Monitoring.Contract.Models;
using Asis.Framework.Monitoring.RabbitMq;
using Asis.Framework.Monitoring.RabbitMq.MessageBuses;
using DDDS.Consumer.MassTransit.Consumers;
using DDDS.Test.WebAPI.Constants;
using LGW.MessageDistributor.Messagebus.Contract.Events;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using RegistrationContextExtensions = MassTransit.RegistrationContextExtensions;
using RegistrationExtensions = MassTransit.RegistrationExtensions;

namespace DDDS.Consumer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost app = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IMetricsLogger, PrometheusMetricsLogger>();
                    services.RegisterAsisMetricsLogger();

                    IConfigurationSection section = hostContext.Configuration.GetSection("RabbitMqOptions");
                    RabbitMqOptions options = section.Get<RabbitMqOptions>() ??
                                              throw new ArgumentNullException(nameof(RabbitMqOptions));

                    services.BuildAsisRabbitMqConsumer<RabbitMqMessageBus>(JsonSerializer.Serialize(options));
                }).Build();
            
            await app.StartAsync();
            
            var metricServer = new KestrelMetricServer(port: 1234);
            metricServer.Start();
            
            IMessageBus bus = app.Services.GetRequiredService<IMessageBus>();
            await TestRabbitMq(bus);
            
            await app.StopAsync();
        }

        private static async Task TestRabbitMq(IMessageBus bus)
        {
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
        }

        private static void MassTransit(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                RegistrationExtensions.AddConsumer<LoadingInstructionCreatedConsumer,LoadingInstructionCreatedConsumerDefinition>(x);

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    Uri uri = new(
                        $"amqp://{RabbitMQConstants.Host}:{RabbitMQConstants.Port}");
                    cfg.Host(uri, "/", c =>
                    {
                        c.Username(RabbitMQConstants.Username);
                        c.Password(RabbitMQConstants.Password);
                    });
                    
                    RegistrationContextExtensions.ConfigureEndpoints(cfg, ctx);
                });
            });
        }
        
        private static void MassTransitOld(IServiceCollection services)
        {
            // services.AddTransient<LoadingInstructionCreatedConsumer>();
            //
            // services.AddMassTransit(x =>
            // {
            //     x.AddConsumer<AsisMassTransitConsumerDecorator<LoadingInstructionCreatedConsumer,LoadingInstructionCreatedEventModel>,LoadingInstructionCreatedConsumerDefinition>();
            //
            //     x.UsingRabbitMq((ctx, cfg) =>
            //     {
            //         Uri uri = new(
            //             $"amqp://{RabbitMQConstants.Host}:{RabbitMQConstants.Port}");
            //         cfg.Host(uri, "/", c =>
            //         {
            //             c.Username(RabbitMQConstants.Username);
            //             c.Password(RabbitMQConstants.Password);
            //         });
            //
            //         cfg.ReceiveEndpoint("LoadingInstructionCreated",e =>
            //             {
            //                 e.ConfigureConsumer<AsisMassTransitConsumerDecorator<LoadingInstructionCreatedConsumer,LoadingInstructionCreatedEventModel>>(ctx);
            //             });
            //
            //         cfg.ConfigureEndpoints(ctx);
            //     });
            // });
            //
            // services.AddSingleton<MassTransitMessageBus>();
            // services.AddSingleton<IMessageBus>(sp =>
            // {
            //     var realBus = sp.GetRequiredService<MassTransitMessageBus>();
            //     return new LoggingDecorator(realBus);
            // });
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
            //
            // using var rabbitBus = new RabbitMqMessageBus(options);
            // IMessageBus bus = new LoggingDecorator(rabbitBus);
            //
            // await bus.StartConsuming();
        }
    }
}