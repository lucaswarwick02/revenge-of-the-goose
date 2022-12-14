using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField] private GameObject farmhandBossPrefab;
    [Space]
    [SerializeField] private GameObject darkGroupPrefab;
    [SerializeField] private Vector3 darkGroupOffset = new Vector3(0, 0, 10);
    [Space]
    [SerializeField] private Transform choiceGateway;
    [SerializeField] private Vector3 choiceGatewayOffset = new Vector3(0, 0, 12.5f);
    [Space]
    [SerializeField] private Transform backBoundary;
    [SerializeField] private Vector3 boundaryDistanceFromPlayer = new Vector3(0, 0, -5);

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

        GameObject farmhandBoss = Instantiate(farmhandBossPrefab, transform.position, Quaternion.identity);
        farmhandBoss.transform.parent = GameObject.FindObjectOfType<MapArea>().transform;
        GameObject darkGroup = Instantiate(darkGroupPrefab, transform.position + darkGroupOffset, Quaternion.identity);

        choiceGateway.position = transform.position + choiceGatewayOffset;
        choiceGateway.gameObject.SetActive(true);

        // Move the back boundary to slightly behind the player, to block them moving backwards
        backBoundary.position = GameObject.FindGameObjectWithTag("Player").transform.position + boundaryDistanceFromPlayer;
    }
}
