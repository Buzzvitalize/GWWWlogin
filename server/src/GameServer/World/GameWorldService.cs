using System.Collections.Concurrent;
using GWWWlogin.GameServer.Models;
using GWWWlogin.Shared;
using Microsoft.Extensions.Options;

namespace GWWWlogin.GameServer.World;

public sealed class GameWorldService : IGameWorldService
{
    private const float StepDistance = 12f;

    private readonly ConcurrentDictionary<int, GameMapRuntimeState> _maps;
    private readonly ConcurrentDictionary<string, GameMonsterRuntimeState> _monstersByInstanceId = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<int, IReadOnlyList<GameMonsterRuntimeState>> _monstersByMap = new();
    private readonly ConcurrentQueue<WorldBridgeEntityUpdate> _recentUpdates = new();
    private readonly IReadOnlyDictionary<int, int> _zoneSizeByMap;
    private long _updateSequence;

    public GameWorldService(IOptions<GameServerOptions> optionsAccessor)
    {
        var options = optionsAccessor.Value;
        var configuredMaps = BuildConfiguredMaps(options);

        _maps = new ConcurrentDictionary<int, GameMapRuntimeState>(configuredMaps.ToDictionary(x => x.MapId, x => x.RuntimeState));
        _zoneSizeByMap = configuredMaps.ToDictionary(x => x.MapId, x => x.ZoneSize);

        SeedMonsters(configuredMaps);
    }

    public IReadOnlyList<WorldBridgeMapSnapshot> GetMaps()
    {
        return _maps.Values
            .OrderBy(x => x.MapId)
            .Select(x => new WorldBridgeMapSnapshot(x.MapId, x.SceneName, x.ZoneSize, x.ActiveNpcCount, x.ActiveMonsterCount, x.SimulationTick, x.LastSimulationAtUtc))
            .ToList();
    }

    public IReadOnlyList<WorldBridgeMonsterSnapshot> GetMonsters(int? mapId = null)
    {
        var monsters = mapId.HasValue && _monstersByMap.TryGetValue(mapId.Value, out var byMap)
            ? byMap
            : _monstersByInstanceId.Values.ToList();

        return monsters
            .OrderBy(x => x.MapId)
            .ThenBy(x => x.InstanceId)
            .Select(ToMonsterSnapshot)
            .ToList();
    }

    public IReadOnlyList<WorldBridgeEntityUpdate> GetRecentUpdates(long afterSequenceId, int take)
    {
        return _recentUpdates
            .ToArray()
            .Where(x => x.SequenceId > afterSequenceId)
            .OrderBy(x => x.SequenceId)
            .Take(take)
            .ToList();
    }

    public void AdvanceSimulationTick()
    {
        foreach (var map in _maps.Values.OrderBy(x => x.MapId).ToList())
        {
            var nextTick = map.SimulationTick + 1;
            _maps[map.MapId] = map with
            {
                SimulationTick = nextTick,
                LastSimulationAtUtc = DateTime.UtcNow
            };

            if (!_monstersByMap.TryGetValue(map.MapId, out var monsters))
            {
                continue;
            }

            foreach (var monster in monsters.Where(x => x.IsAlive))
            {
                var direction = nextTick % 2 == 0 ? -1f : 1f;
                monster.PositionX += StepDistance * direction;
                monster.ZoneKey = BuildZoneKey(map.MapId, monster.PositionX, monster.PositionY);
                monster.LastUpdatedAtUtc = DateTime.UtcNow;

                PublishUpdate(map.MapId, "ENTITY_MOVE", monster);
            }
        }
    }

