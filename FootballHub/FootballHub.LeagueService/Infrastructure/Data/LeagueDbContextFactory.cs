using FootballHub.LeagueService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

public class LeagueDbContextFactory : IDesignTimeDbContextFactory<LeagueDbContext>
{
    public LeagueDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LeagueDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=DESKTOP-O7O7SSV\\SQLEXPRESS01;Database=FootballHub_Leagues;Trusted_Connection=True;TrustServerCertificate=True");

        return new LeagueDbContext(optionsBuilder.Options);
    }
}