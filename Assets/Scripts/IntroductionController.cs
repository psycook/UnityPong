using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroductionController : MonoBehaviour
{
    public TextMeshProUGUI playerCountText;
    public TextMeshProUGUI pointsToWinText;
    public TextMeshProUGUI p1difficultyText;
    public TextMeshProUGUI p2difficultyText;
    public TextMeshProUGUI aiDifficulty;
    private GamePreferences _gamePreferences;

    void Start()
    {
        _gamePreferences = GamePreferences.Instance;
        string players = (_gamePreferences.PlayerCount == 1) ? "player" : "players";
        string player2Name = (_gamePreferences.PlayerCount == 1) ? "AI" : "P2";
        playerCountText.text = $"[1] {_gamePreferences.PlayerCount} {players}";
        pointsToWinText.text = $"[2] {_gamePreferences.PointsToWin} points to win";
        p1difficultyText.text = $"[3] P1 Paddle - {_gamePreferences.P1PaddleSize}";
        p2difficultyText.text = $"[4] {player2Name} Paddle - {_gamePreferences.P2PaddleSize}";
        aiDifficulty.text = $"[5] AI Difficulty - {_gamePreferences.AIDifficulty}";
        aiDifficulty.enabled = (_gamePreferences.PlayerCount == 1) ? true : false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _gamePreferences.PlayerCount = CyclePlayerCount(_gamePreferences.PlayerCount);
            string players = (_gamePreferences.PlayerCount == 1) ? "player" : "players";
            playerCountText.text = $"[1] {_gamePreferences.PlayerCount} {players}";
            string player2Name = (_gamePreferences.PlayerCount == 1) ? "AI" : "P2";
            p2difficultyText.text = $"[4] {player2Name} Paddle - {_gamePreferences.P2PaddleSize}";
            aiDifficulty.enabled = (_gamePreferences.PlayerCount == 1) ? true : false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _gamePreferences.PointsToWin = CyclePointsToWin(_gamePreferences.PointsToWin);
            pointsToWinText.text = $"[2] {_gamePreferences.PointsToWin} points to win";
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _gamePreferences.P1PaddleSize = CyclePaddleSize(_gamePreferences.P1PaddleSize);
            p1difficultyText.text = $"[3] P1 Paddle - {_gamePreferences.P1PaddleSize}";
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _gamePreferences.P2PaddleSize = CyclePaddleSize(_gamePreferences.P2PaddleSize);
            string player2Name = (_gamePreferences.PlayerCount == 1) ? "AI" : "P2";
            p2difficultyText.text = $"[4] {player2Name} Paddle - {_gamePreferences.P2PaddleSize}";
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _gamePreferences.AIDifficulty = CycleAIDifficulty(_gamePreferences.AIDifficulty);
            aiDifficulty.text = $"[5] AI Difficulty - {_gamePreferences.AIDifficulty}";
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    private int CyclePlayerCount(int currentPlayerCount)
    {
        return (currentPlayerCount == 1) ? 2 : 1;
    }

    private int CyclePointsToWin(int currentPointsToWin)
    {
        int response = 5;
        switch (currentPointsToWin)
        {
            case 3:
                response = 5;
                break;
            case 5:
                response = 7;
                break;
            case 7:
                response = 11;
                break;
            default:
                response = 3;
                break;
        }
        return response;
    }

    private PaddleSize CyclePaddleSize(PaddleSize currentDifficulty)
    {
        PaddleSize response = PaddleSize.Small;
        switch(currentDifficulty)
        {
            case PaddleSize.Small:
                response = PaddleSize.Medium;
                break;
            case PaddleSize.Medium:
                response = PaddleSize.Big;
                break;
            default:
                response = PaddleSize.Small;
                break;
        }
        return response;
    }

    private Difficulty CycleAIDifficulty(Difficulty currentDifficulty)
    {
        Difficulty response = Difficulty.Easy;
        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                response = Difficulty.Medium;
                break;
            case Difficulty.Medium:
                response = Difficulty.Hard;
                break;
            default:
                response = Difficulty.Easy;
                break;
        }
        return response;
    }
}