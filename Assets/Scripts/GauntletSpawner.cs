using UnityEngine;

public class GauntletSpawner : MonoBehaviour
{
    [SerializeField] private GauntletAttacker attackerPrefab;
    [SerializeField] private float spawnInterval;

    private float nextSpawnTime;

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
        Instantiate(attackerPrefab, transform.position, transform.rotation);
    }
}
