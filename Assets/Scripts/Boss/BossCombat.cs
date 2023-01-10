using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private float attackInterval = 4;
    [SerializeField] private float startAttackDelay = 1.5f;
    [SerializeField] private float attackDamage = 20;

    [Header("Object Spawning")]
    [SerializeField] private float maxObstacleDelay = 1.5f;
    [SerializeField] private float minObstacleDelay = 3f;
    [SerializeField] private float obstacleSpawnDistance;
    [SerializeField] private GameObject[] obstacles;

    Destructible destructible;
    BossManager bossManager;

    Transform player;
    Animator anim;
    AudioSource audioSource;

    private float timer;
    private float maxHitPoints;
    private float nextAttackTime;

    public bool canSpawnObstacles = true;

    private void Awake() {
        destructible = GetComponent<Destructible>();
        bossManager = GetComponent<BossManager>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start() {
        maxHitPoints = destructible.getHitPoints();
        ResetTimer();
        nextAttackTime = Time.time + startAttackDelay;
    }

    private void Update()
    {
        if (GameHandler.InNeutralMode) return;

        timer -= Time.deltaTime;
        if (timer < 0) {
            InstantiateRandomObstacle();
            ResetTimer();
        }

        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackInterval;
            AttackPlayer();
        }
    }

    private void ResetTimer () {
        timer = Mathf.Lerp(maxObstacleDelay, minObstacleDelay, destructible.getHitPoints() / maxHitPoints);
    }

    private void InstantiateRandomObstacle () {
        InstantiateObstacle(GetRandomObstacle());
    }

    private void AttackPlayer()
    {
        audioSource.Play();
        anim.SetTrigger("Attack");
        PlayerHealth.InflictDamage(attackDamage, transform);
    }

    private void InstantiateObstacle (GameObject obstacle) {
        if (!canSpawnObstacles) return;
        
        Instantiate(obstacle, transform.position + new Vector3(player.position.x, 0f, obstacleSpawnDistance), Quaternion.identity);
    }

    private GameObject GetRandomObstacle () {
        return obstacles[Random.Range(0, obstacles.Length)];
    }
}
