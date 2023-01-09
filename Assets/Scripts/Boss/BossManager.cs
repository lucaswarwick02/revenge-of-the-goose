using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField] private GameObject farmhandBossPrefab;

    private void OnEnable() {
        GetComponent<Destructible>().OnDestroyed.AddListener(CreateFarmhandBoss);
    }

    private void OnDisable() {
        GetComponent<Destructible>().OnDestroyed.RemoveListener(CreateFarmhandBoss);
    }

    private void CreateFarmhandBoss (RaycastHit hit, Vector3 vec) {
        // Remove all Farmhands
        foreach(FarmhandController farmhand in GameObject.FindObjectsOfType<FarmhandController>()) {
            Destroy(farmhand.gameObject);
        }
        // Remove all Dogs
        foreach(DogController dog in GameObject.FindObjectsOfType<DogController>()) {
            Destroy(dog.gameObject);
        }

        Instantiate(farmhandBossPrefab, transform.position, Quaternion.identity);
    }
}
