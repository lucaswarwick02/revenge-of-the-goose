using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : MonoBehaviour
{
    [SerializeField] private float obstacleSpawnDistance;
    [SerializeField] private GameObject[] obstacles;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("InstantiateRandomObstacle", 3f, 3f);
    }

    private void InstantiateRandomObstacle () {
        InstantiateObstacle(GetRandomObstacle());
    }

    private void InstantiateObstacle (GameObject obstacle) {
        float horizontalOffset = Random.Range(-3f, 3f);
        Instantiate(obstacle, transform.position + new Vector3(horizontalOffset, 0f, obstacleSpawnDistance), Quaternion.identity);
    }

    private GameObject GetRandomObstacle () {
        return obstacles[Random.Range(0, obstacles.Length)];
    }
}
