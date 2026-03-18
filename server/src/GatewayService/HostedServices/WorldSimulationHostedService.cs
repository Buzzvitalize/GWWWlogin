using System.Collections.Concurrent;
using GWWWlogin.GatewayService.Broadcast;
using GWWWlogin.GatewayService.Definitions;
using GWWWlogin.GatewayService.Models;
using GWWWlogin.GatewayService.World;

namespace GWWWlogin.GatewayService.HostedServices;

public sealed class WorldSimulationHostedService(
    ILogger<WorldSimulationHostedService> logger,
    IMapDefinitionService mapDefinitionService,
    IMapStateService mapStateService,
    IMapBroadcastService mapBroadcastService) : BackgroundService
{
    private const float StepDistance = 12f;
    private static readonly TimeSpan TickInterval = TimeSpan.FromSeconds(3);
    private readonly ConcurrentDictionary<string, int> _tickDirectionByMonster = new(StringComparer.OrdinalIgnoreCase);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                SimulateMonsterMovement();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "World simulation tick failed");
            }

            await Task.Delay(TickInterval, stoppingToken);
        }
    }

    private void SimulateMonsterMovement()
    {
        foreach (var map in mapDefinitionService.GetAll())
        {
            var monsters = mapStateService.GetMonsters(map.Id);
            foreach (var monster in monsters.Where(x => x.IsAlive))
            {
                var tick = _tickDirectionByMonster.AddOrUpdate(monster.InstanceId, 1, (_, current) => current + 1);
                var direction = tick % 2 == 0 ? -1f : 1f;
                var nextX = monster.PositionX + (StepDistance * direction);
                var nextY = monster.PositionY;

                var updated = mapStateService.UpdateMonsterPosition(monster.InstanceId, nextX, nextY);
                if (updated is null)
                {
                    continue;
                }

                mapBroadcastService.Publish(map.Id, new MapBroadcastEvent(
                    0,
                    "ENTITY_MOVE",
                    Guid.Empty,
                    updated.DisplayName,
                    updated.SceneName,
                    updated.PositionX,
                    updated.PositionY,
                    DateTime.UtcNow,
                    "monster",
                    updated.InstanceId));
            }
        }
    }
}
