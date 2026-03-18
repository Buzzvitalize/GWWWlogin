using GWWWlogin.GatewayService.Broadcast;
using GWWWlogin.GatewayService.Data;
using GWWWlogin.GatewayService.Definitions;
using GWWWlogin.GatewayService.HostedServices;
using GWWWlogin.GatewayService.Models;
using GWWWlogin.GatewayService.Protocols;
using GWWWlogin.GatewayService.Services;
using GWWWlogin.GatewayService.World;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GatewayOptions>(builder.Configuration.GetSection(GatewayOptions.SectionName));
builder.Services.AddDbContext<GatewayDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AuthDb")));
builder.Services.AddSingleton<IMapDefinitionService, MapDefinitionService>();
builder.Services.AddSingleton<IMapStateService, MapStateService>();
builder.Services.AddSingleton<IMapBroadcastService, MapBroadcastService>();
builder.Services.AddScoped<IGatewaySessionService, GatewaySessionService>();
builder.Services.AddHostedService<TcpGatewayHostedService>();
builder.Services.AddHostedService<WorldSimulationHostedService>();

var app = builder.Build();

app.MapGet("/health", async (IConfiguration configuration, GatewayDbContext dbContext, IMapDefinitionService mapDefinitionService, CancellationToken cancellationToken) =>
{
    var options = configuration.GetSection(GatewayOptions.SectionName).Get<GatewayOptions>() ?? new GatewayOptions();
    var sessionCount = await dbContext.Sessions.CountAsync(cancellationToken);

    return Results.Ok(new
    {
        service = "gateway-service",
        status = "ok",
        tcpHost = options.Host,
        tcpPort = options.Port,
        publicHost = options.PublicHost,
        sessions = sessionCount,
        maps = mapDefinitionService.GetAll().Count,
        supportedCommands = new[] { "HELLO", "ENTER_MAP", "PING", "WHOAMI", "MOVE", "LEAVE", "POLL", "AROUND" },
        handshakeVersion = GatewayProtocolSerializer.HandshakeVersion,
        worldEventVersion = GatewayProtocolSerializer.WorldEventVersion,
        aroundVersion = GatewayProtocolSerializer.AroundVersion,
        utc = DateTime.UtcNow
    });
});

app.Run();
