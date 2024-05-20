using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverMenu : MonoBehaviour
{
    private GameManager gameManager;
    //private int gOScore;
    public TextMeshProUGUI gOScoreText;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gOScoreText.text = "Final Score: " + GameManager.finalScore.ToString();
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync("Game");
    }
}
