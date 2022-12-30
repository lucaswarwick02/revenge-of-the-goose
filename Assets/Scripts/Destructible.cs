using System;
using UnityEngine;
using UnityEngine.Events;

public class Destructible : MonoBehaviour
{
    [SerializeField] private DestructableType type;
    [SerializeField] private float hitPoints = 100;
    [SerializeField] private int destructionScore;
    [Space]
    public UnityEvent OnDestroyed;
    public UnityEvent<RaycastHit> OnHit;

    private enum DestructableType
    {
        Obstacle,
        Enemy,
        Friendly,
    }

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
        if (type == DestructableType.Enemy)
        {
            PlaythroughStats.IncrementEnemyKillCount(transform.position);
        }
        else if (type == DestructableType.Friendly)
        {
            PlaythroughStats.IncrementFriendlyKillCount(transform.position);
        }

        PlaythroughStats.AddDestructionScore(destructionScore, transform.position);

        OnDestroyed.Invoke();
        Destroy(gameObject);
    }
}
