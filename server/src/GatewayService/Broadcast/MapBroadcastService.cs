using System.Collections.Concurrent;
using GWWWlogin.GatewayService.Models;

namespace GWWWlogin.GatewayService.Broadcast;

public sealed class MapBroadcastService : IMapBroadcastService
{
    private readonly ConcurrentDictionary<int, ConcurrentQueue<MapBroadcastEvent>> _eventsByMap = new();

    public void Publish(int mapId, MapBroadcastEvent mapEvent)
    {
        var queue = _eventsByMap.GetOrAdd(mapId, _ => new ConcurrentQueue<MapBroadcastEvent>());
        queue.Enqueue(mapEvent);

        while (queue.Count > 100 && queue.TryDequeue(out _))
        {
        }
    }

    public IReadOnlyList<MapBroadcastEvent> Read(int mapId, int take)
    {
        if (!_eventsByMap.TryGetValue(mapId, out var queue))
        {
            return [];
        }

        return queue.Reverse().Take(take).Reverse().ToList();
    }
}
