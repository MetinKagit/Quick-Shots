using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPool : MonoBehaviour
{
    
    public GameObject ballPrefab;
    private GameManager gameManager;
    private int poolSize = 17;

    public int PoolSize => poolSize;

    private Queue<GameObject> balls;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        balls = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject ball = Instantiate(ballPrefab);
            ball.SetActive(false);
            balls.Enqueue(ball);
        }
    }

    public GameObject GetBall()
    {
        if (balls.Count > 0)
        {
            GameObject ball = balls.Dequeue();
            ball.SetActive(true);
            return ball;
        }
        return null;
    }

    public void ReturnBall(GameObject ball)
    {
        ball.SetActive(false);
        balls.Enqueue(ball);
    }

    public void AddBalls(int numberOfBalls)
    {
        for (int i = 0; i < numberOfBalls; i++)
        {
            GameObject ball = Instantiate(ballPrefab);
            ball.SetActive(false);
            balls.Enqueue(ball);
        }
        poolSize += numberOfBalls;
        gameManager.AddBall(numberOfBalls);
    }
}

