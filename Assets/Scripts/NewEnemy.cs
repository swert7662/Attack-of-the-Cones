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

    private void Awake()
    {
        _target = GameManager.Instance._playerTransform;
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
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

    #endregion
}
