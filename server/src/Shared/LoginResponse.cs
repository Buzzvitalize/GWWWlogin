namespace GWWWlogin.Shared;

public sealed record LoginResponse(
    bool Success,
    string Message,
    Guid? AccountId,
    string? SessionToken,
    DateTime? SessionExpiresAtUtc,
    IReadOnlyList<ServerInfo> Servers);
