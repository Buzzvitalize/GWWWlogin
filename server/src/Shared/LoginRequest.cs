namespace GWWWlogin.Shared;

public sealed record LoginRequest(
    string Username,
    string Password);
