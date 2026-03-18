namespace GWWWlogin.GatewayService.Models;

public sealed record MonsterSpawnState(
    string MonsterKey,
    string DisplayName,
    string SceneName,
    int MapId,
    float PositionX,
    float PositionY);
