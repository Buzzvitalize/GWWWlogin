using System.Net;
using System.Net.Sockets;
using System.Text;
using GWWWlogin.GatewayService.Models;
using GWWWlogin.GatewayService.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GWWWlogin.GatewayService.HostedServices;

public sealed class TcpGatewayHostedService(
    ILogger<TcpGatewayHostedService> logger,
    IServiceScopeFactory scopeFactory,
    IOptions<GatewayOptions> options) : BackgroundService
{
    private readonly GatewayOptions _options = options.Value;
    private TcpListener? _listener;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _listener = new TcpListener(IPAddress.Parse(_options.Host), _options.Port);
        _listener.Start();

        logger.LogInformation("Gateway TCP listening on {Host}:{Port}", _options.Host, _options.Port);

        while (!stoppingToken.IsCancellationRequested)
        {
            var client = await _listener.AcceptTcpClientAsync(stoppingToken);
            _ = HandleClientAsync(client, stoppingToken);
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _listener?.Stop();
        return base.StopAsync(cancellationToken);
    }

    private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
    {
        using (client)
        {
            var remoteEndpoint = client.Client.RemoteEndPoint?.ToString() ?? "unknown";
            logger.LogInformation("Gateway connection accepted from {RemoteEndpoint}", remoteEndpoint);

            using var networkStream = client.GetStream();
            using var reader = new StreamReader(networkStream, Encoding.UTF8, leaveOpen: true);
            using var writer = new StreamWriter(networkStream, Encoding.UTF8, leaveOpen: true) { AutoFlush = true };

            await writer.WriteLineAsync("GWWW_GATEWAY_READY");

            var line = await reader.ReadLineAsync(cancellationToken);
            if (string.IsNullOrWhiteSpace(line))
            {
                await writer.WriteLineAsync("ERROR empty_command");
                return;
            }

            using var scope = scopeFactory.CreateScope();
            var sessionService = scope.ServiceProvider.GetRequiredService<IGatewaySessionService>();
            var result = await sessionService.HandleCommandAsync(line, cancellationToken);
            await writer.WriteLineAsync(result.ResponseLine);
        }
    }
}
