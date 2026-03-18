namespace GWWWlogin.LoginService.Models;

public sealed class Session
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public Account? Account { get; set; }

    public Guid? SelectedCharacterId { get; set; }

    public Character? SelectedCharacter { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime ExpiresAtUtc { get; set; }

    public DateTime LastSeenAtUtc { get; set; }
}
