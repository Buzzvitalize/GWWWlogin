using GWWWlogin.LoginService.Data;
using GWWWlogin.LoginService.Models;
using GWWWlogin.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GWWWlogin.LoginService.Services;

public sealed class AccountService(AuthDbContext dbContext) : IAccountService
{
    private readonly PasswordHasher<Account> _passwordHasher = new();

    public async Task<AccountResponse> RegisterAsync(RegisterAccountRequest request, CancellationToken cancellationToken)
    {
        var username = request.Username.Trim();
        var normalizedUsername = username.ToUpperInvariant();
        var email = request.Email.Trim();
        var normalizedEmail = email.ToUpperInvariant();

        if (await dbContext.Accounts.AnyAsync(x => x.NormalizedUsername == normalizedUsername, cancellationToken))
        {
            throw new InvalidOperationException("Username already exists.");
        }

        if (await dbContext.Accounts.AnyAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken))
        {
            throw new InvalidOperationException("Email already exists.");
        }

        var now = DateTime.UtcNow;
        var account = new Account
        {
            Id = Guid.NewGuid(),
            Username = username,
            NormalizedUsername = normalizedUsername,
            Email = email,
            NormalizedEmail = normalizedEmail,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        account.PasswordHash = _passwordHasher.HashPassword(account, request.Password);

        dbContext.Accounts.Add(account);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AccountResponse(
            account.Id,
            account.Username,
            account.Email,
            account.CreatedAtUtc);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, IReadOnlyList<ServerInfo> servers, CancellationToken cancellationToken)
    {
        var username = request.Username.Trim();
        var normalizedUsername = username.ToUpperInvariant();

        var account = await dbContext.Accounts
            .SingleOrDefaultAsync(x => x.NormalizedUsername == normalizedUsername, cancellationToken);

        if (account is null)
        {
            return new LoginResponse(false, "Invalid username or password.", null, Array.Empty<ServerInfo>());
        }

        var verification = _passwordHasher.VerifyHashedPassword(account, account.PasswordHash, request.Password);
        if (verification == PasswordVerificationResult.Failed)
        {
            return new LoginResponse(false, "Invalid username or password.", null, Array.Empty<ServerInfo>());
        }

        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        account.UpdatedAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        return new LoginResponse(true, $"Welcome, {account.Username}.", token, servers);
    }
}
