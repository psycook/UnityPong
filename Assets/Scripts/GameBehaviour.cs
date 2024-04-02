using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameBehaviour : MonoBehaviour
{
    public GameObject ballPrefab;
    public TextMeshProUGUI p1Score;
    public TextMeshProUGUI p2Score;
    public TextMeshProUGUI message;

    private int _paddle1Score;
    private int _paddle2Score;
    private List<GameObject> _ballArray;
    private GameState _state = GameState.Idle;
    private string[] _launchSequence = { "READY", "STEADY", "LETS PONG!" };
    private int _sequenceIndex = 0;
    private float _sequenceTimer = 1.0f;

    void Start()
    {
        _paddle1Score = 0;
        _paddle2Score = 0;
        _ballArray = new List<GameObject>();
        ResetPoint();
        UpdateScores();
    }

    void ResetPoint()
    {
        _state = GameState.Idle;
        message.text = "SPACE TO SERVE";
        message.enabled = true;
    }

    void UpdateScores()
    {
        p1Score.text = $"{_paddle1Score.ToString("D2")}";
        p2Score.text = $"{_paddle2Score.ToString("D2")}";
    }

    void LaunchGame()
    {
        _sequenceIndex = 0;
        _state = GameState.LaunchSequence;
        _sequenceTimer = 1.0f;
        message.text = _launchSequence[_sequenceIndex];
        message.enabled = true;
    }

    void Update()
    {
        if(_state == GameState.Idle)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                LaunchGame();
            }
        }
        else if(_state == GameState.GameWon)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("IntroScene");
            }
        }
        else if(_state == GameState.LaunchSequence)
        {
            _sequenceTimer -= Time.deltaTime;
            if(_sequenceTimer <= 0.0)
            {
                _sequenceTimer = 1.0f;
                if (++_sequenceIndex == _launchSequence.Length)
                {
                    message.enabled = false;
                    _state = GameState.LetsPong;
                    StartGame();
                }
                else
                {
                    message.text = _launchSequence[_sequenceIndex];
                }
            }
        }
    }

    private void StartGame()
    {
        NewBall();
    }

    public void NewBall()
    {
        GameObject newBall = Instantiate(ballPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        _ballArray.Add(newBall);
    }

    public void BallLost(Collider2D outOfBoundsCollision, GameObject ball)
    {
        if (outOfBoundsCollision.gameObject.transform.position.x > 0)
        {
            _paddle1Score++;
        }
        else
        {
            _paddle2Score++;
        }

        UpdateScores();

        if (_ballArray.Contains(ball))
        {
            bool isRemoved = _ballArray.Remove(ball);
            if(isRemoved && _ballArray.Count == 0)
            {
                ResetPoint();
            }
        }

        if (_paddle1Score == GamePreferences.Instance.PointsToWin)
        {
            _state = GameState.GameWon;
            message.text = "CONGRATS P1 - SPACE TO REPLAY";
            message.enabled = true;
            return;
        }

        if (_paddle1Score == GamePreferences.Instance.PointsToWin  ||
            _paddle2Score == GamePreferences.Instance.PointsToWin)
        {
            _state = GameState.GameWon;
            string playerName = (_paddle1Score == GamePreferences.Instance.PointsToWin) ? "P1" : "P2";
            message.text = $"CONGRATS {playerName} - SPACE TO REPLAY";
            message.enabled = true;
            return;
        }
    }

    public GameObject getFirstBall()
    {
        if(_ballArray != null && _ballArray.Count > 0)
        {
            return _ballArray[0];
        }
        return null;
    }
}