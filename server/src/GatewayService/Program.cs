using GWWWlogin.GatewayService.Data;
using GWWWlogin.GatewayService.HostedServices;
using GWWWlogin.GatewayService.Models;
using GWWWlogin.GatewayService.Services;
using GWWWlogin.GatewayService.World;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GatewayOptions>(builder.Configuration.GetSection(GatewayOptions.SectionName));
builder.Services.AddDbContext<GatewayDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AuthDb")));
builder.Services.AddSingleton<IMapStateService, MapStateService>();
builder.Services.AddScoped<IGatewaySessionService, GatewaySessionService>();
builder.Services.AddHostedService<TcpGatewayHostedService>();

var app = builder.Build();

app.MapGet("/health", async (IConfiguration configuration, GatewayDbContext dbContext, CancellationToken cancellationToken) =>
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
        supportedCommands = new[] { "HELLO", "ENTER_MAP", "PING", "WHOAMI", "MOVE" },
        utc = DateTime.UtcNow
    });
});

app.Run();
