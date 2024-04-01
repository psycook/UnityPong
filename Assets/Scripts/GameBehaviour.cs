using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameBehaviour : MonoBehaviour
{
    public GameObject ballPrefab;
    public TextMeshProUGUI p1Score;
    public TextMeshProUGUI p2Score;

    private string _state;
    private int _paddle1Score;
    private int _paddle2Score;
    private List<GameObject> _ballArray;

    void Start()
    {
        _state = "idle";
        _paddle1Score = 0;
        _paddle2Score = 0;
        _ballArray = new List<GameObject>();
        UpdateScores();
    }


    void UpdateScores()
    {
        p1Score.text = $"{_paddle1Score.ToString("D2")}";
        p2Score.text = $"{_paddle2Score.ToString("D2")}";
    }

    void Update()
    {
        if (_state == "idle")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
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
        }
        if (_ballArray.Count == 0)
        {
            Debug.Log("No balls");
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