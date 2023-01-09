using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompanions : MonoBehaviour
{
    [SerializeField] private Transform bunnySpot;
    [SerializeField] private Transform sheepSpot;

    private static PlayerCompanions _instance;

    private void Awake() {
        _instance = this;
    }

    public static Transform GetBunnySpot () {
        return _instance.bunnySpot;
    }

    public static Transform GetSheepSpot () {
        return _instance.sheepSpot;
    }
}
