using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallThrower : MonoBehaviour
{
    private GameManager gameManager;
    public Transform cam;
    public Transform attackPoint;
    public GameObject ball;
    public BallPool ballPool;

    [Header("Throwing")] public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardForce;

    bool readyToThrow;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(throwKey) && gameManager.isGameActive == true)
        {
            AudioManager.Instance.PlaySFX("Throw");
            Vector3 forceDirection = cam.transform.forward;
            RaycastHit hit;
            GameObject ball = ballPool.GetBall();
            if (ball != null)
            {
                if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
                {
                    forceDirection = (hit.point - attackPoint.position).normalized;
                }
                ball.transform.position = attackPoint.position;
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                rb.velocity = forceDirection * throwForce + Vector3.up * throwUpwardForce;
               
                gameManager.UpdateNumberOfBall();
                    
                Destroy(ball, 5f);
            }
            else
            {
                GameOverAfterDelay(1);
            }
        }
    }
    
    IEnumerator GameOverAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (ball != null)
        {
            gameManager.GameOver(); 
        }
        else
        {
            yield break;
        }
    }

}