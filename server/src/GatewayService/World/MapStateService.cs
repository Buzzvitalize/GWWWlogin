using System.Collections.Concurrent;
using GWWWlogin.GatewayService.Definitions;
using GWWWlogin.GatewayService.Models;

namespace GWWWlogin.GatewayService.World;

public sealed class MapStateService(IMapDefinitionService mapDefinitionService) : IMapStateService
{
    private const int ZoneSize = 200;

    private readonly ConcurrentDictionary<Guid, ActivePlayerState> _playersBySession = new();
    private readonly ConcurrentDictionary<int, IReadOnlyList<LiveNpcState>> _npcsByMap = new();
    private readonly ConcurrentDictionary<int, IReadOnlyList<LiveMonsterState>> _monstersByMap = new();
    private readonly ConcurrentDictionary<string, LiveNpcState> _npcsByInstanceId = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, LiveMonsterState> _monstersByInstanceId = new(StringComparer.OrdinalIgnoreCase);

    public ActivePlayerState EnterMap(
        Guid sessionId,
        Guid accountId,
        Guid characterId,
        string characterName,
        string faction,
        string sceneName,
        int mapId,
        float positionX,
        float positionY,
        int level)
    {
        EnsureMapEntitiesLoaded(mapId);

        var state = new ActivePlayerState
        {
            SessionId = sessionId,
            AccountId = accountId,
            CharacterId = characterId,
            CharacterName = characterName,
            Faction = faction,
            SceneName = sceneName,
            MapId = mapId,
            PositionX = positionX,
            PositionY = positionY,
            Level = level,
            EnteredMapAtUtc = DateTime.UtcNow
        };

        _playersBySession.AddOrUpdate(sessionId, state, (_, _) => state);
        return state;
    }

    public ActivePlayerState? GetBySession(Guid sessionId)
    {
        return _playersBySession.TryGetValue(sessionId, out var state)
            ? state
            : null;
    }

    public ActivePlayerState? Move(Guid sessionId, float positionX, float positionY)
    {
        if (!_playersBySession.TryGetValue(sessionId, out var state))
        {
            return null;
        }

        state.PositionX = positionX;
        state.PositionY = positionY;
        return state;
    }

    public ActivePlayerState? Travel(Guid sessionId, string sceneName, int mapId, float positionX, float positionY)
    {
        if (!_playersBySession.TryGetValue(sessionId, out var state))
        {
            return null;
        }

        EnsureMapEntitiesLoaded(mapId);

        state.SceneName = sceneName;
        state.MapId = mapId;
        state.PositionX = positionX;
        state.PositionY = positionY;
        state.EnteredMapAtUtc = DateTime.UtcNow;
        return state;
    }

    public ActivePlayerState? Leave(Guid sessionId)
    {
        return _playersBySession.TryRemove(sessionId, out var state)
            ? state
            : null;
    }

    public IReadOnlyList<PlayerPresence> GetPlayers(int mapId)
    {
        return _playersBySession.Values
            .Where(x => x.MapId == mapId)
            .Select(ToPlayerPresence)
            .ToList();
    }

    public IReadOnlyList<PlayerPresence> GetPlayersInRange(int mapId, float originX, float originY, float radius)
    {
        return _playersBySession.Values
            .Where(x => x.MapId == mapId && IsWithinRange(originX, originY, x.PositionX, x.PositionY, radius))
            .Select(ToPlayerPresence)
            .ToList();
    }

    public IReadOnlyList<NpcSpawnState> GetNpcs(int mapId)
    {
        EnsureMapEntitiesLoaded(mapId);
        return _npcsByMap.TryGetValue(mapId, out var npcs)
            ? npcs.Select(ToNpcSnapshot).ToList()
            : [];
    }

    public IReadOnlyList<NpcSpawnState> GetNpcsInRange(int mapId, float originX, float originY, float radius)
    {
        EnsureMapEntitiesLoaded(mapId);
        return _npcsByMap.TryGetValue(mapId, out var npcs)
            ? npcs
                .Where(x => IsWithinRange(originX, originY, x.PositionX, x.PositionY, radius))
                .Select(ToNpcSnapshot)
                .ToList()
            : [];
    }

    public IReadOnlyList<MonsterSpawnState> GetMonsters(int mapId)
    {
        EnsureMapEntitiesLoaded(mapId);
        return _monstersByMap.TryGetValue(mapId, out var monsters)
            ? monsters.Select(ToMonsterSnapshot).ToList()
            : [];
    }

    public IReadOnlyList<MonsterSpawnState> GetMonstersInRange(int mapId, float originX, float originY, float radius)
    {
        EnsureMapEntitiesLoaded(mapId);
        return _monstersByMap.TryGetValue(mapId, out var monsters)
            ? monsters
                .Where(x => IsAliveAndWithinRange(x, originX, originY, radius))
                .Select(ToMonsterSnapshot)
                .ToList()
            : [];
    }

    public MonsterSpawnState? UpdateMonsterPosition(string instanceId, float positionX, float positionY)
    {
        if (!_monstersByInstanceId.TryGetValue(instanceId, out var monster))
        {
            return null;
        }

        monster.PositionX = positionX;
        monster.PositionY = positionY;
        monster.ZoneKey = BuildZoneKey(positionX, positionY);
        monster.LastUpdatedAtUtc = DateTime.UtcNow;
        return ToMonsterSnapshot(monster);
    }

