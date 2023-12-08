using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemyPrefabs; // Array to hold enemy types
    [SerializeField] private float _spawnRate; // Time in seconds between spawns
    [SerializeField] private float _spawnMax;

    private void Start()
    {
        StartCoroutine(SpawnEnemiesCoroutine());
    }

    private IEnumerator SpawnEnemiesCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnRate);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, _enemyPrefabs.Length); // Choose a random enemy prefab
        GameObject enemyToSpawn = _enemyPrefabs[randomIndex];

        if (enemyToSpawn != null)
        {
            Instantiate(enemyToSpawn, transform.position, Quaternion.identity); // Spawn the enemy at the spawner's position
        }
    }
}
