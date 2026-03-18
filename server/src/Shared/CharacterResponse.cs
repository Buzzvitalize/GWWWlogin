namespace GWWWlogin.Shared;

public sealed record CharacterResponse(
    Guid Id,
    Guid AccountId,
    string Name,
    string Class,
    string Gender,
    int Level,
    int MapId,
    float PositionX,
    float PositionY,
    DateTime CreatedAtUtc);
