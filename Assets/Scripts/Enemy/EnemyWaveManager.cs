using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] private List<EnemyWave> _enemyWaves;
    [SerializeField] private List<Transform> _spawnPoints;

    private int currentWaveIndex = 0;
    private EnemyWave currentWave => _enemyWaves[currentWaveIndex];
    private float waveStartTime;
    private int enemiesSpawned = 0;

    private Coroutine coroutineRegularEnemies;
    private Coroutine coroutineEliteEnemies;
    private void Start()
    {
        StartWave(currentWaveIndex);
    }

    private void StartWave(int index)
    {
        // Stop previous wave's coroutines if they are running
        if (coroutineRegularEnemies != null) StopCoroutine(coroutineRegularEnemies);
        if (coroutineEliteEnemies != null) StopCoroutine(coroutineEliteEnemies);

        currentWaveIndex = index;
        waveStartTime = Time.time;
        enemiesSpawned = 0; // Reset the count for the new wave

        coroutineRegularEnemies = StartCoroutine(SpawnEnemiesCoroutine(currentWave.spawnRate, false));
        if (currentWave._eliteEnemyList.Count > 0)
        {
            coroutineEliteEnemies = StartCoroutine(SpawnEnemiesCoroutine(currentWave.spawnRate, true)); // Ensure you're using the correct spawn rate here
        }
    }

    private IEnumerator SpawnEnemiesCoroutine(float spawnRate, bool isElite)
    {
        while (true)
        {
            // Check if the wave's duration has ended
            if (Time.time - waveStartTime > currentWave.waveDuration)
            {
                // Try to transition to the next wave
                if (currentWaveIndex + 1 < _enemyWaves.Count)
                {
                    StartWave(currentWaveIndex + 1);
                }
                else
                {
                    Debug.Log("No new wave available. Continuing with the current wave.");
                    waveStartTime = Time.time; // Reset wave 
                }
            }

            if (enemiesSpawned < currentWave.enemyMaxCount)
            {
                yield return new WaitForSeconds(1f / spawnRate);
                SpawnEnemy(isElite);
            }
            else
            {
                // Wait for a bit before checking if we can spawn more enemies
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private void SpawnEnemy(bool isElite)
    {
        List<GameObject> enemyPool = isElite ? currentWave._eliteEnemyList : currentWave._enemyList;
        if (enemyPool.Count == 0)
        {
            Debug.LogWarning("Enemy pool is empty.");
            return;
        }

        int randomIndex = Random.Range(0, enemyPool.Count);
        GameObject enemy = enemyPool[randomIndex];
        Vector3 spawnPosition = SpawnPointSelect();
        ObjectPoolManager.SpawnObject(enemy, spawnPosition, Quaternion.identity, ObjectPoolManager.PoolType.Enemy);
        enemiesSpawned++;
    }

    private Vector3 SpawnPointSelect()
    {
        int randomIndex = Random.Range(0, _spawnPoints.Count);
        return _spawnPoints[randomIndex].position;
    }

    public void ReduceEnemyCount()
    {
        enemiesSpawned--;
    }
}
