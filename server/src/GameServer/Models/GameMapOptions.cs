namespace GWWWlogin.GameServer.Models;

public sealed class GameMapOptions
{
    public int MapId { get; set; }

    public string SceneName { get; set; } = string.Empty;

    public string TerrainFolder { get; set; } = string.Empty;

    public string MonsterFolder { get; set; } = string.Empty;

    public int ZoneSize { get; set; } = 200;

    public List<GameMonsterSeedOptions> Monsters { get; set; } = [];
}
