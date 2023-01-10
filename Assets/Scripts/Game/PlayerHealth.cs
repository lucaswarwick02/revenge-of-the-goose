using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : MonoBehaviour
{
    private const float VIGNETTE_LERP_SPEED = 5f;

    public static PlayerHealth INSTANCE;

    public const float MAX_HEALTH = 100;

    public static event Action<float, float, Vector3> OnPlayerTakeDamage;

    public static event Action<Transform> OnPlayerDie;

    [SerializeField] private VolumeProfile volumeProfile;
    [SerializeField] private AnimationCurve vignetteIntensityCurve;

    private Vignette vignette;
    private float targetVignetteIntensity;

    public static float CurrentHealth { get; private set; }

    public static bool InflictDamage(float amount, Transform enemy)
    {
        CurrentHealth -= amount;
        OnPlayerTakeDamage?.Invoke(CurrentHealth, amount, enemy.position);

        INSTANCE.UpdateHealthVignette();

        if (CurrentHealth <= 0)
        {
            Die(enemy);
            return true;
        }

        return false;
    }

    private static void Die(Transform killer)
    {
        OnPlayerDie?.Invoke(killer);
    }

    public static void ResetHealth()
    {
        CurrentHealth = MAX_HEALTH;

        if (INSTANCE != null)
        {
            INSTANCE.UpdateHealthVignette();
        }
    }

    private void Awake()
    {
        INSTANCE = this;
        volumeProfile.TryGet(out vignette);

        GameHandler.OnMapAreaChanged += ResetHealth;
    }
    private void OnDisable()
    {
        GameHandler.OnMapAreaChanged -= ResetHealth;
    }

    private void Update()
    {
        var intensity = vignette.intensity;
        intensity.value = Mathf.Lerp(intensity.value, targetVignetteIntensity, VIGNETTE_LERP_SPEED * Time.unscaledDeltaTime);
        vignette.intensity = intensity;
    }

    public static void PickupHealth () {
        CurrentHealth = Mathf.Clamp(CurrentHealth + 20, 0, MAX_HEALTH);
        INSTANCE.UpdateHealthVignette();
    }

    public void UpdateHealthVignette ()
    {
        targetVignetteIntensity = vignetteIntensityCurve.Evaluate(1 - CurrentHealth / MAX_HEALTH);
    }
}
