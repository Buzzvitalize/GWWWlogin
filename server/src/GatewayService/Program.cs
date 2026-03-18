using GWWWlogin.GatewayService.HostedServices;
using GWWWlogin.GatewayService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GatewayOptions>(builder.Configuration.GetSection(GatewayOptions.SectionName));
builder.Services.AddHostedService<TcpGatewayHostedService>();

var app = builder.Build();

app.MapGet("/health", (IConfiguration configuration) =>
{
    var options = configuration.GetSection(GatewayOptions.SectionName).Get<GatewayOptions>() ?? new GatewayOptions();

    return Results.Ok(new
    {
        service = "gateway-service",
        status = "ok",
        tcpHost = options.Host,
        tcpPort = options.Port,
        publicHost = options.PublicHost,
        utc = DateTime.UtcNow
    });
});

app.Run();
