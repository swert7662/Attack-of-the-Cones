using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class NewEnemy : MonoBehaviour, IDamageable
{
    public float _speed;
    public float _attackRange;

    [SerializeField] private ParticleSystem _deathSprinkles;
    [SerializeField] private Healthbar _healthbar;

    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }

    private Transform _target;
    private Animator _animator;
    private DamageFlash _damageFlash;

    private void Awake()
    {
        _target = GameManager.Instance._playerTransform;
        _animator = GetComponentInChildren<Animator>();
        _damageFlash = GetComponentInChildren<DamageFlash>();
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        FlipTowardsTarget();
        //If not in attack range, move towards the player
        if (Vector3.Distance(transform.position, _target.position) > _attackRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
        }
    }

    #region Health Functions

    public void Damage(float damageAmount)
    {
        // debug that says how much damage was taken
        Debug.Log($"{gameObject.name} took {damageAmount} damage");
        _animator.SetTrigger("Hit");
        _damageFlash.Flash();
        CurrentHealth -= damageAmount;        
        _healthbar.UpdateHealthbar(MaxHealth, CurrentHealth);
        if (CurrentHealth <= 0)
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
        CurrentHealth = MaxHealth;
        _healthbar.UpdateHealthbar(MaxHealth, CurrentHealth);

        //IsAggroed = false;
        //IsWithinStrikingDistance = false;
    }

    private void FlipTowardsTarget()
    {
        bool shouldFlip = (transform.position.x > _target.position.x && _animator.transform.localScale.x > 0) ||
                          (transform.position.x < _target.position.x && _animator.transform.localScale.x < 0);

        if (shouldFlip)
        {
            Vector3 scale = _animator.transform.localScale;
            scale.x *= -1;
            _animator.transform.localScale = scale;
        }
    }


    #endregion
}
