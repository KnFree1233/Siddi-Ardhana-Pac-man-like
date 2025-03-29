using UnityEngine;
using System;

public class Pickable : MonoBehaviour
{
    [SerializeField] public PickableType pickableType;
    [SerializeField] private float rotationSpeed;

    public Action<Pickable> OnPicked;

    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (OnPicked != null)
            {
                OnPicked(this);
            }

            Destroy(gameObject);
        }
    }
}
