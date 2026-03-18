using GWWWlogin.GameServer.Models;
using GWWWlogin.GameServer.World;
using Microsoft.Extensions.Options;

namespace GWWWlogin.GameServer.HostedServices;

public sealed class ZoneSimulationHostedService(
    ILogger<ZoneSimulationHostedService> logger,
    IGameWorldService gameWorldService,
    IOptions<GameServerOptions> optionsAccessor) : BackgroundService
{
    private readonly TimeSpan _tickInterval = TimeSpan.FromMilliseconds(optionsAccessor.Value.SimulationTickMilliseconds);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "GameServer zone simulation started with tick interval {TickIntervalMs}ms",
            _tickInterval.TotalMilliseconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            gameWorldService.AdvanceSimulationTick();
            await Task.Delay(_tickInterval, stoppingToken);
        }
    }
}
