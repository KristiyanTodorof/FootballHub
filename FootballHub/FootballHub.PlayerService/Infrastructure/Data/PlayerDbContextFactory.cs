using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FootballHub.PlayerService.Infrastructure.Data;

public class PlayerDbContextFactory : IDesignTimeDbContextFactory<PlayerDbContext>
{
    public PlayerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PlayerDbContext>();
        optionsBuilder.UseSqlServer(
           "Server=DESKTOP-O7O7SSV\\SQLEXPRESS01;Database=FootballHub_Matches;Trusted_Connection=True;TrustServerCertificate=True");

        return new PlayerDbContext(optionsBuilder.Options);
    }
}