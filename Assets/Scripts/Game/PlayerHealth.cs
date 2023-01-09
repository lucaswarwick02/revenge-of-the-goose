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

    public static event Action<Vector3> OnPlayerDie;

    [SerializeField] private VolumeProfile volumeProfile;
    [SerializeField] private AnimationCurve vignetteIntensityCurve;

    private Vignette vignette;
    private float targetVignetteIntensity;

    public static float CurrentHealth { get; private set; }

    public static bool InflictDamage(float amount, Vector3 origin)
    {
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
        volumeProfile.TryGet(out vignette);
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
