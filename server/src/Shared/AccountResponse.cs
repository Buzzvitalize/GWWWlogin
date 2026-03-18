namespace GWWWlogin.Shared;

public sealed record AccountResponse(
    Guid Id,
    string Username,
    string Email,
    DateTime CreatedAtUtc);
