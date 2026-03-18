using GWWWlogin.GatewayService.Models;
using Microsoft.EntityFrameworkCore;

namespace GWWWlogin.GatewayService.Data;

public sealed class GatewayDbContext(DbContextOptions<GatewayDbContext> options) : DbContext(options)
{
    public DbSet<GatewaySessionRecord> Sessions => Set<GatewaySessionRecord>();

    public DbSet<GatewayCharacterRecord> Characters => Set<GatewayCharacterRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var session = modelBuilder.Entity<GatewaySessionRecord>();
        session.ToTable("sessions");
        session.HasKey(x => x.Id);
        session.Property(x => x.Token).HasMaxLength(128).IsRequired();
        session.Property(x => x.CreatedAtUtc).IsRequired();
        session.Property(x => x.ExpiresAtUtc).IsRequired();
        session.Property(x => x.LastSeenAtUtc).IsRequired();
        session.HasIndex(x => x.Token).IsUnique();

        var character = modelBuilder.Entity<GatewayCharacterRecord>();
        character.ToTable("characters");
        character.HasKey(x => x.Id);
        character.Property(x => x.Name).HasMaxLength(24).IsRequired();
        character.Property(x => x.NormalizedName).HasMaxLength(24).IsRequired();
        character.Property(x => x.Class).HasMaxLength(24).IsRequired();
        character.Property(x => x.Gender).HasMaxLength(16).IsRequired();
        character.Property(x => x.Faction).HasMaxLength(16).IsRequired();
        character.Property(x => x.SceneName).HasMaxLength(64).IsRequired();
        character.Property(x => x.Level).IsRequired();
        character.Property(x => x.MapId).IsRequired();
        character.Property(x => x.PositionX).IsRequired();
        character.Property(x => x.PositionY).IsRequired();
        character.Property(x => x.CreatedAtUtc).IsRequired();
        character.Property(x => x.UpdatedAtUtc).IsRequired();

        session.HasOne(x => x.SelectedCharacter)
            .WithMany(x => x.Sessions)
            .HasForeignKey(x => x.SelectedCharacterId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
