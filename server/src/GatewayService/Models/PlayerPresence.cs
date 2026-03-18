namespace GWWWlogin.GatewayService.Models;

public sealed record PlayerPresence(
    Guid SessionId,
    Guid CharacterId,
    string CharacterName,
    string Faction,
    string SceneName,
    int MapId,
    float PositionX,
    float PositionY,
    int Level);
