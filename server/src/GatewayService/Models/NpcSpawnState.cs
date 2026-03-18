namespace GWWWlogin.GatewayService.Models;

public sealed record NpcSpawnState(
    string NpcKey,
    string DisplayName,
    string SceneName,
    int MapId,
    float PositionX,
    float PositionY);
