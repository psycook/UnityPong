public class GamePreferences
{
    public int PlayerCount { get; set; }
    public PaddleSize P1PaddleSize { get; set; }
    public PaddleSize P2PaddleSize { get; set; }
    public Difficulty AIDifficulty { get; set; }
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
        P1PaddleSize = PaddleSize.Medium;
        P2PaddleSize = PaddleSize.Medium;
        AIDifficulty = Difficulty.Medium;
        PointsToWin = 3;
    }
}