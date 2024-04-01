using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroductionController : MonoBehaviour
{
    public TextMeshProUGUI playerCountText;
    public TextMeshProUGUI p1difficultyText;
    public TextMeshProUGUI p2difficultyText;
    public TextMeshProUGUI pointsToWinText;
    private GamePreferences _gamePreferences;

    void Start()
    {
        _gamePreferences = GamePreferences.Instance;
        string players = (_gamePreferences.PlayerCount == 1) ? "player" : "players";
        string player2Name = (_gamePreferences.PlayerCount == 1) ? "AI" : "P2";
        playerCountText.text = $"[1] {_gamePreferences.PlayerCount} {players}";
        p1difficultyText.text = $"[2] P1 {_gamePreferences.P1Difficulty}";
        p2difficultyText.text = $"[3] {player2Name} {_gamePreferences.P2Difficulty}";
        pointsToWinText.text = $"[4] {_gamePreferences.PointsToWin} points to win";
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _gamePreferences.PlayerCount = CyclePlayerCount(_gamePreferences.PlayerCount);
            string players = (_gamePreferences.PlayerCount == 1) ? "player" : "players";
            playerCountText.text = $"[1] {_gamePreferences.PlayerCount} {players}";
            string playerName = (_gamePreferences.PlayerCount == 1) ? "AI" : "P2";
            p2difficultyText.text = $"[3] {playerName} {_gamePreferences.P2Difficulty}";
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _gamePreferences.P1Difficulty = CycleDifficulty(_gamePreferences.P1Difficulty);
            p1difficultyText.text = $"[2] P1 {_gamePreferences.P1Difficulty}";
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _gamePreferences.P2Difficulty = CycleDifficulty(_gamePreferences.P2Difficulty);
            string player2Name = (_gamePreferences.PlayerCount == 1) ? "AI" : "P2";
            p2difficultyText.text = $"[3] {player2Name} {_gamePreferences.P2Difficulty}";
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _gamePreferences.PointsToWin = CyclePointsToWin(_gamePreferences.PointsToWin);
            pointsToWinText.text = $"[4] {_gamePreferences.PointsToWin} points to win";
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    private int CyclePlayerCount(int currentPlayerCount)
    {
        return (currentPlayerCount == 1) ? 2 : 1;
    }

    private Difficulty CycleDifficulty(Difficulty currentDifficulty)
    {
        Difficulty response = Difficulty.Easy;
        switch(currentDifficulty)
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

    private int CyclePointsToWin(int currentPointsToWin)
    {
        int response = 5;
        switch(currentPointsToWin)
        {
            case 5:
                response = 7;
                break;
            case 7:
                response = 11;
                break;
            case 11:
                response = 15;
                break;
            default:
                response = 5;
                break;
        }
        return response;
    }
}