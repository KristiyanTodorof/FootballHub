using FootballHub.LeagueService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FootballHub.LeagueService.Infrastructure.Data;

public class LeagueDbContext : DbContext
{
    public LeagueDbContext(DbContextOptions<LeagueDbContext> options) : base(options) { }

    public DbSet<League> Leagues => Set<League>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Standing> Standings => Set<Standing>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<League>(entity =>
        {
            entity.ToTable("Leagues");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Country).HasMaxLength(100).IsRequired();
            entity.Property(e => e.LogoUrl).HasMaxLength(500);
            entity.HasMany(e => e.Teams)
                  .WithOne(e => e.League)
                  .HasForeignKey(e => e.LeagueId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.Standings)
                  .WithOne(e => e.League)
                  .HasForeignKey(e => e.LeagueId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.ToTable("Teams");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.ShortName).HasMaxLength(10).IsRequired();
            entity.Property(e => e.Country).HasMaxLength(100).IsRequired();
            entity.Property(e => e.LogoUrl).HasMaxLength(500);
        });

        modelBuilder.Entity<Standing>(entity =>
        {
            entity.ToTable("Standings");
            entity.HasKey(e => e.Id);
            entity.Ignore(e => e.GoalDifference);
            entity.Ignore(e => e.Points);
            entity.HasIndex(e => new { e.LeagueId, e.TeamId }).IsUnique();
        });
    }
}