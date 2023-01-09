using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompanions : MonoBehaviour
{
    private static PlayerCompanions _instance;

    private static GameObject bunnyCompanionInstance;
    private static GameObject sheepCompanionInstance;

    [SerializeField] private Transform bunnySpot;
    [SerializeField] private Transform sheepSpot;

    [SerializeField] private RabbitController bunnyCompanionPrefab;
    [SerializeField] private SheepController sheepCompanionPrefab;

    private void Awake()
    {
        _instance = this;
        bunnyCompanionInstance = null;
        sheepCompanionInstance = null;
        GameHandler.OnGameLoaded += UpdateCompanionsOnGameLoad;
    }

    private void OnDisable()
    {
        GameHandler.OnGameLoaded -= UpdateCompanionsOnGameLoad;
    }

    public static Transform RegisterAsBunnyCompanion(GameObject companionObject)
    {
        Debug.Log("New Bunny Companion registered!");
        bunnyCompanionInstance = companionObject;
        return _instance.bunnySpot;
    }

    public static Transform RegisterAsSheepCompanion(GameObject companionObject)
    {
        Debug.Log("New Sheep Companion registered!");
        sheepCompanionInstance = companionObject;
        return _instance.sheepSpot;
    }

    private void UpdateCompanionsOnGameLoad()
    {
        if (bunnyCompanionInstance != null)
        {
            Destroy(bunnyCompanionInstance);
            bunnyCompanionInstance = null;
        }

        if (sheepCompanionInstance != null)
        {
            Destroy(sheepCompanionInstance);
            sheepCompanionInstance = null;
        }

        if (PlaythroughStats.IsBunnyCompanionUnlocked)
        {
            var newRabbit = Instantiate(bunnyCompanionPrefab, bunnySpot.position + Vector3.back * 3, Quaternion.identity);
            newRabbit.MakeCompanion();
        }

        if (PlaythroughStats.IsSheepCompanionUnlocked)
        {
            var newSheep = Instantiate(bunnyCompanionPrefab, sheepSpot.position + Vector3.back * 3, Quaternion.identity);
            newSheep.MakeCompanion();
        }
    }
}
