namespace GWWWlogin.Shared;

public sealed record RegisterAccountRequest(
    string Username,
    string Password,
    string Email);
