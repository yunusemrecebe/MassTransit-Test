using DDDS.Test.WebAPI.Constants;
using LGW.MessageDistributor.Messagebus.Contract.Events;
using LGW.MessageDistributor.MessageBus.Core.Consts;
using MassTransit;
using RabbitMQ.Client;

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


        #region [ Event Model Configurations ]

        #region [ LoadingInstructionCreatedThresholdExceeded ]

        cfg.Message<LoadingInstructionCreatedThresholdExceededEventModel>(e => e.SetEntityName(MessageBusConsts.Events.LoadingInstructionThresholdExceeded)); // name of the primary exchange
        cfg.Publish<LoadingInstructionCreatedThresholdExceededEventModel>(e => e.ExchangeType = ExchangeType.Topic); // primary exchange type
        cfg.Send<LoadingInstructionCreatedThresholdExceededEventModel>(e =>
        {
            e.UseCorrelationId(x =>
            {
                if (Guid.TryParse(x.CorrelationId, out Guid correlationId))
                {
                }
                else
                {
                    if (string.IsNullOrEmpty(x.CorrelationId))
                    {
                        correlationId = Guid.NewGuid();
                    }
                    else
                    {

                        string c = "";
                        x.CorrelationId.TakeLast(36).ToList().ForEach(x => c += x);

                        if (Guid.TryParse(c, out correlationId))
                        {

                        }
                        else
                        {
                            correlationId = Guid.NewGuid();
                        }
                    }
                }

                return correlationId;
            });
            e.UseRoutingKeyFormatter(context =>
            {
                return $"{MessageBusConsts.RoutingKeys.LoadingInstructionCreatedExceeded}.{context.Message.CityCode}";
            });
        });


        #endregion

        #region [ LoadingInstructionAppliedThresholdExceeded ]

        cfg.Message<LoadingInstructionAppliedThresholdExceededEventModel>(e => e.SetEntityName(MessageBusConsts.Events.LoadingInstructionThresholdExceeded)); // name of the primary exchange
        cfg.Publish<LoadingInstructionAppliedThresholdExceededEventModel>(e => e.ExchangeType = ExchangeType.Topic); // primary exchange type
        cfg.Send<LoadingInstructionAppliedThresholdExceededEventModel>(e =>
        {
            e.UseCorrelationId(x =>
            {
                if (Guid.TryParse(x.CorrelationId, out Guid correlationId))
                {
                }
                else
                {
                    if (string.IsNullOrEmpty(x.CorrelationId))
                    {
                        correlationId = Guid.NewGuid();
                    }
                    else
                    {

                        string c = "";
                        x.CorrelationId.TakeLast(36).ToList().ForEach(x => c += x);

                        if (Guid.TryParse(c, out correlationId))
                        {

                        }
                        else
                        {
                            correlationId = Guid.NewGuid();
                        }
                    }
                }

                return correlationId;
            });
            e.UseRoutingKeyFormatter(context =>
            {
                return $"{MessageBusConsts.RoutingKeys.LoadingInstructionAppliedExceeded}.{context.Message.CityCode}";
            });
        });

        #endregion

        #region [ LoadingInstructionCancelledThresholdExceeded ]

        cfg.Message<LoadingInstructionCancelledThresholdExceededEventModel>(e => e.SetEntityName(MessageBusConsts.Events.LoadingInstructionThresholdExceeded)); // name of the primary exchange
        cfg.Publish<LoadingInstructionCancelledThresholdExceededEventModel>(e => e.ExchangeType = ExchangeType.Topic); // primary exchange type
        cfg.Send<LoadingInstructionCancelledThresholdExceededEventModel>(e =>
        {
            e.UseCorrelationId(x =>
            {
                if (Guid.TryParse(x.CorrelationId, out Guid correlationId))
                {
                }
                else
                {
                    if (string.IsNullOrEmpty(x.CorrelationId))
                    {
                        correlationId = Guid.NewGuid();
                    }
                    else
                    {

                        string c = "";
                        x.CorrelationId.TakeLast(36).ToList().ForEach(x => c += x);

                        if (Guid.TryParse(c, out correlationId))
                        {

                        }
                        else
                        {
                            correlationId = Guid.NewGuid();
                        }
                    }
                }

                return correlationId;
            });
            e.UseRoutingKeyFormatter(context =>
            {
                return $"{MessageBusConsts.RoutingKeys.LoadingInstructionCancelledExceeded}.{context.Message.CityCode}";
            });
        });

        #endregion

        #endregion

        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddStackExchangeRedisCache(action => {
    action.Configuration = "localhost:6379";
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
