using FootballHub.LeagueService.Application.Services;
using FootballHub.LeagueService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "FootballHub - League Service", Version = "v1" });
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

builder.Services.AddDbContext<LeagueDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LeagueDb")));

builder.Services.AddScoped<ILeagueService, LeagueAppService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LeagueDbContext>();
    await db.Database.MigrateAsync();
}

app.UseAuthorization();
app.UseCors("AllowWeb");
app.MapControllers();
app.Run();