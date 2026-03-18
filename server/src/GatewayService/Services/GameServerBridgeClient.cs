using System.Net.Http.Json;
using GWWWlogin.GatewayService.Models;
using GWWWlogin.Shared;
using Microsoft.Extensions.Options;

namespace GWWWlogin.GatewayService.Services;

public sealed class GameServerBridgeClient(
    HttpClient httpClient,
    IOptions<GameServerBridgeOptions> options,
    ILogger<GameServerBridgeClient> logger) : IGameServerBridgeClient
{
    private readonly GameServerBridgeOptions _options = options.Value;

    public async Task<IReadOnlyList<WorldBridgeMonsterSnapshot>> GetMonstersAsync(int mapId, CancellationToken cancellationToken)
    {
        try
        {
            var monsters = await httpClient.GetFromJsonAsync<List<WorldBridgeMonsterSnapshot>>(
                $"/world/monsters?mapId={mapId}",
                cancellationToken);

            return monsters ?? [];
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "GameServer bridge request failed against {BaseUrl}", _options.BaseUrl);
            return [];
        }
    }

    public async Task<IReadOnlyList<WorldBridgeEntityUpdate>> GetEventsAsync(long afterSequenceId, int take, CancellationToken cancellationToken)
    {
        try
        {
            var events = await httpClient.GetFromJsonAsync<List<WorldBridgeEntityUpdate>>(
                $"/world/events?afterSequenceId={afterSequenceId}&take={take}",
                cancellationToken);

            return events ?? [];
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "GameServer bridge events request failed against {BaseUrl}", _options.BaseUrl);
            return [];
        }
    }
}
