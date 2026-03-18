namespace GWWWlogin.Shared;

public sealed record CharacterSelectionResponse(
    Guid SessionId,
    Guid AccountId,
    Guid CharacterId,
    string CharacterName,
    int MapId,
    float PositionX,
    float PositionY,
    int Level,
    DateTime SessionExpiresAtUtc,
    bool ReadyForGateway);
