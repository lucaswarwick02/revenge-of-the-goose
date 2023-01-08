using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompanions : MonoBehaviour
{
    [SerializeField] private GameObject bunnyCompanion;
    [SerializeField] private GameObject sheepCompanion;

    // Start is called before the first frame update
    void Start()
    {
        GameHandler.OnMapAreaChanged += UpdateCompanions;
    }

    private void OnDisable()
    {
        GameHandler.OnMapAreaChanged -= UpdateCompanions;
    }

    private void UpdateCompanions () {
        bunnyCompanion.SetActive(PlaythroughStats.IsBunnyCompanionUnlocked);
        sheepCompanion.SetActive(PlaythroughStats.IsSheepCompanionUnlocked);
    }
}
