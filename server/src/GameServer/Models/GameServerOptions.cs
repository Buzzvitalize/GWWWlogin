namespace GWWWlogin.GameServer.Models;

public sealed class GameServerOptions
{
    public const string SectionName = "GameServer";

    public string Host { get; set; } = "0.0.0.0";

    public int Port { get; set; } = 5100;

    public int ZoneSize { get; set; } = 200;

    public List<GameMapOptions> Maps { get; set; } = [];
}
