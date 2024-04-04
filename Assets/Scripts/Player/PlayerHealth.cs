using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private PlayerStats player;
    [SerializeField] private Health health;
    [SerializeField] private GameEvent playerHealthEvent;
    [SerializeField] private GameEvent playerDeathEvent;

    private DamagedData _playerDamagedData;
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public Vector3 Extents { get; set; }

    private void Awake()
    {
        if (health == null) { Debug.LogError("Health is null!"); }

        health.CurrentHealth = health.MaxHealth;

        _playerDamagedData = new DamagedData(transform.GetComponentInParent<PlayerController>().gameObject);

        CapsuleCollider2D collider = GetComponentInParent<CapsuleCollider2D>();
        if (collider != null)
        {
            Extents = collider.bounds.extents;
        }
    }
    public void Damage(float damageAmount, DamageType damageType)
    {
        health.CurrentHealth -= damageAmount;

        playerHealthEvent.Raise(this, _playerDamagedData); // Calls out to healthbar and damage flash

        if (health.CurrentHealth <= 0) { Die(); }
    }

    public void Die()
    {
        Debug.Log("Player died!");
        playerDeathEvent.Raise();
        player.IsAlive = false;
    }
}
