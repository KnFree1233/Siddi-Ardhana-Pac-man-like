using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickableManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Enemy enemy;
    [SerializeField] private TMP_Text coinsLeftText;
    [SerializeField] private TMP_Text winText;

    private List<Pickable> pickableList = new List<Pickable>();

    void Start()
    {
        winText.enabled = false;
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
        coinsLeftText.text = pickableList.Count + " coins left";
        Debug.Log(pickableList.Count);
    }

    private void OnPickablePicked(Pickable pickable)
    {
        if (pickable.pickableType == PickableType.PowerUp)
        {
            player?.PickPowerUp();
        }
        pickableList.Remove(pickable);
        coinsLeftText.text = pickableList.Count + " coins left";
        if (pickableList.Count <= 0)
        {
            Destroy(enemy.gameObject);
            winText.enabled = true;
            Debug.Log("You Win!");
        }
    }
}
