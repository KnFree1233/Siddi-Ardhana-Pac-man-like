using UnityEngine;
using System;

public abstract class Pickable : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    public PickableType type { get; protected set; }
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
                TriggerEffect();
                OnPicked(this);
            }

            Destroy(gameObject);
        }
    }

    protected abstract void TriggerEffect();
    public abstract void Initialize(); 
}
