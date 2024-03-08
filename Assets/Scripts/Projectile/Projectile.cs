using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour, IDespawn
{
    #region Variables and Properties
    [SerializeField] private float projectileSpeed = 1f; 
    [SerializeField] private PlayerStats _player;

    private Transform targetTransform;
    private Transform startingTransform;
    public float journeyTime = 3.0f;
    private float startTime;

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
        startingTransform = transform;
        startTime = Time.time;
    }

    private void FixedUpdate() { MoveProjectile(); }
    #endregion

    #region Physics Methods

    private void MoveProjectile()
    {
        rb.velocity = transform.up * projectileSpeed;
        //Vector3 center = (startingTransform.position + targetTransform.position) * 0.5F;

        //// move the center a bit downwards to make the arc vertical
        //center -= new Vector3(0, .25f, 0);

        //// Interpolate over the arc relative to center
        //Vector3 riseRelCenter = startingTransform.position - center;
        //Vector3 setRelCenter = targetTransform.position - center;

        //// The fraction of the animation that has happened so far is
        //// equal to the elapsed time divided by the desired time for
        //// the total journey.
        //float fracComplete = (Time.time - startTime) / journeyTime;

        //transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
        //transform.position += center;
    }
    #endregion

    public void SetTarget(Transform target)
    {
        targetTransform = target;
    }

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
        startingTransform = null;
        startTime = 0f;
    }

    #endregion
}
