using System.ComponentModel.DataAnnotations;

namespace GWWWlogin.GameServer.Models;

public sealed class GameServerOptions
{
    public const string SectionName = "GameServer";

    [Required]
    public string Host { get; set; } = "0.0.0.0";

    [Range(1, 65535)]
    public int Port { get; set; } = 5100;

    [Range(50, 5000)]
    public int ZoneSize { get; set; } = 200;

    [Range(100, 60000)]
    public int SimulationTickMilliseconds { get; set; } = 2000;

    [Range(1, 100)]
    public int MaxSeedMonstersPerMap { get; set; } = 8;

    [Range(1, 100)]
    public float StepDistance { get; set; } = 12f;

    [Range(10, 5000)]
    public int RecentEventLimit { get; set; } = 200;
}
