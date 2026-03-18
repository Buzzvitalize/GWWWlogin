namespace GWWWlogin.GameServer.World;

public sealed record GameMapRuntimeState(
    int MapId,
    string SceneName,
    int ZoneSize,
    int ActiveNpcCount,
    int ActiveMonsterCount,
    long SimulationTick,
    DateTime LastSimulationAtUtc);
