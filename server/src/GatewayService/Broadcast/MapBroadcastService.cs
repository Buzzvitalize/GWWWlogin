using System.Collections.Concurrent;
using GWWWlogin.GatewayService.Models;

namespace GWWWlogin.GatewayService.Broadcast;

public sealed class MapBroadcastService : IMapBroadcastService
{
    private readonly ConcurrentDictionary<int, ConcurrentQueue<MapBroadcastEvent>> _eventsByMap = new();
    private readonly ConcurrentDictionary<int, long> _sequenceByMap = new();

    public MapBroadcastEvent Publish(int mapId, MapBroadcastEvent mapEvent)
    {
        var queue = _eventsByMap.GetOrAdd(mapId, _ => new ConcurrentQueue<MapBroadcastEvent>());
        var sequenceId = _sequenceByMap.AddOrUpdate(mapId, 1, (_, current) => current + 1);
        var sequencedEvent = mapEvent with { SequenceId = sequenceId };

        queue.Enqueue(sequencedEvent);

        while (queue.Count > 100 && queue.TryDequeue(out _))
        {
        }

        return sequencedEvent;
    }

    public IReadOnlyList<MapBroadcastEvent> ReadSince(int mapId, long afterSequenceId, int take)
    {
        if (!_eventsByMap.TryGetValue(mapId, out var queue))
        {
            return [];
        }

        return queue
            .ToArray()
            .Where(x => x.SequenceId > afterSequenceId)
            .OrderBy(x => x.SequenceId)
            .Take(take)
            .ToList();
    }

}
