using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IHealth
{
    [SerializeField] private float speed = 5.0f;    
    [SerializeField] private float health = 100.0f;
    [SerializeField] private float damageCooldown = .5f;

    private float lastDamageTime = -1.0f;

    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public Vector2 Extents { get; set; }

    public event Action<GameObject> OnDamageTaken;

    private void Awake()
    {
        MaxHealth = health;
        CurrentHealth = health;
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        if (collider != null)
        {
            Extents = collider.bounds.extents;
        }
    }

    private void Update()
    {
        NormalMovement();
    }

    private void NormalMovement()
    {
        // Get input from WASD keys
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Create a movement vector based on the input and speed
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime;

        // Move the player
        transform.Translate(movement);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Get the game object we collided with
        GameObject collisionGameObject = collision.gameObject;

        if (collisionGameObject.CompareTag("Enemy") && Time.time - lastDamageTime > damageCooldown)
        {
            Damage(collisionGameObject.GetComponent<NewEnemy>().AttackDamage);
            lastDamageTime = Time.time;
        }
    }   

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        OnDamageTaken?.Invoke(gameObject);

        if (CurrentHealth <= 0) { Die(); }
    }

    public void Die()
    {
        Debug.Log("Player died!");
    }
    #region Old Movement
    private void MouseBasedMovement()
    {
        // Convert the mouse position to world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z; // Ensure there is no change in the z-axis

        // Move the player towards the mouse position
        transform.position = Vector3.MoveTowards(transform.position, mousePosition, speed * Time.deltaTime);
    }
    #endregion
}