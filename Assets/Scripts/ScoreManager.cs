using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;

    private static Action OnPlayerWin;
    private int maxScore;
    private int currentScore;

    public void Start()
    {
        currentScore = 0;
        maxScore = 0;
        UpdateScore();
    }

    public void SetOnPlayerWin(Action function)
    {
        OnPlayerWin += function;
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
            OnPlayerWin?.Invoke();
        }
    }

    private void UpdateScore()
    {
        scoreText.text = currentScore.ToString() + " / " + maxScore.ToString();
    }

    public int GetScore()
    {
        return currentScore;
    }
}
