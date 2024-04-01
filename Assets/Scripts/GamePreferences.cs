public class GamePreferences
{
    public int PlayerCount { get; set; }
    public Difficulty P1Difficulty { get; set; }
    public Difficulty P2Difficulty { get; set; }
    public int PointsToWin { get; set; }

    public static GamePreferences _instance;

    public static GamePreferences Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new GamePreferences();
            }
            return _instance;
        }
    }

    GamePreferences()
    {
        PlayerCount = 2;
        P1Difficulty = Difficulty.Medium;
        P2Difficulty = Difficulty.Medium;
        PointsToWin = 5;
    }
}