    public NpcSpawnState? UpdateNpcPosition(string instanceId, float positionX, float positionY)
    {
        if (!_npcsByInstanceId.TryGetValue(instanceId, out var npc))
        {
            return null;
        }

        npc.PositionX = positionX;
        npc.PositionY = positionY;
        npc.ZoneKey = BuildZoneKey(positionX, positionY);
        npc.LastUpdatedAtUtc = DateTime.UtcNow;
        return ToNpcSnapshot(npc);
    }

    public int GetPopulation(int mapId)
    {
        return _playersBySession.Values.Count(x => x.MapId == mapId);
    }

    private void EnsureMapEntitiesLoaded(int mapId)
    {
        _npcsByMap.GetOrAdd(mapId, _ => BuildLiveNpcs(mapId));
        _monstersByMap.GetOrAdd(mapId, _ => BuildLiveMonsters(mapId));
    }

    private IReadOnlyList<LiveNpcState> BuildLiveNpcs(int mapId)
    {
        var definition = mapDefinitionService.GetById(mapId);
        if (definition is null)
        {
            return [];
        }

        var loadedAtUtc = DateTime.UtcNow;
        var npcs = definition.Npcs
            .Select((spawn, index) => new LiveNpcState
            {
                InstanceId = $"npc:{mapId}:{spawn.NpcKey}:{index}",
                NpcKey = spawn.NpcKey,
                DisplayName = spawn.DisplayName,
                SceneName = definition.SceneName,
                MapId = mapId,
                PositionX = spawn.PositionX,
                PositionY = spawn.PositionY,
                ZoneKey = BuildZoneKey(spawn.PositionX, spawn.PositionY),
                LastUpdatedAtUtc = loadedAtUtc
            })
            .ToList();

        foreach (var npc in npcs)
        {
            _npcsByInstanceId[npc.InstanceId] = npc;
        }

        return npcs;
    }

    private IReadOnlyList<LiveMonsterState> BuildLiveMonsters(int mapId)
    {
        var definition = mapDefinitionService.GetById(mapId);
        if (definition is null)
        {
            return [];
        }

        var loadedAtUtc = DateTime.UtcNow;
        var monsters = definition.Monsters
            .Select((spawn, index) => new LiveMonsterState
            {
                InstanceId = $"monster:{mapId}:{spawn.MonsterKey}:{index}",
                MonsterKey = spawn.MonsterKey,
                DisplayName = spawn.DisplayName,
                SceneName = definition.SceneName,
                MapId = mapId,
                PositionX = spawn.PositionX,
                PositionY = spawn.PositionY,
                ZoneKey = BuildZoneKey(spawn.PositionX, spawn.PositionY),
                IsAlive = true,
                LastUpdatedAtUtc = loadedAtUtc
            })
            .ToList();

        foreach (var monster in monsters)
        {
            _monstersByInstanceId[monster.InstanceId] = monster;
        }

        return monsters;
    }

    private static bool IsAliveAndWithinRange(LiveMonsterState monster, float originX, float originY, float radius)
    {
        return monster.IsAlive && IsWithinRange(originX, originY, monster.PositionX, monster.PositionY, radius);
    }

    private static PlayerPresence ToPlayerPresence(ActivePlayerState state)
    {
        return new PlayerPresence(
            state.SessionId,
            state.CharacterId,
            state.CharacterName,
            state.Faction,
            state.SceneName,
            state.MapId,
            state.PositionX,
            state.PositionY,
            state.Level);
    }

    private static NpcSpawnState ToNpcSnapshot(LiveNpcState npc)
    {
        return new NpcSpawnState(
            npc.InstanceId,
            npc.NpcKey,
            npc.DisplayName,
            npc.SceneName,
            npc.MapId,
            npc.PositionX,
            npc.PositionY,
            npc.ZoneKey,
            npc.LastUpdatedAtUtc);
    }

    private static MonsterSpawnState ToMonsterSnapshot(LiveMonsterState monster)
    {
        return new MonsterSpawnState(
            monster.InstanceId,
            monster.MonsterKey,
            monster.DisplayName,
            monster.SceneName,
            monster.MapId,
            monster.PositionX,
            monster.PositionY,
            monster.ZoneKey,
            monster.IsAlive,
            monster.LastUpdatedAtUtc);
    }

    private static bool IsWithinRange(float originX, float originY, float targetX, float targetY, float radius)
    {
        var deltaX = targetX - originX;
        var deltaY = targetY - originY;
        var distanceSquared = (deltaX * deltaX) + (deltaY * deltaY);
        var radiusSquared = radius * radius;
        return distanceSquared <= radiusSquared;
    }

    private static string BuildZoneKey(float positionX, float positionY)
    {
        var zoneX = (int)MathF.Floor(positionX / ZoneSize);
        var zoneY = (int)MathF.Floor(positionY / ZoneSize);
        return $"zone:{zoneX}:{zoneY}";
    }
}
