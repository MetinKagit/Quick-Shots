using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private GameManager _gameManager;
    private Timer timer;
    private BallPool ballPool;
    private Transform spawnPoint;
    private Quaternion startRotation;
    private Coroutine destroyCoroutine;
    private bool isTargetLive;
    
    private Dictionary<string, Action<Collision>> collisionActions;
    void Awake()
    {
        collisionActions = new Dictionary<string, Action<Collision>>
        {
            { "Target", HandleTargetCollision },
            { "TargetHard", HandleTargetHardCollision },
            { "TargetBall", HandleTargetBallCollision },
            { "TargetTime", HandleTargetTimeCollision }
        };
    }
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        timer = FindObjectOfType<Timer>();
        ballPool = FindObjectOfType<BallPool>();
        startRotation = transform.rotation;
        StartCoroutine(RotateOverTime(Vector3.right * -90f, 0.2f));
        if (gameObject.CompareTag("Target"))
        {
            destroyCoroutine = StartCoroutine(DestroyAfterDelay(2f));
        }
        else if (gameObject.CompareTag("TargetHard"))
        {
            destroyCoroutine = StartCoroutine(DestroyAfterDelay(1.5f));
        }
        else if (gameObject.CompareTag("TargetBall"))
        {
            destroyCoroutine = StartCoroutine(DestroyAfterDelay(1.5f));
        }
        else if (gameObject.CompareTag("TargetTime"))
        {
            destroyCoroutine = StartCoroutine(DestroyAfterDelay(1.5f));
        }
    }
    
    private void HandleTargetCollision(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            HandleCollision(10, collision);
        }
    }

    private void HandleTargetHardCollision(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            HandleCollision(20, collision);
        }
    }

    private void HandleTargetBallCollision(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            HandleCollision(3, collision);
        }
    }

    private void HandleTargetTimeCollision(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            HandleCollision(7, collision);
        }
    }
    
    public void SetSpawnPoint(Transform spawnPoint)
    {
        this.spawnPoint = spawnPoint;
    }
    
    IEnumerator RotateOverTime(Vector3 targetRotation, float duration)
    {
        Quaternion startRotationVar = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(transform.eulerAngles + targetRotation);

        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            // Interpolate rotation over time
            transform.rotation = Quaternion.Lerp(startRotationVar, endRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the rotation ends exactly at the target rotation
        transform.rotation = endRotation;
    }
    
    public IEnumerator RotateAndDestroy(int score)
    {
        yield return StartCoroutine(RotateOverTime(startRotation.eulerAngles, 0.2f));
        Destroy(gameObject);
        _gameManager.FreeSpawnPoint(spawnPoint);
        _gameManager.UpdateScore(score);
    }
    public IEnumerator RotateAndDestroyBall(int score)
    {
        yield return StartCoroutine(RotateOverTime(startRotation.eulerAngles, 0.2f));
        Destroy(gameObject);
        _gameManager.FreeSpawnPoint(spawnPoint);
        ballPool.AddBalls(score);
    }
    public IEnumerator RotateAndDestroyTimer(int score)
    {
        yield return StartCoroutine(RotateOverTime(startRotation.eulerAngles, 0.2f));
        Destroy(gameObject);
        _gameManager.FreeSpawnPoint(spawnPoint);
        timer.AddTime((float)score);
    }
    
    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(RotateOverTime(startRotation.eulerAngles, 0.2f));
        Destroy(gameObject);
        _gameManager.FreeSpawnPoint(spawnPoint);
    }
    
    private void OnCollisionEnter(Collision others)
    {
        if (collisionActions.TryGetValue(gameObject.tag, out var action))
        {
            action(others);
        }
    }
    
    private void HandleCollision(int score, Collision collision)
    {
        AudioManager.Instance.PlaySFX("Hit");
        if (destroyCoroutine != null)
        {
            StopCoroutine(destroyCoroutine);
        }

        if (gameObject.CompareTag("TargetBall"))
        {
            StartCoroutine(RotateAndDestroyBall(score));
            Destroy(collision.gameObject);
        }
        else if (gameObject.CompareTag("TargetTime"))
        {
            StartCoroutine(RotateAndDestroyTimer(score));
            Destroy(collision.gameObject);
        }
        else
        {
            StartCoroutine(RotateAndDestroy(score));
            Destroy(collision.gameObject);  
        }
        
    }
    
}
