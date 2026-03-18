namespace GWWWlogin.GatewayService.Protocols;

public sealed record GatewayCommandResult(
    bool Success,
    string ResponseLine);
