using System;
using UnityEngine;

public class NewEnemy : MonoBehaviour, IHealth
{
    [SerializeField] private EnemyStats _enemyStats;

    [SerializeField] private float despawnRange; // Example range, adjust as needed
    [SerializeField] private float despawnTime; // Time in seconds before despawning

    public event Action<GameObject> OnDamageTaken;
    public static event Action<Vector3, short> OnEnemyDeath;

    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public float AttackDamage { get; set; }
    public Vector2 Extents { get; set; }

    private Animator _animator;    
    private Vector3 _originalScale;

    private Vector3 _currentDirection;
    private float _updateInterval = 1f; // Time between direction updates
    private float _timeUntilNextUpdate = 0f;
    private float despawnTimer;
    private bool isOutOfRange = false;

    private void OnEnable()
    {
        MaxHealth = _enemyStats.maxHealth;
        CurrentHealth = _enemyStats.maxHealth;
        AttackDamage = _enemyStats.attackDamage;
        OnDamageTaken?.Invoke(gameObject); // Call on enable to reset healthbar
    }

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _originalScale = _animator.transform.localScale;
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        if (collider != null)
        {
            Extents = collider.bounds.extents;
        }
    }

    private void Update()
    {
        //FlipTowardsTarget();
        if (isOutOfRange)
        {
            //Debug.Log(this.ToString() + " Enemy is out of range for : " + despawnTimer);
            CheckDespawnCondition();
        }

        // Update the direction at fixed intervals
        if (Time.time >= _timeUntilNextUpdate)
        {
            UpdateDirection();
            _timeUntilNextUpdate = Time.time + _updateInterval;

            if (!isOutOfRange)
            {
                CheckDespawnCondition();
            }
        }

        MoveInCurrentDirection();
    }

    #region Health Functions

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        _animator.SetTrigger("Hit");

        OnDamageTaken?.Invoke(gameObject); //Calls out to damage flash and healthbar

        if (CurrentHealth <= 0) { Die(); }
    }

    public void Die()
    {
        OnEnemyDeath?.Invoke(gameObject.transform.position, (short)_enemyStats.expPoints);
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
        if (GameManager.Instance._playerTransform.position != null)
        {
            Vector3 targetDirection = GameManager.Instance._playerTransform.position - transform.position;
            _currentDirection = targetDirection.normalized;
        }
    }

    private void FlipTowardsTarget()
    {
        bool shouldFlip = (transform.position.x > GameManager.Instance._playerTransform.position.x && _animator.transform.localScale.x > 0) ||
                          (transform.position.x < GameManager.Instance._playerTransform.position.x && _animator.transform.localScale.x < 0);

        if (shouldFlip)
        {
            Vector3 scale = _animator.transform.localScale;
            scale.x *= -1;
            _animator.transform.localScale = scale;
        }
    }
    #endregion

    #region Range Check
    private bool IsOutOfRange()
    {
        return Vector3.Distance(transform.position, GameManager.Instance._playerTransform.position) > despawnRange;
    }

    private void CheckDespawnCondition()
    {
        isOutOfRange = IsOutOfRange();

        if (isOutOfRange)
        {
            despawnTimer += Time.deltaTime;
            if (despawnTimer >= despawnTime)
            {
                Despawn();
            }
        }
        else
        {
            // Reset the timer if the enemy comes back within range
            despawnTimer = 0;
        }
    }
    #endregion

    #region Reset and Despawn
    private void ResetEnemy()
    {
        CurrentHealth = _enemyStats.maxHealth;
        despawnTimer = 0;
        _animator.transform.localScale = _originalScale;
    }
    private void Despawn()
    {
        ResetEnemy();
        ObjectPoolManager.DespawnObject(gameObject);
    }
    #endregion
}
