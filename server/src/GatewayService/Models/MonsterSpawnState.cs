namespace GWWWlogin.GatewayService.Models;

public sealed record MonsterSpawnState(
    string InstanceId,
    string MonsterKey,
    string DisplayName,
    string SceneName,
    int MapId,
    float PositionX,
    float PositionY,
    string ZoneKey,
    bool IsAlive,
    DateTime LastUpdatedAtUtc);
