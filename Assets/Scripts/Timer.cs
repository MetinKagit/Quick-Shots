using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float RemainingTime = 45f;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive == true)
        {
            if (RemainingTime > 0)
            {
                RemainingTime -= Time.deltaTime;
            }
            else
            {
                RemainingTime = 0;
                timerText.color = Color.red;
                StartCoroutine(GameOverAfterDelay(2f));
            
            }
            int minutes = Mathf.FloorToInt(RemainingTime / 60);
            int seconds = Mathf.FloorToInt(RemainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        
    }
    
    IEnumerator GameOverAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameManager.GameOver();
    }
    
    public void AddTime(float time)
    {
        RemainingTime += time;
    }
}
