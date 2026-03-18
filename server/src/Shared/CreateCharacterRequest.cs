namespace GWWWlogin.Shared;

public sealed record CreateCharacterRequest(
    Guid AccountId,
    string Name,
    string Class,
    string Gender);
