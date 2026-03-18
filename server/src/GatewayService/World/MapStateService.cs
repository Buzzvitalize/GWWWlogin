using System.Collections.Concurrent;
using GWWWlogin.GatewayService.Definitions;
using GWWWlogin.GatewayService.Models;

namespace GWWWlogin.GatewayService.World;

public sealed class MapStateService(IMapDefinitionService mapDefinitionService) : IMapStateService
{
    private readonly ConcurrentDictionary<Guid, ActivePlayerState> _playersBySession = new();

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
            .Select(x => new PlayerPresence(
                x.SessionId,
                x.CharacterId,
                x.CharacterName,
                x.Faction,
                x.SceneName,
                x.MapId,
                x.PositionX,
                x.PositionY,
                x.Level))
            .ToList();
    }

    public IReadOnlyList<NpcSpawnState> GetNpcs(int mapId)
    {
        var definition = mapDefinitionService.GetById(mapId);
        return definition is null
            ? []
            : definition.Npcs.Select(x => new NpcSpawnState(x.NpcKey, x.DisplayName, definition.SceneName, mapId, x.PositionX, x.PositionY)).ToList();
    }

    public IReadOnlyList<MonsterSpawnState> GetMonsters(int mapId)
    {
        var definition = mapDefinitionService.GetById(mapId);
        return definition is null
            ? []
            : definition.Monsters.Select(x => new MonsterSpawnState(x.MonsterKey, x.DisplayName, definition.SceneName, mapId, x.PositionX, x.PositionY)).ToList();
    }

    public int GetPopulation(int mapId)
    {
        return _playersBySession.Values.Count(x => x.MapId == mapId);
    }
}
