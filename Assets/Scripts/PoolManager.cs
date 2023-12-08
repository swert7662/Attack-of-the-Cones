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
    private GameObject _poolContainer;

    void Awake()
    {
        Instance = this;

        _poolContainer = new GameObject($"PoolContainer - {_projectilePrefab.name}");
        _poolContainer.transform.SetParent(transform);

        _projectilePool = new ObjectPool<Projectile>(
            CreateProjectile, // On create
            projectile => {
                projectile.gameObject.SetActive(true); // On get from pool
            }, projectile => {
                projectile.gameObject.SetActive(false); // On return to pool
            }, projectile => {
                Destroy(projectile.gameObject); Debug.Log("Destroying Projectile."); // On destroy
            }, false, 10, 200); // Collection Check, Default, Max
    }
    private Projectile CreateProjectile()
    {
        Projectile projectileInstance = Instantiate(_projectilePrefab);
        projectileInstance.transform.SetParent(_poolContainer.transform);
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
}
