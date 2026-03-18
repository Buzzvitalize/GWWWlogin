namespace GWWWlogin.GatewayService.Models;

public sealed class ActivePlayerState
{
    public Guid SessionId { get; init; }

    public Guid AccountId { get; init; }

    public Guid CharacterId { get; init; }

    public string CharacterName { get; init; } = string.Empty;

    public string Faction { get; init; } = string.Empty;

    public string SceneName { get; init; } = string.Empty;

    public int MapId { get; set; }

    public float PositionX { get; set; }

    public float PositionY { get; set; }

    public int Level { get; init; }

    public DateTime EnteredMapAtUtc { get; init; }

    public long LastSeenBroadcastSequence { get; set; }
}
