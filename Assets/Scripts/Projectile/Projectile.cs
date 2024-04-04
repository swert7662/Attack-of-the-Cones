using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour, IDespawn
{
    #region Variables and Properties
    [SerializeField] private float projectileSpeed; 
    [SerializeField] private PlayerStats _player;

    private Rigidbody2D rb;

    public static event Action OnProjectileShoot;
    public static event Action<Vector2, Vector2> OnProjectileImpact;
    public static event Action<Vector2, Vector2, Vector3> OnProjectileExit;
    public static event Action<GameObject, Vector2> OnAdditionalEffectsTrigger;
    #endregion

    #region Awake, Enable, and FixedUpdate
    private void Awake() 
    { 
        rb = GetComponent<Rigidbody2D>(); 
    }

    private void OnEnable() 
    {
        OnProjectileShoot?.Invoke();
    }

    private void FixedUpdate() 
    { 
        MoveProjectile(); 
    }
    #endregion

    #region Physics Methods

    private void MoveProjectile()
    {
        rb.velocity = transform.up * projectileSpeed;
    }
    #endregion

    #region Collision Handling
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision);
        Despawn();
    }

    private void HandleCollision(Collision2D collision)
    {
        Vector2 hitPoint = collision.GetContact(0).point;
        Vector2 hitNormal = collision.GetContact(0).normal;

        OnProjectileImpact?.Invoke(hitPoint, hitNormal);

        IHealth damageable = collision.collider.GetComponent<IHealth>() ?? collision.collider.GetComponentInParent<IHealth>();
        if (damageable != null)
        {
            OnProjectileExit?.Invoke(hitPoint, hitNormal, damageable.Extents);
            damageable.Damage(_player.Damage, DamageType.Normal);
            OnAdditionalEffectsTrigger?.Invoke(collision.gameObject, hitPoint);
        }
    }

    public void Despawn()
    {
        ResetForPool();
        ObjectPoolManager.DespawnObject(gameObject);
    }

    public void ResetForPool()
    {
        rb.velocity = Vector2.zero;
    }

    #endregion
}
