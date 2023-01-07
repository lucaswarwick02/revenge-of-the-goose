using UnityEngine;
using UnityEngine.Events;

public class Destructible : MonoBehaviour
{
    [SerializeField] private string destructibleName;
    [SerializeField] private DestructableType type;
    [SerializeField] private float hitPoints = 100;
    [SerializeField] private int destructionScore;
    [Space]
    [SerializeField] private bool destroyImmediately = true;

    public UnityEvent<RaycastHit, Vector3> OnDestroyed;
    public UnityEvent<RaycastHit> OnHit;

    public bool IsDestroyed { get; private set; }

    private void Start() {
        if (type == DestructableType.Boss) {
            BossBar.SetupSlider(hitPoints, transform, destructibleName);
            OnHit.AddListener(_ => BossBar.UpdateHealthBar(hitPoints) );
        }

        if (type == DestructableType.Friendly) {
            PlaythroughStats.IncrementAnimalsEncountered();
        }

        if (type == DestructableType.Enemy) {
            PlaythroughStats.IncrementEnemiesEncountered();
        }
    }

    private enum DestructableType
    {
        Obstacle,
        Enemy,
        Friendly,
        Boss,
    }

    public bool InflictDamage(float amount, RaycastHit hitInfo, Vector3 origin)
    {
        hitPoints -= amount;
        OnHit?.Invoke(hitInfo);

        if (hitPoints <= 0 && !IsDestroyed)
        {
            BeginDestruction(hitInfo, origin);
            return true;
        }

        return false;
    }

    private void BeginDestruction(RaycastHit hitInfo, Vector3 origin)
    {
        IsDestroyed = true;

        if (type == DestructableType.Enemy)
        {
            PlaythroughStats.IncrementEnemyKillCount(transform.position);
        }
        else if (type == DestructableType.Friendly)
        {
            PlaythroughStats.IncrementAnimalKillCount(transform.position);
        }

        PlaythroughStats.AddDestructionScore(destructionScore, transform.position);

        OnDestroyed?.Invoke(hitInfo, origin);

        if (destroyImmediately)
        {
            Destroy(gameObject);
        }
    }
}
