using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileSpawner : MonoBehaviour
{
    #region Variables and Properties
    [SerializeField] private float _attackRange; 
    [SerializeField] private float _attackSpeed;
    [SerializeField] private Projectile _projectilePrefab;
    
    private List<GameObject> _enemiesInRange = new();
    private CircleCollider2D _attackRangeCollider;
    private System.Random _rand = new();
    private Transform _target;
    private LayerMask _enemyLayerMask;
    private float _timeSinceLastAttack;
    #endregion

    private void Awake()
    {
        _attackRangeCollider = GetComponent<CircleCollider2D>();
        _enemyLayerMask = GameManager.Instance._enemyLayer;
        if (_attackRangeCollider != null) { _attackRangeCollider.radius = _attackRange; }
        _timeSinceLastAttack = 0f;
    }

    private void Update()
    {
        UpdateEnemiesInRange();
        _timeSinceLastAttack += Time.deltaTime;

        if (_timeSinceLastAttack >= (1f / _attackSpeed))
        {
            _target = ChooseTarget();
            if (_target != null)
            {
                ProjectileSpawn();
                _timeSinceLastAttack = 0f;
            }
        }
    }

    void ProjectileSpawn()
    {
        ObjectPoolManager.SpawnObject(_projectilePrefab.gameObject, _target.position - transform.position, transform.position, ObjectPoolManager.PoolType.Projectile); // Shoot projectile at target
    }

    #region Targeting & Trigger Handling
    private Transform ChooseTarget()
    {
        if (_enemiesInRange.Count == 0) { return null; }

        return _enemiesInRange.OrderBy(n => _rand.Next()).FirstOrDefault()?.transform;
    }

    // add to enemies in range list using OverlapCircleAll
    private void UpdateEnemiesInRange()
    {
        _enemiesInRange.Clear();
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, _attackRange, _enemyLayerMask);
        foreach (Collider2D enemy in enemies)
        {
            _enemiesInRange.Add(enemy.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
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
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black; // Set the color of the Gizmo
        Gizmos.DrawWireSphere(transform.position, _attackRange); // Draw a wire sphere with the attackRange as the radius
    }
}
