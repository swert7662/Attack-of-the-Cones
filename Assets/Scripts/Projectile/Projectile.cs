using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    #region Variables and Properties
    [SerializeField] private float projectileSpeed = 1f;
    [SerializeField] private float selfDestructTime = 2f;
    [SerializeField] private float damage = 10f;    

    private Rigidbody2D rb;

    private Coroutine _returnToPoolTimerCoroutine;

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
        _returnToPoolTimerCoroutine = StartCoroutine(ReturnToPoolAfterTimer()); 
    }

    private void FixedUpdate() { MoveProjectile(); }
    #endregion

    #region Physics Methods

    private void MoveProjectile()
    {
        rb.velocity = transform.up * projectileSpeed;
    }
    #endregion

    #region Pooling Methods
    private IEnumerator ReturnToPoolAfterTimer()
    {
        float elapsedTime = 0f;
        while (elapsedTime < selfDestructTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Despawn();
    }
    public void ResetProjectile() { rb.velocity = Vector2.zero; }
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
            damageable.Damage(damage);
            OnAdditionalEffectsTrigger?.Invoke(collision.gameObject, hitPoint);
        }
    }

    private void Despawn()
    {
        // Add impact effects can be added here later
        ObjectPoolManager.DespawnObject(gameObject);
    }
    #endregion

    #region Gizmos
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Vector3 forwardEnd = transform.position + transform.up * 2;
    //    Gizmos.DrawLine(transform.position, forwardEnd);
    //    Gizmos.DrawSphere(forwardEnd, 0.1f);
    //}
    #endregion
}
