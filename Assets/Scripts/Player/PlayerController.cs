using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IHealth
{   
    [SerializeField] private float damageCooldown = .5f;

    [SerializeField] private Health health;
    [SerializeField] private PlayerStats player;
    [SerializeField] private EnemyStats enemyStats;

    [SerializeField] private Projectile _defaultProjectile;

    [SerializeField] private GameEvent playerDeathEvent;
    [SerializeField] private GameEvent playerHealthEvent;

    private Vector2 movementInput;
    private MainControls mainControls;


    private DamagedData _playerDamagedData;

    private float lastDamageTime = -1.0f;
    private Vector3 Offset = new Vector3(0, 1, 0);
    private ProjectileSpawner _weaponLoadout;

    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public Vector3 Extents { get; set; }

    private void Awake()
    {
        if (player == null) { Debug.LogError("Player is null!"); }
        if (health == null) { Debug.LogError("Health is null!"); }

        player.IsAlive = true;
        player.SetLastFollower(this.transform);
        player.SetProjectile(_defaultProjectile);     
        player.SetTransform(this.transform);
        

        player.ResetStats();

        health.CurrentHealth = health.MaxHealth;        

        _playerDamagedData = new DamagedData(this.gameObject);
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        if (collider != null)
        {
            Extents = collider.bounds.extents;
            player.SetCollider(collider);
        }

        mainControls = new MainControls();

        mainControls.PlayerControls.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        mainControls.PlayerControls.Move.canceled += ctx => movementInput = Vector2.zero;
    }

    private void Update()
    {
        if (player.IsAlive)
        {
            NormalMovement();
        }
        player.Position = transform.position + Offset;
    }

    private void NormalMovement()
    {
        // Create a movement vector based on the input and speed
        Vector3 movement = new Vector3(movementInput.x, movementInput.y, 0) * player.Speed * Time.deltaTime;

        // Move the player
        transform.Translate(movement);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Get the game object we collided with
        GameObject collisionGameObject = collision.gameObject;

        if (collisionGameObject.tag.Equals("Enemy") && Time.time - lastDamageTime > damageCooldown)
        {
            Damage(enemyStats.attackDamage, DamageType.Normal);
            lastDamageTime = Time.time;
        }
    }   

    public void Damage(float damageAmount, DamageType damageType)
    {
        health.CurrentHealth -= damageAmount;

        playerHealthEvent.Raise(this, _playerDamagedData); // Calls out to healthbar and damage flash
        //OnDamageTaken?.Invoke(gameObject, Extents, damageAmount);

        if (health.CurrentHealth <= 0) { Die(); }
    }

    public void Die()
    {
        Debug.Log("Player died!");
        playerDeathEvent.Raise();
        player.IsAlive = false;
    }

    private void OnEnable()
    {
        mainControls.Enable();
    }

    private void OnDisable()
    {
        mainControls.Disable();
    }

}