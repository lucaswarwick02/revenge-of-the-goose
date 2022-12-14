using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossBar : MonoBehaviour
{
    private static BossBar INSTANCE;

    [SerializeField] private Slider slider;
    [SerializeField] private Transform player;
    [SerializeField] private TextMeshProUGUI bossName;

    private Transform destructable;

    private void Awake() {
        INSTANCE = this;
    }

    private void OnEnable() {
        GameHandler.OnNeutralModeChange += SetActive;
    }

    private void OnDisable() {
        GameHandler.OnNeutralModeChange -= SetActive;
    }

    private void Update() {
        if (destructable == null || slider.value <= 0f) {
            SetActive(false);
            return;
        }

        if (Vector3.Magnitude(player.position - destructable.position) <= 24f) {
            SetActive(true);
        }
        else {
            SetActive(false);
        }
    }

    public static void SetupSlider(float maxValue, Transform destructable, string bossName) {
        INSTANCE.slider.maxValue = maxValue;
        INSTANCE.slider.value = maxValue;
        INSTANCE.destructable = destructable;
        INSTANCE.bossName.text = bossName;
    }

    public static void UpdateHealthBar (float value) {
        INSTANCE.slider.value = value;
    }

    private void SetActive (bool active) {
        if (active && GameHandler.InNeutralMode) return;
        
        slider.gameObject.SetActive(active);
        bossName.gameObject.SetActive(active);
    }
}
