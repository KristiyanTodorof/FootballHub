using FootballHub.PlayerService.Application.Services;
using FootballHub.PlayerService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "FootballHub - Player Service", Version = "v1" });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWeb", policy =>
    {
        policy.WithOrigins("http://localhost:5005")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<PlayerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PlayerDb")));

builder.Services.AddScoped<IPlayerService, PlayerAppService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PlayerDbContext>();
    await db.Database.MigrateAsync();
}

app.UseAuthorization();
app.UseCors("AllowWeb");
app.MapControllers();
app.Run();