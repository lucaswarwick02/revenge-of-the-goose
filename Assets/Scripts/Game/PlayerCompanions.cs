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
        bunnyCompanion.SetActive(PlaythroughStats.IsBunnyCompanionUnlocked);
        sheepCompanion.SetActive(PlaythroughStats.IsSheepCompanionUnlocked);
    }
}
