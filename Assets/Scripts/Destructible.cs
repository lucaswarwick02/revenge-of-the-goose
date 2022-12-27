using System;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public event Action OnDestroyed;

    [SerializeField] private float hitPoints = 100;

    public bool InflictDamage(float amount)
    {
        hitPoints -= amount;

        if (hitPoints <= 0)
        {
            Destroy();
            return true;
        }

        return false;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
