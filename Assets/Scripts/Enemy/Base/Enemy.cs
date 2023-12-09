using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckable
{
    [SerializeField] private float _deathTime = 2f;

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotationSpeed = 1.5f;

    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _attackSpeed = 1.5f;
    [SerializeField] private float _attackPause = 0.5f; // Duration of attack pause

    private Transform _target;
    private bool _isAttacking;
    private bool _isDying;

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

    #endregion

    #region Idle State Variables
    public Rigidbody2D ProjectilePrefab;
    public float RandomMovementRange = 5f;
    public float RandomMovementSpeed = 2f;

    #endregion

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        FollowState = new EnemyFollowState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;
        RB = GetComponent<Rigidbody2D>();

        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    #region Health Functions

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        if (CurrentHealth <= 0 && !_isDying)
        {
            Die();
        }
    }

    public void Die()
    {
        _isDying = true;
        Debug.Log("Dying");
        Destroy(gameObject);
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


    void OnDrawGizmos()
    {
        if (_target != null)
        {
            // Calculate the normalized direction towards the target
            Vector3 directionToTarget = (_target.position - transform.position).normalized;

            // Draw forward direction using the calculated direction
            Gizmos.color = Color.red;
            Vector3 forwardEnd = transform.position + directionToTarget * 2; // Adjust the multiplier to control the length of the line
            Gizmos.DrawLine(transform.position, forwardEnd);
            Gizmos.DrawSphere(forwardEnd, 0.1f);
        }
    }
}
