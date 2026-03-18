using GWWWlogin.GameServer.HostedServices;
using GWWWlogin.GameServer.Models;
using GWWWlogin.GameServer.World;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GameServerOptions>(builder.Configuration.GetSection(GameServerOptions.SectionName));
builder.Services.AddSingleton<IGameWorldService, GameWorldService>();
builder.Services.AddHostedService<ZoneSimulationHostedService>();

var app = builder.Build();

app.MapGet("/health", (IConfiguration configuration, IGameWorldService gameWorldService) =>
{
    var options = configuration.GetSection(GameServerOptions.SectionName).Get<GameServerOptions>() ?? new GameServerOptions();
    var maps = gameWorldService.GetMaps();

    return Results.Ok(new
    {
        service = "game-server",
        status = "ok",
        host = options.Host,
        port = options.Port,
        zoneSize = options.ZoneSize,
        maps,
        utc = DateTime.UtcNow
    });
});

app.MapGet("/world/maps", (IGameWorldService gameWorldService) => Results.Ok(gameWorldService.GetMaps()));
app.MapGet("/world/monsters", (IGameWorldService gameWorldService, int? mapId) => Results.Ok(gameWorldService.GetMonsters(mapId)));
app.MapGet("/world/events", (IGameWorldService gameWorldService, long afterSequenceId = 0, int take = 50) => Results.Ok(gameWorldService.GetRecentUpdates(afterSequenceId, take)));

app.Run();
