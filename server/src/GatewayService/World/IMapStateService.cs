using GWWWlogin.GatewayService.Models;

namespace GWWWlogin.GatewayService.World;

public interface IMapStateService
{
    ActivePlayerState EnterMap(
        Guid sessionId,
        Guid accountId,
        Guid characterId,
        string characterName,
        int mapId,
        float positionX,
        float positionY,
        int level);

    ActivePlayerState? GetBySession(Guid sessionId);

    ActivePlayerState? Move(Guid sessionId, float positionX, float positionY);

    int GetPopulation(int mapId);
}
