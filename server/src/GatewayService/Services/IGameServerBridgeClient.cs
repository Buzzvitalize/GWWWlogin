using GWWWlogin.Shared;

namespace GWWWlogin.GatewayService.Services;

public interface IGameServerBridgeClient
{
    Task<IReadOnlyList<WorldBridgeMonsterSnapshot>> GetMonstersAsync(int mapId, CancellationToken cancellationToken);
}
