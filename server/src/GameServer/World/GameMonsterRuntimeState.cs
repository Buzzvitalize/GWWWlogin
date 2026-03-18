namespace GWWWlogin.GameServer.World;

public sealed class GameMonsterRuntimeState
{
    public string InstanceId { get; init; } = string.Empty;

    public int MapId { get; init; }

    public string SceneName { get; init; } = string.Empty;

    public string DisplayName { get; init; } = string.Empty;

    public float PositionX { get; set; }

    public float PositionY { get; set; }

    public string ZoneKey { get; set; } = string.Empty;

    public bool IsAlive { get; set; } = true;

    public DateTime LastUpdatedAtUtc { get; set; }
}
