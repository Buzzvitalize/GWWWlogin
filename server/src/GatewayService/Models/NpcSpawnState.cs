namespace GWWWlogin.GatewayService.Models;

public sealed record NpcSpawnState(
    string InstanceId,
    string NpcKey,
    string DisplayName,
    string SceneName,
    int MapId,
    float PositionX,
    float PositionY,
    string ZoneKey,
    DateTime LastUpdatedAtUtc);
