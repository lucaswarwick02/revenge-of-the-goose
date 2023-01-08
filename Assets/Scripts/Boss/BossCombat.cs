using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : MonoBehaviour
{
    [SerializeField] private float maxObstacleDelay = 1.5f;
    [SerializeField] private float minObstacleDelay = 3f;
    [SerializeField] private float obstacleSpawnDistance;
    [SerializeField] private Obstacle[] obstacles;

    Destructible destructible;
    BossManager bossManager;

    private float timer;
    private float maxHitPoints;

    private void Awake() {
        destructible = GetComponent<Destructible>();
        bossManager = GetComponent<BossManager>();
    }

    private void Start() {
        maxHitPoints = destructible.getHitPoints();
        ResetTimer();
    }

    private void Update()
    {
        if (!bossManager.CombatEnabled) return;
        
        timer -= Time.deltaTime;
        if (timer < 0) {
            InstantiateRandomObstacle();
            ResetTimer();
        }
    }

    private void ResetTimer () {
        timer = Mathf.Lerp(maxObstacleDelay, minObstacleDelay, destructible.getHitPoints() / maxHitPoints);
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
