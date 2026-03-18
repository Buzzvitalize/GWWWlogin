using GWWWlogin.GatewayService.Models;

namespace GWWWlogin.GatewayService.World;

public interface IMapStateService
{
    ActivePlayerState EnterMap(
        Guid sessionId,
        Guid accountId,
        Guid characterId,
        string characterName,
        string faction,
        string sceneName,
        int mapId,
        float positionX,
        float positionY,
        int level);

    ActivePlayerState? GetBySession(Guid sessionId);

    ActivePlayerState? Move(Guid sessionId, float positionX, float positionY);

    ActivePlayerState? Travel(Guid sessionId, string sceneName, int mapId, float positionX, float positionY);

    ActivePlayerState? Leave(Guid sessionId);

    IReadOnlyList<PlayerPresence> GetPlayers(int mapId);

    IReadOnlyList<PlayerPresence> GetPlayersInRange(int mapId, float originX, float originY, float radius);

    IReadOnlyList<NpcSpawnState> GetNpcs(int mapId);

    IReadOnlyList<NpcSpawnState> GetNpcsInRange(int mapId, float originX, float originY, float radius);

    IReadOnlyList<MonsterSpawnState> GetMonsters(int mapId);

    IReadOnlyList<MonsterSpawnState> GetMonstersInRange(int mapId, float originX, float originY, float radius);

    MonsterSpawnState? UpdateMonsterPosition(string instanceId, float positionX, float positionY);

    NpcSpawnState? UpdateNpcPosition(string instanceId, float positionX, float positionY);

    int GetPopulation(int mapId);
}
