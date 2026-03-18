using System.Collections.Concurrent;
using GWWWlogin.Shared;

namespace GWWWlogin.GameServer.World;

public sealed class GameWorldService : IGameWorldService
{
    private const int ZoneSize = 200;
    private const float StepDistance = 12f;

    private readonly ConcurrentDictionary<int, GameMapRuntimeState> _maps = new(
        [
            new KeyValuePair<int, GameMapRuntimeState>(1001, new GameMapRuntimeState(1001, "Athens_Newbie", ZoneSize, 3, 1, 0, DateTime.UtcNow)),
            new KeyValuePair<int, GameMapRuntimeState>(1002, new GameMapRuntimeState(1002, "Sparta_Newbie", ZoneSize, 3, 1, 0, DateTime.UtcNow))
        ]);

    private readonly ConcurrentDictionary<string, GameMonsterRuntimeState> _monstersByInstanceId = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<int, IReadOnlyList<GameMonsterRuntimeState>> _monstersByMap = new();
    private readonly ConcurrentQueue<WorldBridgeEntityUpdate> _recentUpdates = new();
    private long _updateSequence;

    public GameWorldService()
    {
        SeedMonsters();
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
                monster.ZoneKey = BuildZoneKey(monster.PositionX, monster.PositionY);
                monster.LastUpdatedAtUtc = DateTime.UtcNow;

                PublishUpdate(map.MapId, "ENTITY_MOVE", monster);
            }
        }
    }

    private void SeedMonsters()
    {
        SeedMonster(1001, "Athens_Newbie", "monster:1001:A_normal_PersianWarriorst_005:0", "Persian Warrior", 420, 315);
        SeedMonster(1002, "Sparta_Newbie", "monster:1002:A_normal_PersianWarriorcd_004:0", "Persian Warrior", 430, 320);
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
            ZoneKey = BuildZoneKey(positionX, positionY),
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

    private static string BuildZoneKey(float positionX, float positionY)
    {
        var zoneX = (int)MathF.Floor(positionX / ZoneSize);
        var zoneY = (int)MathF.Floor(positionY / ZoneSize);
        return $"zone:{zoneX}:{zoneY}";
    }
}
