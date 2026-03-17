using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FootballHub.MatchService.Infrastructure.Data;

public class MatchDbContextFactory : IDesignTimeDbContextFactory<MatchDbContext>
{
    public MatchDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MatchDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=DESKTOP-O7O7SSV\\SQLEXPRESS01;Database=FootballHub_Matches;Trusted_Connection=True;TrustServerCertificate=True");

        return new MatchDbContext(optionsBuilder.Options);
    }
}