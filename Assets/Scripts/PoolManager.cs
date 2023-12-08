using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private int _projectilePoolAmount;

    [SerializeField] private bool _usePool = true;

    public static PoolManager Instance { get; private set; }

    private ObjectPool<Projectile> _projectilePool;
    void Awake()
    {
        _projectilePool = new ObjectPool<Projectile>(CreateProjectile, // On create
            projectile => {
                projectile.gameObject.SetActive(true); // On get from pool
            }, projectile => {
                projectile.gameObject.SetActive(false); // On return to pool
            }, projectile => {
                Destroy(projectile.gameObject); Debug.Log("Destroying Projectile."); // On destroy
            }, false, 10, 200); // Collection Check, Default, Max

        //InvokeRepeating(nameof(SetupPool), 1f, 1f);
        //StartCoroutine(SpawnProjectileCoroutine());
        SetupPool();
    }
    private Projectile CreateProjectile()
    {
        Projectile projectileInstance = Instantiate(_projectilePrefab);
        projectileInstance.SetPool(_projectilePool);
        return projectileInstance;
    }
    private void KillProjectile(Projectile projectile)
    {
        if (_usePool) { _projectilePool.Release(projectile); }
        else { Destroy(projectile.gameObject); }
    }

    public Projectile GetProjectile()
    {
        return _projectilePool.Get();
    }

    private void SetupPool()
    {
        for (var i = 0; i < _projectilePoolAmount; i++)
        {
            var projectile = _usePool ? _projectilePool.Get() : Instantiate(_projectilePrefab);
            projectile.transform.position = transform.position;
            projectile.Init(KillProjectile);
        }
    }
}
