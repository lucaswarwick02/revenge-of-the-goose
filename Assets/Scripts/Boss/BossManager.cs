using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField] private GameObject farmhandBossPrefab;
    [SerializeField] private GameObject darkGroupPrefab;
    [SerializeField] private Vector3 darkGroupOffset = new Vector3(0, 0, 10);

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

        // Spawn in the dying farmer
        Instantiate(farmhandBossPrefab, transform.position, Quaternion.identity);

        // Spawn in the dark group behind the farmer
        Instantiate(darkGroupPrefab, transform.position + darkGroupOffset, Quaternion.identity);
    }
}
