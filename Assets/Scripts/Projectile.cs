using System;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 1f;
    [SerializeField] private float selfDestructTime = 2f;
    [SerializeField] private float damage = 10f;

    private float lifeTime;
    private Action<Projectile> killAction;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ResetLifeTime();
    }

    private void Update()
    {
        UpdateLifeTime();
    }

    private void FixedUpdate()
    {
        MoveProjectile();
    }

    public void Init(Action<Projectile> killAction)
    {
        this.killAction = killAction;
    }

    public void Shoot(Vector2 direction, Vector2 position)
    {
        transform.position = position;
        transform.up = direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision);
        Impact();
    }

    private void MoveProjectile()
    {
        rb.velocity = transform.up * projectileSpeed;
    }

    private void UpdateLifeTime()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Debug.Log("SelfDestructing");
            Impact();
        }
    }

    private void HandleCollision(Collision2D collision)
    {
        var damageable = collision.collider.GetComponent<IDamageable>() ?? collision.collider.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            Debug.Log("Hit damageable");
            damageable.Damage(damage);
        }
    }

    private void Impact()
    {
        ResetProjectile();
        killAction?.Invoke(this);
    }

    public void ResetProjectile()
    {
        rb.velocity = Vector2.zero;
        ResetLifeTime();
    }

    private void ResetLifeTime()
    {
        lifeTime = selfDestructTime;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 forwardEnd = transform.position + transform.up * 2;
        Gizmos.DrawLine(transform.position, forwardEnd);
        Gizmos.DrawSphere(forwardEnd, 0.1f);
    }
}
