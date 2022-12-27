using System;
using UnityEngine;
using UnityEngine.Events;

public class Destructible : MonoBehaviour
{
    [SerializeField] private float hitPoints = 100;
    [Space]
    public UnityEvent OnDestroyed;
    public UnityEvent<RaycastHit> OnHit;

    public bool InflictDamage(float amount, RaycastHit hitInfo)
    {
        hitPoints -= amount;
        OnHit.Invoke(hitInfo);

        if (hitPoints <= 0)
        {
            Destroy();
            return true;
        }

        return false;
    }

    private void Destroy()
    {
        OnDestroyed.Invoke();
        Destroy(gameObject);
    }
}
