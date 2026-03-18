namespace GWWWlogin.Shared;

public sealed record WorldBridgeMapSnapshot(
    int MapId,
    string SceneName,
    int ZoneSize,
    int ActiveNpcCount,
    int ActiveMonsterCount,
    long SimulationTick,
    DateTime LastSimulationAtUtc);
