using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompanions : MonoBehaviour
{
    public static PlayerCompanions INSTANCE;

    public Transform bunnySpot;
    
    public GameObject bunnyCompanion;

    private void Awake() {
        INSTANCE = this;
    }

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
        if (bunnyCompanion) bunnyCompanion.transform.position = bunnySpot.position;
    }
}
