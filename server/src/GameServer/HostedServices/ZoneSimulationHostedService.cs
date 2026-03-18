using GWWWlogin.GameServer.World;

namespace GWWWlogin.GameServer.HostedServices;

public sealed class ZoneSimulationHostedService(
    ILogger<ZoneSimulationHostedService> logger,
    IGameWorldService gameWorldService) : BackgroundService
{
    private static readonly TimeSpan TickInterval = TimeSpan.FromSeconds(2);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("GameServer zone simulation started");

        while (!stoppingToken.IsCancellationRequested)
        {
            gameWorldService.AdvanceSimulationTick();
            await Task.Delay(TickInterval, stoppingToken);
        }
    }
}
