using DDDS.Consumer.MassTransit.Consumers;
using DDDS.Test.WebAPI.Constants;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


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
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("localhost", "/", c =>
                    {
                        c.Username(RabbitMQConstants.Username);
                        c.Password(RabbitMQConstants.Password);
                    });

                    cfg.ReceiveEndpoint(RabbitMQConstants.Events.LoadingInstructionCreated, e =>
                    {
                        e.Consumer<LoadingInstructionCreatedConsumer>();
                        e.Bind("LoadingGateway");
                        e.ConfigureDefaultErrorTransport();
                        e.Durable = true;
                    });

                    cfg.ReceiveEndpoint(RabbitMQConstants.Events.LoadingInstructionThresholdExceeded, e =>
                    {
                        e.Consumer<LoadingInstructionThresholdExceededConsumer>(x =>
                            x.UseRetry(r => r.Incremental(5, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10)))
                        );
                        e.Bind("LoadingGateway");
                        e.ConfigureDefaultErrorTransport();
                    });

                    cfg.ConfigureEndpoints(ctx);
                });
            });
        });
}