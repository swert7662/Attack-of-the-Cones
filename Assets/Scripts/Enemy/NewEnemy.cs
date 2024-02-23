using System;
using UnityEngine;

public class NewEnemy : MonoBehaviour, IHealth, IDespawn
{
    [SerializeField] protected EnemyStats _enemyStats;
    [SerializeField] private Player _player;

    [SerializeField] private GameEvent _enemyDeathEvent;
    private EnemyDeathData _enemyDeathData;
    
    [SerializeField] private GameEvent _enemyDamagedEvent;
    private DamagedData _enemyDamagedData;

    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public float AttackDamage { get; set; }

    public Vector3 Extents { get; set; }

    private Animator _animator;    
    private Vector3 _originalScale;

    private Vector3 _currentDirection;
    private float _updateInterval = 1f; // Time between direction updates
    private float _timeUntilNextUpdate = 0f;

    protected virtual void OnEnable()
    {
        MaxHealth = _enemyStats.maxHealth;
        CurrentHealth = _enemyStats.maxHealth;
        AttackDamage = _enemyStats.attackDamage;
    }

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _originalScale = _animator.transform.localScale;
        _enemyDeathData = new EnemyDeathData(Vector3.zero, 0);
        _enemyDamagedData = new DamagedData(this.gameObject, MaxHealth, Extents);
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        if (collider != null)
        {
            Extents = collider.bounds.extents;
        }
    }

    private void Update()
    {
        //FlipTowardsTarget();

        // Update the direction at fixed intervals
        if (Time.time >= _timeUntilNextUpdate)
        {
            UpdateDirection();
            _timeUntilNextUpdate = Time.time + _updateInterval;
        }

        MoveInCurrentDirection();
    }

    #region Health Functions

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        _animator.SetTrigger("Hit");

        _enemyDamagedData.Position = gameObject.transform.position;
        _enemyDamagedData.Extents = Extents;
        _enemyDamagedData.DamageAmount = damageAmount;
        _enemyDamagedData.GameObjectSender = gameObject;

        _enemyDamagedEvent.Raise(this, _enemyDamagedData); //Calls out to damage flash, healthbar, and damage popup

        if (CurrentHealth <= 0) { Die(); }
    }

    public virtual void Die()
    {
        _enemyDeathData.Position = gameObject.transform.position;
        _enemyDeathData.ExpPoints = (short)_enemyStats.expPoints;
        Debug.Log("Enemy died worth " + _enemyDeathData.ExpPoints);

        _enemyDeathEvent.Raise(this.transform, _enemyDeathData);
        
        Despawn();
    }

    #endregion

    #region Movement Functions
    private void MoveInCurrentDirection()
    {
        transform.position += _currentDirection * _enemyStats.speed * Time.deltaTime;
    }

    private void UpdateDirection()
    {
        if (_player.Position != null)
        {
            Vector3 targetDirection = _player.Position - transform.position;
            _currentDirection = targetDirection.normalized;
        }
    }

    private void FlipTowardsTarget()
    {
        bool shouldFlip = (transform.position.x > _player.Position.x && _animator.transform.localScale.x > 0) ||
                          (transform.position.x < _player.Position.x && _animator.transform.localScale.x < 0);

        if (shouldFlip)
        {
            Vector3 scale = _animator.transform.localScale;
            scale.x *= -1;
            _animator.transform.localScale = scale;
        }
    }
    #endregion

    #region Reset and Despawn
    public void ResetForPool()
    {
        CurrentHealth = _enemyStats.maxHealth;
        _animator.transform.localScale = _originalScale;
    }
    public void Despawn()
    {
        ResetForPool();
        ObjectPoolManager.DespawnObject(gameObject);
    }
    #endregion
}

[System.Serializable]
public class EnemyDeathData : UnityEngine.Object
{
    public Vector3 Position;
    public short ExpPoints;

    public EnemyDeathData(Vector3 position, short expPoints)
    {
        Position = position;
        ExpPoints = expPoints;
    }
}