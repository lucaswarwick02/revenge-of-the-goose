using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public const float MAX_HEALTH = 100;
    public const float DELAY_BEFORE_HEALTH_GAIN = 3;
    public const float HEALTH_GAIN_PER_SECOND = 5;

    public static event Action<float, float, Vector3> OnPlayerTakeDamage;

    public static event Action<Vector3> OnPlayerDie;

    private static float canHealTime;

    public static float CurrentHealth { get; private set; }

    public static bool InflictDamage(float amount, Vector3 origin)
    {
        canHealTime = Time.time + DELAY_BEFORE_HEALTH_GAIN;
        CurrentHealth -= amount;
        OnPlayerTakeDamage?.Invoke(CurrentHealth, amount, origin);

        if (CurrentHealth <= 0)
        {
            Die(origin);
            return true;
        }

        return false;
    }

    private static void Die(Vector3 origin)
    {
        OnPlayerDie?.Invoke(origin);
    }

    private void Awake()
    {
        CurrentHealth = MAX_HEALTH;
    }

    private void Update()
    {
        if (Time.time > canHealTime)
        {
            CurrentHealth += HEALTH_GAIN_PER_SECOND * Time.deltaTime;
        }
    }
}
