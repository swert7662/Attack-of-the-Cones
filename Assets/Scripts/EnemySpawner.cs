using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Enemy> _enemies;
    [SerializeField] private float _spawnRate;
    [SerializeField] private float _spawnRadius;
    private Vector3 _nextSpawnPoint;
    private float _nextSpawnAngle;

    private void Start()
    {
        StartCoroutine(SpawnEnemiesCoroutine());
    }

    private IEnumerator SpawnEnemiesCoroutine()
    {
        while (true)
        {
            _nextSpawnAngle = CalculateNextSpawnAngle();
            yield return new WaitForSeconds(_spawnRate);
            SpawnEnemy();
        }
    }

    private float CalculateNextSpawnAngle()
    {
        return Random.Range(0f, 2f * Mathf.PI);
    }

    private Vector3 CalculateNextSpawnPoint()
    {
        return transform.position + new Vector3(_spawnRadius * Mathf.Cos(_nextSpawnAngle), _spawnRadius * Mathf.Sin(_nextSpawnAngle), 0f);
    }

    private void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, _enemies.Count);
        var enemy = _enemies[randomIndex];
        ObjectPoolManager.SpawnObject(enemy.gameObject, CalculateNextSpawnPoint(), Quaternion.identity, ObjectPoolManager.PoolType.Enemy);
    }

    private void OnDrawGizmos()
    {
        // Draw the spawn radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _spawnRadius);

        // Draw the next spawn point
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(CalculateNextSpawnPoint(), 0.5f); // Adjust the size as needed
    }
}
