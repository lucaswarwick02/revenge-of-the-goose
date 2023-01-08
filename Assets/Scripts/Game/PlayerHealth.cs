using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : MonoBehaviour
{
    public const float MAX_HEALTH = 100;
    public const float DELAY_BEFORE_HEALTH_GAIN = 3;
    public const float HEALTH_GAIN_PER_SECOND = 5;

    public static event Action<float, float, Vector3> OnPlayerTakeDamage;

    public static event Action<Vector3> OnPlayerDie;

    private static float canHealTime;

    [SerializeField] private VolumeProfile volumeProfile;

    public static float CurrentHealth { get; private set; }

    public static PlayerHealth INSTANCE;

    public static bool InflictDamage(float amount, Vector3 origin)
    {
        canHealTime = Time.time + DELAY_BEFORE_HEALTH_GAIN;
        CurrentHealth -= amount;
        OnPlayerTakeDamage?.Invoke(CurrentHealth, amount, origin);

        INSTANCE.UpdateHealthVignette();

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
        INSTANCE = this;
    }

    private void Update()
    {
        if (Time.time > canHealTime)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + HEALTH_GAIN_PER_SECOND * Time.deltaTime, 0, MAX_HEALTH);
            UpdateHealthVignette();
        }
    }

    private void UpdateHealthVignette () {
        float percentage = CurrentHealth / MAX_HEALTH;

        Vignette vignette;
        volumeProfile.TryGet(out vignette);
        ClampedFloatParameter intensity = vignette.intensity;
        intensity.value = 1f - percentage;
        vignette.intensity = intensity;
    }
}
