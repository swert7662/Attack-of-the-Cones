using Com.LuisPedroFonseca.ProCamera2D;
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
    [SerializeField] private float _fireShakeForce = .2f;

    [SerializeField] private PlayerStats _player;
    [SerializeField] private EnemyStats _enemyStats; 
    
    private CircleCollider2D _attackRangeCollider;
    private List<GameObject> _enemiesInRange = new();
    private Transform _target;

    private System.Random _rand = new();
    private float _timeSinceLastAttack;
    private Projectile currentProjectile;
    #endregion

    private void Awake()
    {
        if (_enemyStats == null) { Debug.LogError("EnemyStats is null!"); }
        if (_player == null) { Debug.LogError("Player is null!"); }
        //currentProjectile = _player.Projectile;
        _attackRangeCollider = GetComponent<CircleCollider2D>();
        if (_attackRangeCollider != null) { _attackRangeCollider.radius = _attackRange; }
        _timeSinceLastAttack = 0f;
    }

    private void Update()
    {
        UpdateEnemiesInRange();
        _timeSinceLastAttack += Time.deltaTime;

        if (_timeSinceLastAttack >= (1f / _player.FireRate))
        {
            _target = ChooseTarget();
            if (_target != null)
            {
                ProjectileSpawn();
                _timeSinceLastAttack = 0f;
            }
        }
    }

    private void ProjectileSpawn()
    {
        Vector3 direction = _target.position - transform.position;

        GameObject bullet = ObjectPoolManager.SpawnObject(_player.Projectile.gameObject, direction, transform.position, ObjectPoolManager.PoolType.Projectile); // Shoot projectile at target
        Projectile projectile = bullet.GetComponent<Projectile>();
        projectile.SetTarget(_target);

        //RecoilShake(direction);
    }

    private void RecoilShake(Vector3 direction)
    {
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var radians = angle * Mathf.Deg2Rad;
        var vForce = new Vector2((float)Mathf.Sin(radians), (float)Mathf.Cos(radians)) * _fireShakeForce;
        ProCamera2DShake.Instance.ApplyShakesTimed(new Vector2[] { vForce }, new Vector3[] { Vector3.zero }, new float[] { .05f });
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
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, _attackRange, _enemyStats.enemyLayerMask);
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
