
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

public enum EObstacleTerminator
{
    TerminatedByDeadZone     = 0,
    TerminatedByNormalBullet = 1
}

public enum EPlayerDefeat
{
    DefeatedByAsteroid      = 0
}