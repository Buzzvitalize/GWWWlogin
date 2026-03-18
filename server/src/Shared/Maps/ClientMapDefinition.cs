namespace GWWWlogin.Shared.Maps;

public sealed record ClientMapDefinition(
    int MapId,
    string SceneName,
    string TerrainFolder,
    string MonsterFolder,
    float DefaultSpawnX,
    float DefaultSpawnY,
    IReadOnlyList<ClientMapAddress> Addresses,
    IReadOnlyList<ClientMonsterTemplate> Monsters);
