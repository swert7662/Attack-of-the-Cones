using System;
using System.Collections;
using UnityEngine;

public class NewEnemy : MonoBehaviour, IHealth
{
    #region Variables, Properties, and Awake
    [SerializeField] protected EnemyStats _enemyStats;
    [SerializeField] private PlayerStats _player;

    [SerializeField] private GameEvent DeathEvent;
    [SerializeField] private GameEvent PointsAddEvent;
    [SerializeField] private GameEvent DamagedEvent;

    [SerializeField] private GameObject EliteEnemy;

    [SerializeField] private int pointValue;

    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public float AttackDamage { get; set; }

    public Vector3 Extents { get; set; }

    private Animator _animator;    
    private Vector3 _originalScale;

    private Vector3 _currentDirection;
    private float _updateInterval = .5f; // Time between direction updates
    private float _timeUntilNextUpdate = 0f;
    private float PromoteTimer = 3f;
    private Collectible SelectedCollectible;

    private EnemyState CurrentState = EnemyState.FollowPlayer;
    public enum EnemyState
    {
        FollowPlayer,
        Promoting
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
    #endregion

    private void Update()
    {
        switch (CurrentState)
        {
            case EnemyState.FollowPlayer:
                FollowPlayerBehavior();
                break;
            case EnemyState.Promoting:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #region State Behaviors

    public void SetState(Component sender, object data)
    {
        if (data is not EnemyState state) { Debug.LogError("SetState failed: Data is not of type EnemyState"); return; }
        EnemyState updatedState = (EnemyState)data;
        Debug.Log($"Enemy state updated to {updatedState}");
        CurrentState = updatedState;
    }

    private void FollowPlayerBehavior()
    {
        if (Time.time >= _timeUntilNextUpdate)
        {
            UpdateDirection();
            _timeUntilNextUpdate = Time.time + _updateInterval;
        }

        MoveInCurrentDirection();
    }

    #endregion

    #region Health Functions

    public void Damage(float damageAmount, DamageType damageType)
    {
        CurrentHealth -= damageAmount;
        _animator.SetTrigger("Hit");

        DamagedData damageData = 
            new DamagedData(this.gameObject,
                            MaxHealth,
                            Extents,
                            gameObject.transform.position,
                            damageAmount,
                            CurrentHealth,
                            damageType);

        DamagedEvent.Raise(this, damageData);

        if (CurrentHealth <= 0) { Die(); }
    }

    public virtual void Die()
    {
        EnemyDeathData DeathData = 
            new EnemyDeathData(gameObject.transform.position, 
                              (short)_enemyStats.expPoints);

        DeathEvent.Raise(this.transform, DeathData);
        PointsAddEvent.Raise(this, pointValue);
      
        Despawn();
    }

    #endregion

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Powerup"))
        {
            if (collision.gameObject.TryGetComponent<Collectible>(out Collectible collectible))
            {
                CurrentState = EnemyState.Promoting;
                SelectedCollectible = collectible;
                StartCoroutine(PromotionCoroutine());
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Powerup"))
        {
            CurrentState = EnemyState.FollowPlayer;
        }
    }
    private IEnumerator PromotionCoroutine()
    {
        yield return new WaitForSeconds(PromoteTimer);

        if (CurrentState == EnemyState.Promoting && SelectedCollectible != null)
        {
            Promote(SelectedCollectible.GetPowerupCategory());
            Destroy(SelectedCollectible.gameObject);
        }
    }

    private void Promote(PowerupList.PowerUpCategory dropType)
    {
        EliteEnemy newElite = ObjectPoolManager.SpawnObject<EliteEnemy>(EliteEnemy, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Enemy);
        newElite.SetPowerupDrop(dropType);
        Despawn();
    }

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
    #endregion

    #region Reset and Despawn
    public void ResetForPool()
    {
        CurrentHealth = _enemyStats.maxHealth;
        CurrentState = EnemyState.FollowPlayer;
        _animator.transform.localScale = _originalScale;
    }

    // These are properties that are reset when pulled from the object pool
    protected virtual void OnEnable()
    {
        MaxHealth = _enemyStats.maxHealth;
        CurrentHealth = _enemyStats.maxHealth;
        AttackDamage = _enemyStats.attackDamage;
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