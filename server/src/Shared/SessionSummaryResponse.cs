namespace GWWWlogin.Shared;

public sealed record SessionSummaryResponse(
    Guid SessionId,
    Guid AccountId,
    Guid? SelectedCharacterId,
    string SessionToken,
    DateTime CreatedAtUtc,
    DateTime ExpiresAtUtc,
    DateTime LastSeenAtUtc,
    bool IsActive);
