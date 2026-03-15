using FootballHub.MatchService.Application.Services;
using FootballHub.MatchService.Infrastructure.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "FootballHub - Match Service", Version = "v1" });
});

builder.Services.AddDbContext<MatchDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MatchDb")));

builder.Services.AddScoped<IMatchService, MatchAppService>();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
        });
        cfg.ConfigureEndpoints(ctx);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MatchDbContext>();
    await db.Database.MigrateAsync();
}

app.UseAuthorization();
app.MapControllers();
app.Run();