using FootballHub.NotificationService.Application.Consumers;
using FootballHub.NotificationService.Application.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "FootballHub - Notification Service", Version = "v1" });
});

builder.Services.AddScoped<INotificationService, NotificationAppService>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<MatchStartedConsumer>();
    x.AddConsumer<GoalScoredConsumer>();
    x.AddConsumer<MatchFinishedConsumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
        });

        cfg.ReceiveEndpoint("match-started", e =>
        {
            e.ConfigureConsumer<MatchStartedConsumer>(ctx);
        });

        cfg.ReceiveEndpoint("goal-scored", e =>
        {
            e.ConfigureConsumer<GoalScoredConsumer>(ctx);
        });

        cfg.ReceiveEndpoint("match-finished", e =>
        {
            e.ConfigureConsumer<MatchFinishedConsumer>(ctx);
        });
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();