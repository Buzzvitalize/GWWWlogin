namespace GWWWlogin.GatewayService.Definitions;

public sealed record MapDefinition(
    int Id,
    string SceneName,
    string TerrainFolder,
    string MonsterFolder,
    IReadOnlyList<NpcSpawn> Npcs,
    IReadOnlyList<MonsterSpawn> Monsters);
