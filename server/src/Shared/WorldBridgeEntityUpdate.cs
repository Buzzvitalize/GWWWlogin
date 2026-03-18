namespace GWWWlogin.Shared;

public sealed record WorldBridgeEntityUpdate(
    long SequenceId,
    int MapId,
    string EventType,
    string EntityKind,
    string EntityInstanceId,
    string DisplayName,
    float PositionX,
    float PositionY,
    string ZoneKey,
    DateTime OccurredAtUtc);
