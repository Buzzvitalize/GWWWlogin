namespace GWWWlogin.LoginService.Models;

public sealed class Character
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public Account? Account { get; set; }

    public string Name { get; set; } = string.Empty;

    public string NormalizedName { get; set; } = string.Empty;

    public string Class { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public int Level { get; set; }

    public int MapId { get; set; }

    public float PositionX { get; set; }

    public float PositionY { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}
