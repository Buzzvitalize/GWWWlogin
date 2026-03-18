using GWWWlogin.GatewayService.Models;

namespace GWWWlogin.GatewayService.Broadcast;

public interface IMapBroadcastService
{
    void Publish(int mapId, MapBroadcastEvent mapEvent);

    IReadOnlyList<MapBroadcastEvent> Read(int mapId, int take);
}
