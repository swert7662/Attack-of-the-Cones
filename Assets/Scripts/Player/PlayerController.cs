using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IHealth
{   
    [SerializeField] private float damageCooldown = .5f;

    [SerializeField] private Health health;
    [SerializeField] private Player player;
    [SerializeField] private EnemyStats enemyStats;

    [SerializeField] private Projectile _defaultProjectile;

    [SerializeField] private GameEvent playerDeathEvent;
    [SerializeField] private GameEvent playerHealthEvent;

    

    private DamagedData _playerDamagedData;

    private float lastDamageTime = -1.0f;
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
        player.SetStats();

        health.CurrentHealth = health.MaxHealth;        

        _playerDamagedData = new DamagedData(this.gameObject);
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        if (collider != null)
        {
            Extents = collider.bounds.extents;
            player.SetCollider(collider);
        }
    }

    private void Update()
    {
        if (player.IsAlive)
        {
            NormalMovement();
        }
        player.Position = transform.position;
    }

    private void NormalMovement()
    {
        // Get input from WASD keys
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Create a movement vector based on the input and speed
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * player.Speed * Time.deltaTime;

        // Move the player
        transform.Translate(movement);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Get the game object we collided with
        GameObject collisionGameObject = collision.gameObject;

        if (collisionGameObject.tag.Equals("Enemy") && Time.time - lastDamageTime > damageCooldown)
        {
            Damage(enemyStats.attackDamage);
            lastDamageTime = Time.time;
        }
    }   

    public void Damage(float damageAmount)
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
    #region Old Movement
    private void MouseBasedMovement()
    {
        // Convert the mouse position to world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z; // Ensure there is no change in the z-axis

        // Move the player towards the mouse position
        transform.position = Vector3.MoveTowards(transform.position, mousePosition, player.Speed * Time.deltaTime);
    }
    #endregion
}