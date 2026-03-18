namespace GWWWlogin.Shared;

public sealed record LoginResponse(
    bool Success,
    string Message,
    string? SessionToken,
    IReadOnlyList<ServerInfo> Servers);
