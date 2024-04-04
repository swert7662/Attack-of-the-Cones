using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{      
    [SerializeField] private PlayerStats player;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PlayerStateManager _stateManager;
    [SerializeField] private float damageCooldown = .5f;
    [SerializeField] private EnemyStats enemyStats;

    [SerializeField] private Projectile _defaultProjectile;

    private Vector2 movementInput;
    private MainControls mainControls;

    private float lastDamageTime = -1.0f;
    private Vector3 Offset = new Vector3(0, 1, 0);
    private ProjectileSpawner _weaponLoadout;    

    private void Awake()
    {
        if (player == null) { Debug.LogError("Player is null!"); }        

        player.IsAlive = true;
        player.SetLastFollower(this.transform);
        player.SetProjectile(_defaultProjectile);     
        player.SetTransform(this.transform);      
        player.ResetStats();

        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        
        if (collider != null) { player.SetCollider(collider);}

        mainControls = new MainControls();

        mainControls.PlayerControls.Move.performed += ctx => _stateManager.SetMovementInput(ctx.ReadValue<Vector2>());
        mainControls.PlayerControls.Move.canceled += ctx => _stateManager.SetMovementInput(Vector2.zero);
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
        //Vector3 movement = new Vector3(movementInput.x, movementInput.y, 0) * player.Speed * Time.deltaTime;
        Vector3 movement = new Vector3(_stateManager.MovementInput.x, _stateManager.MovementInput.y, 0) * player.Speed * Time.deltaTime;

        transform.Translate(movement);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject collisionGameObject = collision.gameObject;

        if (collisionGameObject.tag.Equals("Enemy") && Time.time - lastDamageTime > damageCooldown)
        {
            _playerHealth.Damage(enemyStats.attackDamage, DamageType.Normal);
            lastDamageTime = Time.time;
        }
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