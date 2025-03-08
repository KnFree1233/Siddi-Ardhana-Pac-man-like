using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;

    private static Action OnPlayerLose;
    private int maxScore;
    private int currentScore;

    public void Start()
    {
        currentScore = 0;
        maxScore = 0;
        UpdateScore();
    }

    public void SetOnPlayerLose(Action function)
    {
        OnPlayerLose += function;
    }

    public void SetMaxScore(int maxScore)
    {
        this.maxScore = maxScore;
        UpdateScore();
    }

    public void AddScore(int score)
    {
        currentScore += score;
        UpdateScore();
        if (currentScore == maxScore)
        {
            OnPlayerLose?.Invoke();
        }
    }

    private void UpdateScore()
    {
        scoreText.text = currentScore.ToString() + " / " + maxScore.ToString();
    }
}
