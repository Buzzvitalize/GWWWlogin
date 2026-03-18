namespace GWWWlogin.GatewayService.Definitions;

public sealed class MapDefinitionService : IMapDefinitionService
{
    private static readonly IReadOnlyList<MapDefinition> Maps =
    [
        new(
            1001,
            "Athens_Newbie",
            "Gw Client/Map/TerrAthens_Newbie",
            "Gw Client/Localization/en_us/Monster/Athens_Newbie",
            [
                new NpcSpawn("Athens_011", "[Priest]Saizeriya", 120, 180),
                new NpcSpawn("Athens_026", "[Warrior]Demetrius", 145, 210),
                new NpcSpawn("Athens_036", "[Skill]Simonides", 160, 240)
            ],
            [
                new MonsterSpawn("A_normal_PersianWarriorst_005", "Persian Warrior", 420, 315)
            ]),
        new(
            1002,
            "Sparta_Newbie",
            "Gw Client/Map/TerrSparta_Newbie",
            "Gw Client/Localization/en_us/Monster/Sparta_Newbie",
            [
                new NpcSpawn("Sparta_011", "[Priest]Diotima", 118, 178),
                new NpcSpawn("Sparta_028", "[Warrior]Patroklos", 142, 208),
                new NpcSpawn("Sparta_096", "[Warrior Supplier]Simonos", 166, 240)
            ],
            [
                new MonsterSpawn("A_normal_PersianWarriorcd_004", "Persian Warrior", 430, 320)
            ])
    ];

    public MapDefinition? GetById(int id)
    {
        return Maps.SingleOrDefault(x => x.Id == id);
    }

    public IReadOnlyList<MapDefinition> GetAll()
    {
        return Maps;
    }
}
