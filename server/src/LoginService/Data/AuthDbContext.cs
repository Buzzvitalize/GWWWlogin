using GWWWlogin.LoginService.Models;
using Microsoft.EntityFrameworkCore;

namespace GWWWlogin.LoginService.Data;

public sealed class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts => Set<Account>();

    public DbSet<Character> Characters => Set<Character>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var account = modelBuilder.Entity<Account>();

        account.ToTable("accounts");
        account.HasKey(x => x.Id);

        account.Property(x => x.Username)
            .HasMaxLength(32)
            .IsRequired();

        account.Property(x => x.NormalizedUsername)
            .HasMaxLength(32)
            .IsRequired();

        account.Property(x => x.Email)
            .HasMaxLength(320)
            .IsRequired();

        account.Property(x => x.NormalizedEmail)
            .HasMaxLength(320)
            .IsRequired();

        account.Property(x => x.PasswordHash)
            .HasMaxLength(512)
            .IsRequired();

        account.Property(x => x.CreatedAtUtc)
            .IsRequired();

        account.Property(x => x.UpdatedAtUtc)
            .IsRequired();

        account.HasIndex(x => x.NormalizedUsername)
            .IsUnique();

        account.HasIndex(x => x.NormalizedEmail)
            .IsUnique();

        var character = modelBuilder.Entity<Character>();

        character.ToTable("characters");
        character.HasKey(x => x.Id);

        character.Property(x => x.Name)
            .HasMaxLength(24)
            .IsRequired();

        character.Property(x => x.NormalizedName)
            .HasMaxLength(24)
            .IsRequired();

        character.Property(x => x.Class)
            .HasMaxLength(24)
            .IsRequired();

        character.Property(x => x.Gender)
            .HasMaxLength(16)
            .IsRequired();

        character.Property(x => x.Level)
            .IsRequired();

        character.Property(x => x.MapId)
            .IsRequired();

        character.Property(x => x.PositionX)
            .IsRequired();

        character.Property(x => x.PositionY)
            .IsRequired();

        character.Property(x => x.CreatedAtUtc)
            .IsRequired();

        character.Property(x => x.UpdatedAtUtc)
            .IsRequired();

        character.HasIndex(x => x.NormalizedName)
            .IsUnique();

        character.HasOne(x => x.Account)
            .WithMany(x => x.Characters)
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
