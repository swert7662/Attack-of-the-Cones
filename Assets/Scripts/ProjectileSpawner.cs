using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private float _attackRange = 10f; 
    [SerializeField] private float _attackSpeed = 1f; 
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private int _spawnAmount = 1;
    [SerializeField] private bool _usePool = true;

    private ObjectPool<Projectile> _projectilePool;
    private List<GameObject> _enemiesInRange = new();
    private CircleCollider2D _attackRangeCollider;
    private System.Random _rand = new();
    private Transform _target;

    private void Start()
    {
        _attackRangeCollider = GetComponent<CircleCollider2D>();
        if (_attackRangeCollider != null) { _attackRangeCollider.radius = _attackRange; }

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
            yield return new WaitForSeconds(1f / _attackSpeed);
        }
    }

    private void SpawnProjectile()
    {
        if (_enemiesInRange.Count > 0)
        {
            _target = ChooseTarget();

            for (var i = 0; i < _spawnAmount; i++)
            {
                var projectile = _usePool ? _projectilePool.Get() : Instantiate(_projectilePrefab);
                projectile.transform.position = transform.position;
                projectile.Init(KillProjectile, _target);
            }
        }
    }

    private Transform ChooseTarget()
    {
        if (_enemiesInRange.Count == 0) { return null; }

        return _enemiesInRange.OrderBy(n => _rand.Next()).FirstOrDefault()?.transform;
    }

    private void KillProjectile(Projectile projectile)
    {
        if (_usePool) { _projectilePool.Release(projectile); }
        else { Destroy(projectile.gameObject); }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy in range");
            _enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _enemiesInRange.Remove(other.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black; // Set the color of the Gizmo
        Gizmos.DrawWireSphere(transform.position, _attackRange); // Draw a wire sphere with the attackRange as the radius
    }
}
