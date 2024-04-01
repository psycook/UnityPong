using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public float speed = 5.0f;
    public float bounceVelocityMultiplier = 1.0f;
    public AudioClip batHit;
    public AudioClip wallHit;
    public AudioClip ballFail;

    private Rigidbody2D _rigidbody;
    private AudioSource _audioSource;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        int direction = Random.Range(0, 2);
        float angle = (direction == 0) ? Random.Range(-45.0f, 45.0f) : Random.Range(135.0f, 225.0f);
        angle *= Mathf.Deg2Rad;
        Vector2 ballDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        _rigidbody.velocity = ballDirection * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    { 
        if(collision.gameObject.tag == "Wall")
        {
            _audioSource.PlayOneShot(wallHit);
            if(_rigidbody.velocity.y < 0.1 && _rigidbody.velocity.y > -0.1)
            {
                if(_rigidbody.velocity.y < 0)
                {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 1.0f);
                }
                else
                {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -1.0f);
                }
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            _audioSource.PlayOneShot(batHit);
            float positionDifference = transform.position.y - collision.gameObject.transform.position.y;
            Vector2 newDirection = new Vector2(_rigidbody.velocity.x, positionDifference * 5.0f).normalized;
            _rigidbody.velocity = newDirection * speed;
            speed += 0.25f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "OutOfBounds")
        {
            _audioSource.PlayOneShot(ballFail);
            GameObject gameController = GameObject.Find("GameController");
            GameBehaviour gameBehaviour = gameController.GetComponent<GameBehaviour>();
            if (gameBehaviour != null)
            {
                gameBehaviour.BallLost(collision, gameObject);
            }
            StartCoroutine(DestroyAfterSound(ballFail.length));
        }
    }

    IEnumerator DestroyAfterSound(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}