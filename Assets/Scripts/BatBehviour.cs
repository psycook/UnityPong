using UnityEngine;
using UnityEngine.InputSystem;

public class BatBehaviour : MonoBehaviour
{
    public float MaxSpeed = 5.0f;
    public float MinY = -5.0f;
    public float MaxY = 5.0f;
    public bool  IsAI = false;

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private PaddleSize _paddleSize = PaddleSize.Medium;
    private Difficulty _aiDifficulty = Difficulty.Medium;
    private float _direction;
    private float _speed;
    private float _clampMin, _clampMax = 0.0f;
    private float _aiDeadzone = 0.5f;

    void Start()
    {
        if(gameObject.name.ToLower() == "paddle1")
        {
            _paddleSize = GamePreferences.Instance.P1PaddleSize;
        }
        else
        {
            _paddleSize = GamePreferences.Instance.P2PaddleSize;
            _aiDifficulty = GamePreferences.Instance.AIDifficulty;
            IsAI = GamePreferences.Instance.PlayerCount == 1 ? true : false;
        }

        if (IsAI)
        {
            switch (_aiDifficulty)
            {
                case Difficulty.Easy:
                    MaxSpeed *= 0.5f;
                    break;
                case Difficulty.Hard:
                    MaxSpeed *= 2.0f;
                    break;
            }
        }
        else
        {
            _playerInput = GetComponent<PlayerInput>();
            _moveAction = _playerInput.actions["Move"];
            if (_moveAction != null)
            {
                _moveAction.Enable();
                _moveAction.performed += OnMovePerformed;
                _moveAction.canceled += OnMoveCancelled;
            }
        }

        switch (_paddleSize)
        {
            case PaddleSize.Big:
                transform.localScale = new Vector3(transform.localScale.x, 1.5f, transform.localScale.z);
                break;
            case PaddleSize.Medium:
                transform.localScale = new Vector3(transform.localScale.x, 1.0f, transform.localScale.z);
                break;
            default:
                transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
                break;
        }

        _clampMin = MinY + (transform.localScale.y/2);
        _clampMax = MaxY - (transform.localScale.y/2);
        _aiDeadzone = (transform.localScale.y/2);
    }

    private void OnDisable()
    {
        if(_moveAction != null)
        {
            _moveAction.Disable();
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        _direction = context.ReadValue<float>();
    }

    private void OnMoveCancelled(InputAction.CallbackContext context)
    {
        _direction = 0.0f;
    }

    private void Update()
    {
        if (IsAI)
        {
            GameObject gameController = GameObject.Find("GameController");
            GameBehaviour gameBehaviour = gameController.GetComponent<GameBehaviour>();
            if (gameBehaviour == null) return;
            GameObject firstBall = gameBehaviour.getFirstBall();
            if (firstBall == null)
            {
                _direction = 0.0f;
                _speed = 0.0f;
                return;
            }

            float ballY = firstBall.transform.position.y;
            float paddleY = transform.position.y;
            float delta = ballY - paddleY;

            if (Mathf.Abs(delta) < _aiDeadzone)
            {
                _direction = 0.0f;
                _speed = 0.0f;
            }
            else
            {
                _direction = (delta < 0) ? -1.0f : 1.0f;
                _speed = Mathf.Clamp(Mathf.Abs(delta) * MaxSpeed, -MaxSpeed, MaxSpeed);
            }
        }
        else
        {
            _speed = MaxSpeed;
        }
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(0.0f, _direction * _speed * Time.deltaTime, 0.0f);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _clampMin, _clampMax), 0.0f);
    }
}