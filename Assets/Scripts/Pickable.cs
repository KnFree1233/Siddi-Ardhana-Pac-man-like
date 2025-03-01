using UnityEngine;
using System;

public class Pickable : MonoBehaviour
{
    [SerializeField] public PickableType pickableType;

    public Action<Pickable> OnPicked;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Pickup: " + pickableType);

            if (OnPicked != null)
            {
                OnPicked(this);
            }

            Destroy(gameObject);
        }
    }
}
