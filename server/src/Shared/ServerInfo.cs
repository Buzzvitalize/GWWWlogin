namespace GWWWlogin.Shared;

public sealed record ServerInfo(
    int Id,
    string Name,
    string Host,
    int Port,
    string Status);
