namespace GWWWlogin.Shared;

public sealed record GatewayTicketResponse(
    Guid SessionId,
    Guid AccountId,
    Guid CharacterId,
    string CharacterName,
    string GatewayHost,
    int GatewayPort,
    int MapId,
    float PositionX,
    float PositionY,
    DateTime SessionExpiresAtUtc,
    int MaxLevel);
