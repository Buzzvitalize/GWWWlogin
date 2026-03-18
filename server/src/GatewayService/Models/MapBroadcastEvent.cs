namespace GWWWlogin.GatewayService.Models;

public sealed record MapBroadcastEvent(
    long SequenceId,
    string EventType,
    Guid SessionId,
    string CharacterName,
    string SceneName,
    float PositionX,
    float PositionY,
    DateTime OccurredAtUtc);
