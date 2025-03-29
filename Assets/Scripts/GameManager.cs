using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject enemyPrefabs;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private int manyEnemy;
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private List<Transform> enemyWaypoints = new List<Transform>();

    private Enemy enemy;
    private ScoreManager scoreManager;
    private bool isDebugOnScene = true;

    private void Start()
    {
        RandomSpawnEnemy();
        scoreManager = FindFirstObjectByType<ScoreManager>();
        scoreManager.SetOnPlayerWin(PlayerWin);
        player.SetOnPlayerLose(PlayerLose);
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
            int index = Random.Range(0, enemySpawnPoints.Length);
            GameObject enemyGO = Instantiate(enemyPrefabs, enemySpawnPoints[index].position, Quaternion.identity, enemyParent);
            enemy = enemyGO.GetComponent<Enemy>();
            enemy.waypoints = enemyWaypoints;
            enemy.player = player;

            if (isDebugOnScene)
            {
                enemyGO.AddComponent<DebugEnemy>().Init(enemy, player);
            }
        }
    }
}
