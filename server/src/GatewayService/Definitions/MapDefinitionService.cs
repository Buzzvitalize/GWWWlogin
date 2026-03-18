using GWWWlogin.Shared.Maps;

namespace GWWWlogin.GatewayService.Definitions;

public sealed class MapDefinitionService(IClientMapCatalog mapCatalog) : IMapDefinitionService
{
    private readonly IReadOnlyList<MapDefinition> _maps = mapCatalog.GetAll()
        .Select(map => new MapDefinition(
            map.MapId,
            map.SceneName,
            map.TerrainFolder,
            map.MonsterFolder,
            [],
            map.Monsters
                .Select((monster, index) => new MonsterSpawn(
                    monster.MonsterKey,
                    monster.DisplayName,
                    map.DefaultSpawnX + ((index % 4) * 18f),
                    map.DefaultSpawnY + ((index / 4) * 18f)))
                .Take(8)
                .ToList()))
        .ToList();

    public MapDefinition? GetById(int id)
    {
        return _maps.SingleOrDefault(x => x.Id == id);
    }

    public IReadOnlyList<MapDefinition> GetAll()
    {
        return _maps;
    }
}
