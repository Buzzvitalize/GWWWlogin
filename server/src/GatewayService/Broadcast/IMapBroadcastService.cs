using GWWWlogin.GatewayService.Models;

namespace GWWWlogin.GatewayService.Broadcast;

public interface IMapBroadcastService
{
    MapBroadcastEvent Publish(int mapId, MapBroadcastEvent mapEvent);

    IReadOnlyList<MapBroadcastEvent> ReadSince(int mapId, long afterSequenceId, int take);

}
