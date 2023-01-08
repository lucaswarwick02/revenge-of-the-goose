using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : MonoBehaviour
{
    [SerializeField] private float maxObstacleDelay = 1.5f;
    [SerializeField] private float minObstacleDelay = 3f;
    [SerializeField] private float obstacleSpawnDistance;
    [SerializeField] private GameObject[] obstacles;

    Destructible destructible;
    BossManager bossManager;

    Transform player;

    private float timer;
    private float maxHitPoints;

    private void Awake() {
        destructible = GetComponent<Destructible>();
        bossManager = GetComponent<BossManager>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start() {
        maxHitPoints = destructible.getHitPoints();
        ResetTimer();
    }

    private void Update()
    {
        if (GameHandler.InNeutralMode) return;

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

    private void InstantiateObstacle (GameObject obstacle) {
        Instantiate(obstacle, transform.position + new Vector3(player.position.x, 0f, obstacleSpawnDistance), Quaternion.identity);
    }

    private GameObject GetRandomObstacle () {
        return obstacles[Random.Range(0, obstacles.Length)];
    }
}
