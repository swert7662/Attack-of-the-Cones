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

    [SerializeField] private GameObject _chainLightning;

    private Rigidbody2D rb;

    private Coroutine _returnToPoolTimerCoroutine;
    #endregion

    #region Awake, Enable, and FixedUpdate
    private void Awake() { rb = GetComponent<Rigidbody2D>(); }

    private void OnEnable() { _returnToPoolTimerCoroutine = StartCoroutine(ReturnToPoolAfterTimer()); }

    private void FixedUpdate() { MoveProjectile(); }
    #endregion

    #region Physics Methods

    //public void Shoot(Vector2 direction, Vector2 position)
    //{
    //    transform.position = position;
    //    transform.up = direction;
    //}

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
        Impact();
    }
    public void ResetProjectile() { rb.velocity = Vector2.zero; }
    #endregion

    #region Collision Handling
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision);
        Impact();
    }

    private void HandleCollision(Collision2D collision)
    {
        var damageable = collision.collider.GetComponent<IDamageable>() ?? collision.collider.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            damageable.Damage(damage);
            ChainLightning(collision);
        }
    }

    private void ChainLightning(Collision2D collision)
    {
        if (GameManager.Instance.IsCooldownElapsed("ElectricSpawner", 1f)) // 1f is the cooldown duration in seconds
        {
            GameObject lightningGO = ObjectPoolManager.SpawnObject(_chainLightning, collision.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Projectile);
            lightningGO.GetComponent<ElectricSpawner>().StartChainAttack(collision.gameObject);
        }
    }

    private void Impact()
    {
        // Add impact effects can be added here later
        ObjectPoolManager.DespawnObject(gameObject);
    }
    #endregion

    #region Gizmos
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 forwardEnd = transform.position + transform.up * 2;
        Gizmos.DrawLine(transform.position, forwardEnd);
        Gizmos.DrawSphere(forwardEnd, 0.1f);
    }
    #endregion
}
