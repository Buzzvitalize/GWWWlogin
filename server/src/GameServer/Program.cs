using GWWWlogin.GameServer.HostedServices;
using GWWWlogin.GameServer.Models;
using GWWWlogin.GameServer.World;
using GWWWlogin.Shared.Maps;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<GameServerOptions>()
    .Bind(builder.Configuration.GetSection(GameServerOptions.SectionName))
    .ValidateDataAnnotations()
    .Validate(options => !string.IsNullOrWhiteSpace(options.Host), "GameServer:Host is required.")
    .ValidateOnStart();

var configuredOptions = builder.Configuration.GetSection(GameServerOptions.SectionName).Get<GameServerOptions>() ?? new GameServerOptions();
builder.WebHost.UseUrls($"http://{configuredOptions.Host}:{configuredOptions.Port}");

builder.Services.AddSingleton<IClientMapCatalog, ClientMapCatalog>();
builder.Services.AddSingleton<IGameWorldService, GameWorldService>();
builder.Services.AddHostedService<ZoneSimulationHostedService>();

var app = builder.Build();

app.MapGet("/health", (IOptions<GameServerOptions> optionsAccessor, IGameWorldService gameWorldService, IClientMapCatalog mapCatalog) =>
{
    var options = optionsAccessor.Value;
    var configuredMaps = mapCatalog.GetAll();
    var maps = gameWorldService.GetMaps();
    var transitions = configuredMaps.Sum(map => map.Addresses.Count);
    var npcs = configuredMaps.Sum(map => map.Npcs.Count);

    return Results.Ok(new
    {
        service = "game-server",
        status = "ok",
        host = options.Host,
        port = options.Port,
        zoneSize = options.ZoneSize,
        simulationTickMilliseconds = options.SimulationTickMilliseconds,
        maxSeedMonstersPerMap = options.MaxSeedMonstersPerMap,
        stepDistance = options.StepDistance,
        recentEventLimit = options.RecentEventLimit,
        configuredMaps = configuredMaps.Count,
        configuredTransitions = transitions,
        configuredNpcs = npcs,
        maps,
        utc = DateTime.UtcNow
    });
});

app.MapGet("/maps", (IGameWorldService gameWorldService) => Results.Ok(gameWorldService.GetMaps()));
app.MapGet("/world/config", (IOptions<GameServerOptions> optionsAccessor) => Results.Ok(optionsAccessor.Value));
app.MapGet("/world/maps", (IGameWorldService gameWorldService) => Results.Ok(gameWorldService.GetMaps()));
app.MapGet("/world/transitions", (IClientMapCatalog mapCatalog, int? mapId) =>
{
    var transitions = mapCatalog.GetAll()
        .Where(map => !mapId.HasValue || map.MapId == mapId.Value)
        .SelectMany(map => map.Addresses.Select((address, index) => new
        {
            mapId = map.MapId,
            sceneName = map.SceneName,
            transitionIndex = index + 1,
            address.Name,
            address.PositionX,
            address.PositionY
        }))
        .ToList();

    return Results.Ok(transitions);
});
app.MapGet("/world/monsters", (IGameWorldService gameWorldService, int? mapId) => Results.Ok(gameWorldService.GetMonsters(mapId)));
app.MapGet("/world/events", (IGameWorldService gameWorldService, long afterSequenceId = 0, int take = 50) => Results.Ok(gameWorldService.GetRecentUpdates(afterSequenceId, take)));

app.Run();
