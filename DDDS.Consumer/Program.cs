using DDDS.Consumer.MassTransit.Consumers;
using DDDS.Test.WebAPI.Constants;
using MassTransit;
using MassTransit.Transports.Fabric;
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

                x.AddConsumer<LoadingInstructionCreatedConsumer>()
                    .Endpoint(x => x.Name = RabbitMQConstants.Events.LoadingInstructionCreated);

                x.AddConsumer<LoadingInstructionThresholdExceededConsumer>(x => 
                        x.UseRetry(r => r.Incremental(5, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10)))
                    )
                    .Endpoint(x => x.Name = RabbitMQConstants.Events.LoadingInstructionCreated); ;

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("localhost", "/", c =>
                    {
                        c.Username(RabbitMQConstants.Username);
                        c.Password(RabbitMQConstants.Password);
                    });

                    cfg.ConfigureEndpoints(ctx);
                });
            });
        });
}


//IBusControl bus = BusConfigurator.ConfigureBus(configuration =>
//{
//    configuration.ReceiveEndpoint(RabbitMQConstants.Events.LoadingInstructionThresholdExceeded, e =>
//    {
//        //e.Batch<QueueMessage>(e => e.Consumer<LoadingInstructionThresholdExceededConsumer>());



//        e.Consumer<LoadingInstructionThresholdExceededConsumer>(x => x.UseRetry(r => r.Incremental(5, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10))));
//    });

//    //configuration.ReceiveEndpoint(RabbitMQConstants.Events.LoadingInstructionCreated, e =>
//    //{
//    //    e.Consumer<LoadingInstructionThresholdExceededConsumer>(x => x.UseRetry(r => r.Incremental(5, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10))));
//    //});
//});

//await bus.StartAsync();

//Console.WriteLine("Listening order commands... Press any key to exit.");
//Console.ReadKey();

//await bus.StopAsync();