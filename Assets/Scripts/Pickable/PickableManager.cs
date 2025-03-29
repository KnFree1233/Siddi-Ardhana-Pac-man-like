using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickableManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] AudioSource pickupCoinSFX;

    private List<Pickable> pickableList = new List<Pickable>();

    void Start()
    {
        InitPickableList();
    }

    private void InitPickableList()
    {
        Pickable[] pickables = FindObjectsOfType<Pickable>();

        for (int i = 0; i < pickables.Length; i++)
        {
            pickables[i].OnPicked += OnPickablePicked;
            pickableList.Add(pickables[i]);
        }
        scoreManager?.SetMaxScore(pickableList.Count);
    }

    private void OnPickablePicked(Pickable pickable)
    {
        if (pickable.pickableType == PickableType.Coin)
        {
            pickupCoinSFX.Play();
        }
        else if (pickable.pickableType == PickableType.PowerUp)
        {
            player?.PickPowerUp();
        }
        scoreManager?.AddScore(1);
        pickableList.Remove(pickable);
    }
}
