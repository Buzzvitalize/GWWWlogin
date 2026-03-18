namespace GWWWlogin.GatewayService.Models;

public sealed class GatewayOptions
{
    public const string SectionName = "Gateway";

    public string Host { get; set; } = "0.0.0.0";

    public int Port { get; set; } = 6001;

    public string PublicHost { get; set; } = "127.1.1.110";

    public int BufferSize { get; set; } = 2048;
}
