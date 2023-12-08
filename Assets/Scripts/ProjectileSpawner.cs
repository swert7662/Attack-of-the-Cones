using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private float _attackRange; 
    [SerializeField] private float _attackSpeed; 
    
    private List<GameObject> _enemiesInRange = new();
    private CircleCollider2D _attackRangeCollider;
    private System.Random _rand = new();
    private Transform _target;
    
    private void Start()
    {
        _attackRangeCollider = GetComponent<CircleCollider2D>();
        if (_attackRangeCollider != null) { _attackRangeCollider.radius = _attackRange; }
    }

    private void Update()
    {
        _target = ChooseTarget();
        if (_target != null) { SpawnProjectile(); }

    }

    private IEnumerator SpawnProjectileCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / _attackSpeed);
        }
    }

    void SpawnProjectile()
    {
        var projectile = PoolManager.Instance.GetProjectile();
        projectile.Shoot(_target.position - transform.position, transform.position);
    }
    
    private Transform ChooseTarget()
    {
        if (_enemiesInRange.Count == 0) { return null; }

        return _enemiesInRange.OrderBy(n => _rand.Next()).FirstOrDefault()?.transform;
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
