using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _enemies;
    [SerializeField] private float _spawnRate;
    [SerializeField] private List<Transform> _spawnPoints;

    private Vector3 _nextSpawnPoint;
    private float _spawnRadius;
    private float _nextSpawnAngle;

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

    private Vector3 SpawnPointSelect()
    {
        int randomIndex = Random.Range(0, _spawnPoints.Count);
        return _spawnPoints[randomIndex].position;
    }

    private void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, _enemies.Count);
        var enemy = _enemies[randomIndex];
        ObjectPoolManager.SpawnObject(enemy, SpawnPointSelect(), Quaternion.identity, ObjectPoolManager.PoolType.Enemy);
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

    #region Old Code
    private float CalculateNextSpawnAngle()
    {
        return Random.Range(0f, 2f * Mathf.PI);
    }

    private Vector3 CalculateNextSpawnPoint()
    {
        return transform.position + new Vector3(_spawnRadius * Mathf.Cos(_nextSpawnAngle), _spawnRadius * Mathf.Sin(_nextSpawnAngle), 0f);
    }
    #endregion

}
