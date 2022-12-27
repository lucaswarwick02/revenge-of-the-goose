using System;
using UnityEngine;

public class Killable : MonoBehaviour
{
    public event Action OnHit;

    public event Action OnKilled;

    [SerializeField] private float hitPoints = 100;

    public bool InflictDamage(float amount)
    {
        hitPoints -= amount;

        if (hitPoints <= 0)
        {
            Kill();
            return true;
        }

        return false;
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}
