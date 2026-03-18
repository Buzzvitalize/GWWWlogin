using System.Collections.Concurrent;

namespace GWWWlogin.GameServer.World;

public sealed class GameWorldService : IGameWorldService
{
    private readonly ConcurrentDictionary<int, GameMapRuntimeState> _maps = new(
        [
            new KeyValuePair<int, GameMapRuntimeState>(1001, new GameMapRuntimeState(1001, "Athens_Newbie", 200, 3, 1, 0, DateTime.UtcNow)),
            new KeyValuePair<int, GameMapRuntimeState>(1002, new GameMapRuntimeState(1002, "Sparta_Newbie", 200, 3, 1, 0, DateTime.UtcNow))
        ]);

    public IReadOnlyList<GameMapRuntimeState> GetMaps()
    {
        return _maps.Values
            .OrderBy(x => x.MapId)
            .ToList();
    }

    public void AdvanceSimulationTick()
    {
        foreach (var map in _maps.Values)
        {
            _maps[map.MapId] = map with
            {
                SimulationTick = map.SimulationTick + 1,
                LastSimulationAtUtc = DateTime.UtcNow
            };
        }
    }
}
