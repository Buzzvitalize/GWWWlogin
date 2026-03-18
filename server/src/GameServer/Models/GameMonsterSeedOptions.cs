namespace GWWWlogin.GameServer.Models;

public sealed class GameMonsterSeedOptions
{
    public string MonsterKey { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public float PositionX { get; set; }

    public float PositionY { get; set; }
}
