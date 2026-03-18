using GWWWlogin.GatewayService.Protocols;

namespace GWWWlogin.GatewayService.Services;

public interface IGatewaySessionService
{
    Task<GatewayCommandResult> HandleCommandAsync(string commandLine, CancellationToken cancellationToken);
}
