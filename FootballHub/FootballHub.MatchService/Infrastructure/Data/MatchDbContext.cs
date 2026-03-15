using FootballHub.MatchService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FootballHub.MatchService.Infrastructure.Data;

public class MatchDbContext : DbContext
{
    public MatchDbContext(DbContextOptions<MatchDbContext> options) : base(options) { }

    public DbSet<Match> Matches => Set<Match>();
    public DbSet<MatchEvent> MatchEvents => Set<MatchEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Match>(entity =>
        {
            entity.ToTable("Matches");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.HomeTeamName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.AwayTeamName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.LeagueName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Status).HasConversion<string>();
            entity.HasMany(e => e.Events)
                  .WithOne(e => e.Match)
                  .HasForeignKey(e => e.MatchId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => e.KickOff);
            entity.HasIndex(e => e.Status);
        });

        modelBuilder.Entity<MatchEvent>(entity =>
        {
            entity.ToTable("MatchEvents");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PlayerName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Team).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Type).HasConversion<string>();
        });
    }
}