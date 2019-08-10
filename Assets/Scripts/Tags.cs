
public static class Tags
{
    public const string Respawn        = "Respawn";
    public const string Finish         = "Finish";
    public const string Player         = "Player";
    public const string GameController = "GameController";
    public const string NormalBullet   = "NormalBullet";
    public const string Asteroid       = "Asteroid";
}

public static class ButtonKeyWord
{
    public const string Horizontal = "Horizontal";
    public const string Vertical   = "Vertical";
    public const string Fire1      = "Fire1";
}

public enum EStatusType
{
    AsteroidTerminatedByDeadZone     = 0,
    AsteroidTerminatedByNormalBullet = 1,
    PlayerDefeatedByAsteroid         = 2
}

public enum ECurrencyType
{
    Chip = 0,
    Core = 1,
    Bit  = 2
}