    private static IReadOnlyList<ConfiguredMapState> BuildConfiguredMaps(GameServerOptions options)
    {
        var maps = options.Maps
            .Where(x => x.MapId > 0 && !string.IsNullOrWhiteSpace(x.SceneName))
            .Select(x => new ConfiguredMapState(
                x.MapId,
                x.ZoneSize > 0 ? x.ZoneSize : options.ZoneSize,
                x.Monsters ?? [],
                new GameMapRuntimeState(
                    x.MapId,
                    x.SceneName,
                    x.ZoneSize > 0 ? x.ZoneSize : options.ZoneSize,
                    0,
                    x.Monsters?.Count ?? 0,
                    0,
                    DateTime.UtcNow)))
            .OrderBy(x => x.MapId)
            .ToList();

        if (maps.Count > 0)
        {
            return maps;
        }

        return
        [
            new ConfiguredMapState(1001, options.ZoneSize, [new GameMonsterSeedOptions { MonsterKey = "A_normal_PersianWarriorst_005", DisplayName = "Persian Warrior", PositionX = 420, PositionY = 315 }], new GameMapRuntimeState(1001, "Athens_Newbie", options.ZoneSize, 0, 1, 0, DateTime.UtcNow)),
            new ConfiguredMapState(1002, options.ZoneSize, [new GameMonsterSeedOptions { MonsterKey = "A_normal_PersianWarriorcd_004", DisplayName = "Persian Warrior", PositionX = 430, PositionY = 320 }], new GameMapRuntimeState(1002, "Sparta_Newbie", options.ZoneSize, 0, 1, 0, DateTime.UtcNow))
        ];
    }

    private void SeedMonsters(IReadOnlyList<ConfiguredMapState> maps)
    {
        foreach (var map in maps)
        {
            foreach (var monster in map.Monsters.Select((monster, index) => (monster, index)))
            {
                SeedMonster(map.MapId, _maps[map.MapId].SceneName, $"monster:{map.MapId}:{monster.monster.MonsterKey}:{monster.index}", monster.monster.DisplayName, monster.monster.PositionX, monster.monster.PositionY);
            }
        }
    }

    private void SeedMonster(int mapId, string sceneName, string instanceId, string displayName, float positionX, float positionY)
    {
        var monster = new GameMonsterRuntimeState
        {
            InstanceId = instanceId,
            MapId = mapId,
            SceneName = sceneName,
            DisplayName = displayName,
            PositionX = positionX,
            PositionY = positionY,
            ZoneKey = BuildZoneKey(mapId, positionX, positionY),
            IsAlive = true,
            LastUpdatedAtUtc = DateTime.UtcNow
        };

        _monstersByInstanceId[monster.InstanceId] = monster;
        _monstersByMap.AddOrUpdate(
            mapId,
            _ => new List<GameMonsterRuntimeState> { monster },
            (_, existing) => existing.Concat([monster]).ToList());
    }

    private void PublishUpdate(int mapId, string eventType, GameMonsterRuntimeState monster)
    {
        var sequenceId = Interlocked.Increment(ref _updateSequence);
        _recentUpdates.Enqueue(new WorldBridgeEntityUpdate(
            sequenceId,
            mapId,
            eventType,
            "monster",
            monster.InstanceId,
            monster.DisplayName,
            monster.PositionX,
            monster.PositionY,
            monster.ZoneKey,
            DateTime.UtcNow));

        while (_recentUpdates.Count > 200 && _recentUpdates.TryDequeue(out _))
        {
        }
    }

    private static WorldBridgeMonsterSnapshot ToMonsterSnapshot(GameMonsterRuntimeState monster)
    {
        return new WorldBridgeMonsterSnapshot(
            monster.InstanceId,
            monster.MapId,
            monster.SceneName,
            monster.DisplayName,
            monster.PositionX,
            monster.PositionY,
            monster.ZoneKey,
            monster.IsAlive,
            monster.LastUpdatedAtUtc);
    }

    private string BuildZoneKey(int mapId, float positionX, float positionY)
    {
        var zoneSize = _zoneSizeByMap.TryGetValue(mapId, out var configuredZoneSize) ? configuredZoneSize : 200;
        var zoneX = (int)MathF.Floor(positionX / zoneSize);
        var zoneY = (int)MathF.Floor(positionY / zoneSize);
        return $"zone:{zoneX}:{zoneY}";
    }

    private sealed record ConfiguredMapState(int MapId, int ZoneSize, IReadOnlyList<GameMonsterSeedOptions> Monsters, GameMapRuntimeState RuntimeState);
}
