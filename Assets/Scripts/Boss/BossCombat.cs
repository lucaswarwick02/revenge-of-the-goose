using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : MonoBehaviour
{
    [SerializeField] private float obstacleDelay = 2f;
    [SerializeField] private float obstacleSpawnDistance;
    [SerializeField] private Obstacle[] obstacles;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("InstantiateRandomObstacle", obstacleDelay, obstacleDelay);
    }

    private void InstantiateRandomObstacle () {
        InstantiateObstacle(GetRandomObstacle());
    }

    private void InstantiateObstacle (Obstacle obstacle) {
        float horizontalOffset = Random.Range(-obstacle.maxOffset, obstacle.maxOffset);
        Instantiate(obstacle.prefab, transform.position + new Vector3(horizontalOffset, 0f, obstacleSpawnDistance), Quaternion.identity);
    }

    private Obstacle GetRandomObstacle () {
        return obstacles[Random.Range(0, obstacles.Length)];
    }

    [System.Serializable]
    public struct Obstacle
    {
        public GameObject prefab;
        public float maxOffset;
    }
}
