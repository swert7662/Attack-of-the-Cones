using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    #region Pooled Objects

    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Enemy[] _enemyPrefabs; // Array to hold enemy types

    private ObjectPool<Projectile> _projectilePool;
    private ObjectPool<Enemy> _enemyPool;

    #endregion

    [SerializeField] private int _projectilePoolAmount;
    [SerializeField] private bool _usePool = true;

    public static PoolManager Instance { get; private set; }
    private GameObject _projectilePoolContainer;
    private GameObject _enemyPoolContainer;


    void Awake()
    {
        Instance = this;

        InitializeProjectilePool();
        InitializeEnemyPool();
    }

    

    #region Projectile Pool Methods
    private void InitializeProjectilePool()
    {
        _projectilePoolContainer = new GameObject($"PoolContainer - {_projectilePrefab.name}");
        _projectilePoolContainer.transform.SetParent(transform);

        _projectilePool = new ObjectPool<Projectile>(
            CreateProjectile,
            projectile => {
                projectile.gameObject.SetActive(true);
            }, projectile => {
                projectile.gameObject.SetActive(false); 
            }, projectile => {
                Destroy(projectile.gameObject); Debug.Log("Destroying Projectile."); 
            }, false, 10, 200);
    }

    private Projectile CreateProjectile()
    {
        Projectile projectileInstance = Instantiate(_projectilePrefab);
        projectileInstance.transform.SetParent(_projectilePoolContainer.transform);
        projectileInstance.gameObject.SetActive(false);
        return projectileInstance;
    }
    private void KillProjectile(Projectile projectile)
    {
        if (_usePool) { _projectilePool.Release(projectile); }
        else { Destroy(projectile.gameObject); }
    }

    public Projectile GetProjectile()
    {
        var projectile = _usePool ? _projectilePool.Get() : CreateProjectile();
        projectile.Init(KillProjectile);
        return projectile;
    }

    #endregion

    #region Enemy Pool Methods
    private void InitializeEnemyPool()
    {
        _enemyPoolContainer = new GameObject($"PoolContainer - Enemies");
        _enemyPoolContainer.transform.SetParent(transform);


        _enemyPool = new ObjectPool<Enemy>(
                       CreateEnemy,
                       enemy => {
                            enemy.gameObject.SetActive(true);
                       }, enemy => {
                            enemy.gameObject.SetActive(false);
                       }, enemy => {
                            Destroy(enemy.gameObject); Debug.Log("Destroying Enemy.");
                       }, false, 10, 200);
    }

    private Enemy CreateEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0, _enemyPrefabs.Length);
        Enemy enemyToSpawn = _enemyPrefabs[randomIndex];

        Enemy enemyInstance = Instantiate(enemyToSpawn);
        enemyInstance.transform.SetParent(_enemyPoolContainer.transform);
        enemyInstance.gameObject.SetActive(false);
        return enemyInstance;
    }

    private void KillEnemy(Enemy enemy)
    {
        if (_usePool) { _enemyPool.Release(enemy); }
        else { Destroy(enemy.gameObject); }
    }

    public Enemy GetEnemy()
    {
        var enemy = _usePool ? _enemyPool.Get() : CreateEnemy();
        enemy.Init(KillEnemy);
        return enemy;
    }

    #endregion
}
