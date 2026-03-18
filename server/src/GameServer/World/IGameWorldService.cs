using GWWWlogin.Shared;

namespace GWWWlogin.GameServer.World;

public interface IGameWorldService
{
    IReadOnlyList<WorldBridgeMapSnapshot> GetMaps();

    IReadOnlyList<WorldBridgeMonsterSnapshot> GetMonsters(int? mapId = null);

    IReadOnlyList<WorldBridgeEntityUpdate> GetRecentUpdates(long afterSequenceId, int take);

    void AdvanceSimulationTick();
}
