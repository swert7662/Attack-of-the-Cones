using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Wave", menuName = "Enemy Wave/ New Enemy Wave", order = 1)]
public class EnemyWave : ScriptableObject
{
    public int enemyMaxCount;
    public float spawnRate;
    public float waveDuration;
    public EnemyStats enemyWaveStats;

    public List<GameObject> _enemyList;
    public List<GameObject> _eliteEnemyList;
}
