using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject enemyPrefabs;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private int manyEnemy;
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private List<WaypointSet> waypointSets = new List<WaypointSet>();
    [SerializeField] private bool isDebugOnScene;
    [SerializeField] GameObject gameplayInterface;
    [SerializeField] GameObject pauseInterface;

    public static Action<int> SpawnEnemy { get; private set; }
    private Enemy enemy;
    private ScoreManager scoreManager;
    private bool isPaused;

    private void Awake()
    {
        gameplayInterface.SetActive(true);
        pauseInterface.SetActive(false);
        isPaused = false;
        if (SpawnEnemy == null)
        {
            SpawnEnemy = SpawningEnemy;
        }
    }

    private void Start()
    {
        RandomSpawnEnemy();
        scoreManager = FindFirstObjectByType<ScoreManager>();
        scoreManager.SetOnPlayerWin(PlayerWin);
        player.SetOnPlayerLose(PlayerLose);
    }

    private void Update()
    {
        PressPause();
    }

    private void PlayerWin()
    {
        SavePlayerScore("Win");
        if (enemy != null) Destroy(enemy.gameObject);
        SceneManager.LoadScene("Game Over");
    }

    private void PlayerLose()
    {
        SavePlayerScore("Lose");
        if (enemy != null) Destroy(enemy);
        if (player != null) Destroy(player.gameObject);
        SceneManager.LoadScene("Game Over");
    }

    private void SavePlayerScore(string condition)
    {
        int score = scoreManager.GetScore();
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetString("Condition", condition);
        PlayerPrefs.Save();
    }

    private void RandomSpawnEnemy()
    {
        for (int i = 0; i < manyEnemy; i++)
        {
            if (i < waypointSets.Count) SpawningEnemy(i);
            if (i >= waypointSets.Count)
            {
                int set = Random.Range(0, waypointSets.Count);
                SpawningEnemy(set);
            }
        }
    }

    private void SpawningEnemy(int indexWaypointSets)
    {
        int index = Random.Range(0, enemySpawnPoints.Length);
        GameObject enemyGO = Instantiate(enemyPrefabs, enemySpawnPoints[index].position, Quaternion.identity, enemyParent);
        enemy = enemyGO.GetComponent<Enemy>();
        enemy.waypointSet = waypointSets[indexWaypointSets];
        enemy.player = player;

        if (isDebugOnScene)
        {
            enemyGO.AddComponent<DebugEnemy>().Init(enemy, player);
        }
    }

    private void PressPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PausingGame();
        }
    }

    public void PausingGame()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        AudioListener.pause = isPaused;

        gameplayInterface.SetActive(!isPaused);
        pauseInterface.SetActive(isPaused);

        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        SceneManager.LoadScene("Main Menu");
    }
}
