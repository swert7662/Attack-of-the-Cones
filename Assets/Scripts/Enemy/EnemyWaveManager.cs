using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] private List<EnemyWave> _enemyWaves;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private GameEvent _newWaveStart;

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

        coroutineRegularEnemies = StartCoroutine(SpawnEnemiesCoroutine(currentWave.spawnRate));
        if (currentWave._eliteEnemyList.Count > 0)
        {
            // Calculate the time interval between elite spawns
            float spawnInterval = currentWave.waveDuration / (currentWave._eliteEnemyList.Count + 1);
            coroutineEliteEnemies = StartCoroutine(SpawnEliteEnemiesCoroutine(spawnInterval, currentWave._eliteEnemyList.Count));
        }
    }

    private IEnumerator SpawnEnemiesCoroutine(float spawnRate)
    {
        while (true)
        {
            // Check if the wave's duration has ended
            if (Time.time - waveStartTime > currentWave.waveDuration)
            {
                // Try to transition to the next wave
                if (currentWaveIndex + 1 < _enemyWaves.Count)
                {
                    //If the next wave has enemy stats raise the start wave event with it, otherwise raise the event with the current wave
                    if (_enemyWaves[currentWaveIndex + 1].enemyWaveStats != null)
                    {
                        _newWaveStart.Raise(this, _enemyWaves[currentWaveIndex + 1].enemyWaveStats);
                    }
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
                SpawnEnemy(false);
            }
            else
            {
                // Wait for a bit before checking if we can spawn more enemies
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private IEnumerator SpawnEliteEnemiesCoroutine(float spawnInterval, int eliteCount)
    {
        for (int i = 0; i < eliteCount; i++)
        {
            // Wait for the spawn interval before spawning the next elite enemy
            yield return new WaitForSeconds(spawnInterval);
            SpawnEnemy(true); // Spawn an elite enemy
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
        ObjectPoolManager.SpawnObject<NewEnemy>(enemy, spawnPosition, Quaternion.identity, ObjectPoolManager.PoolType.Enemy);
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
