using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PauseMenu pauseMenu;
    public bool isGameActive;
    private readonly float[] probabilities = {0.5f, 0.35f, 0.05f, 0.1f};
    private int score;
    public static int finalScore;
    public TextMeshProUGUI scoreText;
    private int NumberOfBalls;
    public TextMeshProUGUI NumberOfBallsText;
    private readonly float spawnInterval = 0.7f;
    public Transform[] spawnPoints;
    public GameObject[] targetPrefabs;
    private Dictionary<Transform, bool> spawnPointStatus = new Dictionary<Transform, bool>();
    
    void Start()
    { 
        pauseMenu = FindObjectOfType<PauseMenu>();
        NumberOfBalls = FindObjectOfType<BallPool>().PoolSize;
        isGameActive = true;
        UpdateScore(0);
        NumberOfBallsText.text = NumberOfBalls.ToString();
        foreach (var spawnPoint in spawnPoints)
        {
            spawnPointStatus[spawnPoint] = false;
        }
        StartCoroutine(SpawnTarget());
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && isGameActive)
        {
            pauseMenu.Pause();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isGameActive = false;
        }
    }

    IEnumerator SpawnTarget()
    {
        while (isGameActive) {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            if (spawnPointStatus[spawnPoint] == false)
            {
                float random = Random.value;
                int selectedIndex = -1;
                for (int i = 0; i < probabilities.Length; i++)
                {
                    random -= probabilities[i];
                    if (random <= 0)
                    {
                        selectedIndex = i;
                        break;
                    }
                }
                GameObject targetPrefab = targetPrefabs[selectedIndex];
                GameObject target = Instantiate(targetPrefab, spawnPoint.position, targetPrefab.transform.rotation);
                spawnPointStatus[spawnPoint] = true;
                target.GetComponent<Target>().SetSpawnPoint(spawnPoint);
                AudioManager.Instance.PlaySFX("Spawn");
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }
    public void UpdateNumberOfBall()
    {
        NumberOfBalls -= 1;
        NumberOfBallsText.text = NumberOfBalls.ToString();
        if (NumberOfBalls == 0)
        {
            StartCoroutine(DelayedGameOver(3f));
        }
    }
    public void AddBall(int numberOfBalls)
    {
        NumberOfBalls += numberOfBalls;
        NumberOfBallsText.text = NumberOfBalls.ToString();
    }
    public void GameOver()
    {
        isGameActive = false;
        StopCoroutine(SpawnTarget());
        finalScore = score;
        SceneManager.LoadSceneAsync("GameOver");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void FreeSpawnPoint(Transform spawnPoint)
    {
        spawnPointStatus[spawnPoint] = false;
    }
    
    IEnumerator DelayedGameOver(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (NumberOfBalls == 0)
        {
            GameOver(); 
        }
    }
}