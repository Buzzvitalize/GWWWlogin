namespace GWWWlogin.GatewayService.Definitions;

public sealed class MapCatalogOptions
{
    public const string SectionName = "Maps";

    public List<MapCatalogEntryOptions> Items { get; set; } = [];
}

public sealed class MapCatalogEntryOptions
{
    public int MapId { get; set; }

    public string SceneName { get; set; } = string.Empty;

    public string TerrainFolder { get; set; } = string.Empty;

    public string MonsterFolder { get; set; } = string.Empty;

    public List<NpcSpawnOptions> Npcs { get; set; } = [];

    public List<MonsterSpawnOptions> Monsters { get; set; } = [];
}

public sealed class NpcSpawnOptions
{
    public string NpcKey { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public float PositionX { get; set; }

    public float PositionY { get; set; }
}

public sealed class MonsterSpawnOptions
{
    public string MonsterKey { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public float PositionX { get; set; }

    public float PositionY { get; set; }
}
