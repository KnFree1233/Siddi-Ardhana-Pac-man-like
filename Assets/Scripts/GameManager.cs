using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text finishText;
    [SerializeField] private Enemy enemy;
    [SerializeField] private Player player;

    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
        scoreManager.SetOnPlayerLose(PlayerWin);
        player.SetOnPlayerLose(PlayerLose);

        finishText.enabled = false;
    }

    private void PlayerWin()
    {
        finishText.text = "You Win";
        finishText.enabled = true;
        if (enemy != null) Destroy(enemy.gameObject);
    }

    private void PlayerLose()
    {
        if (finishText != null)
        {
            finishText.text = "You Lose";
            finishText.enabled = true;
        }
        enemy?.audioSource.Stop();
        if (enemy != null) Destroy(enemy);
        if (player != null) Destroy(player.gameObject);
    }
}
