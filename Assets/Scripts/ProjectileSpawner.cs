using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private int _spawnAmount = 1;
    [SerializeField] private bool _usePool = true;
    [SerializeField] private Transform _target;

    private ObjectPool<Projectile> _projectilePool;

    private void Start()
    {
        _projectilePool = new ObjectPool<Projectile>(() => {
            return Instantiate(_projectilePrefab); // On create
        }, projectile => {
               projectile.gameObject.SetActive(true); // On get from pool
        }, projectile => {
               projectile.gameObject.SetActive(false); // On return to pool
        }, projectile => {
               Destroy(projectile.gameObject); // On destroy
        }, false, 10, 20); // Collection Check, Default, Max
        
        //InvokeRepeating(nameof(SpawnProjectile), 1f, 1f);
        StartCoroutine(SpawnProjectileCoroutine());
    }

    private IEnumerator SpawnProjectileCoroutine()
    {
        while (true)
        {
            SpawnProjectile();
            yield return new WaitForSeconds(1f);
        }
    }

    private void SpawnProjectile()
    {
        for (var i = 0; i < _spawnAmount; i++)
        {
            var projectile = _usePool ? _projectilePool.Get() : Instantiate(_projectilePrefab);
            projectile.transform.position = transform.position;
            projectile.Init(KillProjectile, this.transform);
        }
    }

    private void KillProjectile(Projectile projectile)
    {
        if (_usePool) { _projectilePool.Release(projectile); }
        else { Destroy(projectile.gameObject); }
    }
}
