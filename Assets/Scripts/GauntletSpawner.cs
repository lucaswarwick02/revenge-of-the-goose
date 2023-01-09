using UnityEngine;

public class GauntletSpawner : MonoBehaviour
{
    [SerializeField] private GauntletAttacker attackerPrefab;
    [SerializeField] private float spawnInterval;
    [SerializeField] private float startDelay;

    private float nextSpawnTime;

    private void Awake()
    {
        nextSpawnTime = Time.time + startDelay;
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnInterval;
            SpawnAttacker();
        }
    }

    private void SpawnAttacker()
    {
        Instantiate(attackerPrefab, transform.position, Quaternion.identity, transform);
    }
}
