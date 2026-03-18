namespace GWWWlogin.GatewayService.Models;

public sealed class LiveNpcState
{
    public string InstanceId { get; init; } = string.Empty;

    public string NpcKey { get; init; } = string.Empty;

    public string DisplayName { get; init; } = string.Empty;

    public string SceneName { get; init; } = string.Empty;

    public int MapId { get; init; }

    public float PositionX { get; set; }

    public float PositionY { get; set; }

    public string ZoneKey { get; set; } = string.Empty;

    public DateTime LastUpdatedAtUtc { get; set; }
}
