using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    private static BossBar INSTANCE;

    [SerializeField] private Slider slider;
    [SerializeField] private Transform player;

    public Transform destructable;

    private void Awake() {
        INSTANCE = this;
    }

    private void Update() {
        if (destructable == null || slider.value <= 0f) {
            HideBossBar();
            return;
        }

        if (Vector3.Magnitude(player.position - destructable.position) <= 12f) {
            ShowBossBar();
        }
        else {
            HideBossBar();
        }
    }

    public static void SetupSlider(float maxValue, Transform destructable) {
        INSTANCE.slider.maxValue = maxValue;
        INSTANCE.slider.value = maxValue;
        INSTANCE.destructable = destructable;
    }

    public static void UpdateHealthBar (float value) {
        INSTANCE.slider.value = value;
    }

    private void HideBossBar () {
        slider.gameObject.SetActive(false);
    }

    private void ShowBossBar () {
        slider.gameObject.SetActive(true);
    }
}
