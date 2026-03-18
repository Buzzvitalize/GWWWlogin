namespace GWWWlogin.GatewayService.Models;

public sealed class GatewaySessionRecord
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public Guid? SelectedCharacterId { get; set; }

    public GatewayCharacterRecord? SelectedCharacter { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime ExpiresAtUtc { get; set; }

    public DateTime LastSeenAtUtc { get; set; }
}
