using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompanions : MonoBehaviour
{
    [SerializeField] private GameObject rabbitCompanion;
    [SerializeField] private GameObject sheepCompanion;

    // Start is called before the first frame update
    void Start()
    {
        rabbitCompanion.SetActive(PlaythroughStats.IsRabbitCompanionUnlocked());
        sheepCompanion.SetActive(PlaythroughStats.IsSheepCompanionUnlocked());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
