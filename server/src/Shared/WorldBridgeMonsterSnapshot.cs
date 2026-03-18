namespace GWWWlogin.Shared;

public sealed record WorldBridgeMonsterSnapshot(
    string InstanceId,
    int MapId,
    string SceneName,
    string DisplayName,
    float PositionX,
    float PositionY,
    string ZoneKey,
    bool IsAlive,
    DateTime LastUpdatedAtUtc);
