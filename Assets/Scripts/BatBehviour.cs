using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BatBehaviour : MonoBehaviour
{
    public float speed = 5.0f;
    public float minY = -3.0f;
    public float maxY = 3.0f;
    public bool isAI = false;

    private float _direction;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private Difficulty _difficulty = Difficulty.Medium;

    void Start()
    {
        if (!isAI)
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

        _difficulty = gameObject.name.ToLower() == "paddle1" ?
            GamePreferences.Instance.P1Difficulty :
            GamePreferences.Instance.P2Difficulty;

        switch(_difficulty)
        {
            case Difficulty.Easy:
                transform.localScale = new Vector3(transform.localScale.x, 1.5f, transform.localScale.z);
                break;
            case Difficulty.Medium:
                transform.localScale = new Vector3(transform.localScale.x, 1.0f, transform.localScale.z);
                break;
            default:
                transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
                break;
        }
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
        if(isAI)
        {
            GameObject gameController = GameObject.Find("GameController");
            GameBehaviour gameBehaviour = gameController.GetComponent<GameBehaviour>();
            if (gameBehaviour != null)
            {
                GameObject firstBall = gameBehaviour.getFirstBall();
                if (firstBall == null)
                {
                    _direction = 0.0f;
                    return;
                }
                if (firstBall.transform.position.y < (transform.position.y - (transform.localScale.y/2)))
                {
                    _direction = -1.0f;
                }
                else if (firstBall.transform.position.y > (transform.position.y + (transform.localScale.y/2)))
                {
                    _direction = 1.0f;
                }
                else
                {
                    _direction = 0.0f;
                }
            }
        }
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(0.0f, _direction * speed * Time.deltaTime, 0.0f);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minY, maxY), 0.0f);
    }
}