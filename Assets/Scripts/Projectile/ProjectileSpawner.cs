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
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _fireShakeForce = .2f;
    [SerializeField] private Projectile _projectilePrefab;
    
    private List<GameObject> _enemiesInRange = new();
    private CircleCollider2D _attackRangeCollider;
    private System.Random _rand = new();
    private Transform _target;
    private LayerMask _enemyLayerMask;
    private float _timeSinceLastAttack;
    #endregion
    private Vector2 _lastForceDirection;
    //public float _angleNum = 90;
    private float _lastAngle;
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
        Vector3 direction = _target.position - transform.position;

        ObjectPoolManager.SpawnObject(_projectilePrefab.gameObject, direction, transform.position, ObjectPoolManager.PoolType.Projectile); // Shoot projectile at target

        RecoilShake(direction);
    }

    private void RecoilShake(Vector3 direction)
    {
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _lastAngle = angle;
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

        Gizmos.color = Color.red; // Set the color of the Gizmo
        var radians = _lastAngle * Mathf.Deg2Rad;

        // Calculate the direction vector from the angle
        Vector3 direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * 5; // Multiplied by 2 for better visibility

        // Draw a ray from the object's position in the direction of the angle
        Gizmos.DrawRay(transform.position, direction);
    }
}
