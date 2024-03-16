using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _enemies;
    [SerializeField] private List<GameObject> _eliteEnemies;
    [SerializeField] private float _spawnRate;
    [SerializeField] private float _eliteSpawnRate = 30f; // Rate at which to spawn elite enemies
    [SerializeField] private List<Transform> _spawnPoints;

    private void Start()
    {
        StartCoroutine(SpawnEnemiesCoroutine());
        StartCoroutine(SpawnEliteEnemiesCoroutine()); // Start the elite enemy spawning coroutine
    }

    private IEnumerator SpawnEnemiesCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnRate);
            SpawnEnemy(false); // Spawn a regular enemy
        }
    }

    private IEnumerator SpawnEliteEnemiesCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_eliteSpawnRate);
            SpawnEnemy(true); // Spawn an elite enemy
        }
    }

    private Vector3 SpawnPointSelect()
    {
        int randomIndex = Random.Range(0, _spawnPoints.Count);
        return _spawnPoints[randomIndex].position;
    }

    private void SpawnEnemy(bool isElite)
    {
        List<GameObject> pool = isElite ? _eliteEnemies : _enemies;
        int randomIndex = Random.Range(0, pool.Count);
        var enemy = pool[randomIndex];
        ObjectPoolManager.SpawnObject<NewEnemy>(enemy, SpawnPointSelect(), Quaternion.identity, ObjectPoolManager.PoolType.Enemy);
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            // Draw a sphere at each spawn point
            foreach (Transform spawnPoint in _spawnPoints)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(spawnPoint.position, 0.5f);
            }
        }
    }
}
