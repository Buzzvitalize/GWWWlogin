using GWWWlogin.LoginService.Models;
using Microsoft.EntityFrameworkCore;

namespace GWWWlogin.LoginService.Data;

public sealed class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts => Set<Account>();

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
    }
}
