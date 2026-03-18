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

    ActivePlayerState? Leave(Guid sessionId);

    IReadOnlyList<PlayerPresence> GetPlayers(int mapId);

    IReadOnlyList<NpcSpawnState> GetNpcs(int mapId);

    IReadOnlyList<MonsterSpawnState> GetMonsters(int mapId);

    int GetPopulation(int mapId);
}
