using Microsoft.Extensions.Options;

namespace GWWWlogin.GatewayService.Definitions;

public sealed class MapDefinitionService : IMapDefinitionService
{
    private readonly IReadOnlyList<MapDefinition> _maps;

    public MapDefinitionService(IOptions<MapCatalogOptions> optionsAccessor)
    {
        _maps = optionsAccessor.Value.Items
            .Where(x => x.MapId > 0 && !string.IsNullOrWhiteSpace(x.SceneName))
            .OrderBy(x => x.MapId)
            .Select(x => new MapDefinition(
                x.MapId,
                x.SceneName,
                x.TerrainFolder,
                x.MonsterFolder,
                x.Npcs.Select(npc => new NpcSpawn(npc.NpcKey, npc.DisplayName, npc.PositionX, npc.PositionY)).ToList(),
                x.Monsters.Select(monster => new MonsterSpawn(monster.MonsterKey, monster.DisplayName, monster.PositionX, monster.PositionY)).ToList()))
            .ToList();

        if (_maps.Count == 0)
        {
            _maps =
            [
                new MapDefinition(1001, "Athens_Newbie", "Gw Client/Map/TerrAthens_Newbie", "Gw Client/Localization/en_us/Monster/Athens_Newbie", [], []),
                new MapDefinition(1002, "Sparta_Newbie", "Gw Client/Map/TerrSparta_Newbie", "Gw Client/Localization/en_us/Monster/Sparta_Newbie", [], [])
            ];
        }
    }

    public MapDefinition? GetById(int id)
    {
        return _maps.SingleOrDefault(x => x.Id == id);
    }

    public IReadOnlyList<MapDefinition> GetAll()
    {
        return _maps;
    }
}
