namespace GWWWlogin.GatewayService.Models;

public sealed class GameServerBridgeOptions
{
    public const string SectionName = "GameServerBridge";

    public string BaseUrl { get; set; } = "http://127.0.0.1:5100";
}
