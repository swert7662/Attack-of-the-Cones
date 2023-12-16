using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckable
{
    private Transform _target;

    [SerializeField] private ParticleSystem _deathSprinkles;

    [SerializeField] private Healthbar _healthbar;
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }
    public Rigidbody2D RB { get; set; }
    public bool IsFacingRight { get; set; } = true;

    #region State Machine Variables
    public EnemyStateMachine StateMachine { get; set; } 
    public EnemyIdleState IdleState { get; set; }
    public EnemyFollowState FollowState { get; set; }
    public EnemyAttackState AttackState { get; set; }
    public bool IsAggroed { get; set; }
    public bool IsWithinStrikingDistance { get; set; }
    public bool IsDead { get; set; }

    #endregion

    #region ScriptableObject Variables

    [SerializeField] private EnemyIdleSOBase EnemyIdleBase;
    [SerializeField] private EnemyFollowSOBase EnemyFollowBase;
    [SerializeField] private EnemyAttackSOBase EnemyAttackBase;

    public EnemyIdleSOBase EnemyIdleBaseInstance { get; set; }
    public EnemyFollowSOBase EnemyFollowBaseInstance { get; set; }
    public EnemyAttackSOBase EnemyAttackBaseInstance { get; set; }

    #endregion

    #region Awake, Start, Update, FixedUpdate

    private void Awake()
    {
        // Check if the scriptable objects are null and instantiate them if they are not
        EnemyIdleBaseInstance = Instantiate(EnemyIdleBase);
        EnemyFollowBaseInstance = Instantiate(EnemyFollowBase);
        EnemyAttackBaseInstance = Instantiate(EnemyAttackBase);

        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        FollowState = new EnemyFollowState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
    }

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();

        EnemyIdleBaseInstance.Initialize(gameObject, this);
        EnemyFollowBaseInstance.Initialize(gameObject, this);
        EnemyAttackBaseInstance.Initialize(gameObject, this);

        StateMachine.Initialize(IdleState);
        CurrentHealth = MaxHealth;
        _healthbar.UpdateHealthbar(MaxHealth, CurrentHealth);
    }

    private void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    #endregion

    #region Health Functions

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        _healthbar.UpdateHealthbar(MaxHealth, CurrentHealth);
        if (CurrentHealth <= 0 && !IsDead)
        {
            Die();
        }
    }

    public void Die()
    {
        ResetEnemy();
        ObjectPoolManager.SpawnObject(_deathSprinkles.gameObject, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Particle); //Quaternion.Euler(-270, 90, 0)  Quaternion.identity
        ObjectPoolManager.DespawnObject(this.gameObject);
    }

    private void ResetEnemy()
    {
        StateMachine.CurrentEnemyState = IdleState;
        CurrentHealth = MaxHealth;
        _healthbar.UpdateHealthbar(MaxHealth, CurrentHealth);

        //transform.position = Vector3.zero;
        IsDead = false;
        IsAggroed = false;
        IsWithinStrikingDistance = false;
    }

    #endregion

    #region Movement Functions
    public void MoveEnemy(Vector2 velocity)
    {
        RB.velocity = velocity;
        CheckLRDirection(velocity);
    }
    public void CheckLRDirection(Vector2 velocity)
    {
        if (IsFacingRight && velocity.x < 0)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
        else if (!IsFacingRight && velocity.x > 0)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
    }

    #endregion

    #region Distance Checks
    public void SetAggroStatus(bool isAggroed)
    {
        IsAggroed = isAggroed;
    }

    public void SetStrikingDistanceStatus(bool isWithinStrikingDistance)
    {
        IsWithinStrikingDistance = isWithinStrikingDistance;
    }
    #endregion

    #region AnimationTriggers

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }
    public enum AnimationTriggerType
    {
        EnemyDamaged,
        PlayFootstepSound
    }

    #endregion

    #region Gizmos
    void OnDrawGizmos()
    {
        if (StateMachine != null && EnemyIdleBaseInstance != null)
        {
            // Check if the current state is EnemyIdleRandomWander
            if (StateMachine.CurrentEnemyState is EnemyIdleState && EnemyIdleBaseInstance is EnemyIdleRandomWander)
            {
                EnemyIdleRandomWander idleRandomWander = (EnemyIdleRandomWander)EnemyIdleBaseInstance;

                // Access the target position and direction
                Vector3 targetPos = idleRandomWander.GetCurrentTargetPosition();
                Vector3 direction = idleRandomWander.GetCurrentDirection();

                // Draw a small sphere at the target position
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(targetPos, 0.4f);

                // Draw a line indicating the direction
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, transform.position + direction * 2); // Length of the direction line
            }
        }
    }
    #endregion
}
