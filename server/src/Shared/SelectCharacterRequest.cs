namespace GWWWlogin.Shared;

public sealed record SelectCharacterRequest(
    string SessionToken,
    Guid CharacterId);
