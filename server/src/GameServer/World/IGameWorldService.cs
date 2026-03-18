namespace GWWWlogin.GameServer.World;

public interface IGameWorldService
{
    IReadOnlyList<GameMapRuntimeState> GetMaps();

    void AdvanceSimulationTick();
}
