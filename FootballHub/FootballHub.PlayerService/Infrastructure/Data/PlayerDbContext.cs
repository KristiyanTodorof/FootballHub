using FootballHub.PlayerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FootballHub.PlayerService.Infrastructure.Data;

public class PlayerDbContext : DbContext
{
    public PlayerDbContext(DbContextOptions<PlayerDbContext> options) : base(options) { }

    public DbSet<Player> Players => Set<Player>();
    public DbSet<PlayerStatistic> PlayerStatistics => Set<PlayerStatistic>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>(entity =>
        {
            entity.ToTable("Players");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            entity.Ignore(e => e.FullName);
            entity.Property(e => e.Nationality).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Position).HasMaxLength(50).IsRequired();
            entity.Property(e => e.PhotoUrl).HasMaxLength(500);
            entity.Property(e => e.TeamName).HasMaxLength(100).IsRequired();
            entity.HasMany(e => e.Statistics)
                  .WithOne(e => e.Player)
                  .HasForeignKey(e => e.PlayerId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => e.TeamId);
            entity.HasIndex(e => e.LeagueId);
        });

        modelBuilder.Entity<PlayerStatistic>(entity =>
        {
            entity.ToTable("PlayerStatistics");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LeagueName).HasMaxLength(100).IsRequired();
            entity.HasIndex(e => new { e.PlayerId, e.Season, e.LeagueId }).IsUnique();
        });
    }
}