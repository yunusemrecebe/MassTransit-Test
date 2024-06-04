using DDDS.Test.WebAPI.Constants;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    //x.SetKebabCaseEndpointNameFormatter();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", c =>
        {
            c.Username(RabbitMQConstants.Username);
            c.Password(RabbitMQConstants.Password);
        });

        cfg.ReceiveEndpoint(RabbitMQConstants.Events.LoadingInstructionCreated, e =>
        {
            e.Bind("LoadingGateway");
            e.ConfigureDefaultErrorTransport();
            e.Durable = true;
        });

        cfg.ReceiveEndpoint(RabbitMQConstants.Events.LoadingInstructionThresholdExceeded, e =>
        {
            e.Bind("LoadingGateway");
            e.ConfigureDefaultErrorTransport();
        });

        cfg.ConfigureEndpoints(ctx);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
