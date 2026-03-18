using GWWWlogin.Shared;

namespace GWWWlogin.GatewayService.Services;

public interface IGameServerBridgeClient
{
    Task<IReadOnlyList<WorldBridgeMonsterSnapshot>> GetMonstersAsync(int mapId, CancellationToken cancellationToken);

    Task<IReadOnlyList<WorldBridgeEntityUpdate>> GetEventsAsync(long afterSequenceId, int take, CancellationToken cancellationToken);
}
