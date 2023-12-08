using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _projectileSpeed = 1f;

    public float _selfDestructTime = 2f;
    private float _lifeTime;

    private Action<Projectile> _killAction;
    private Rigidbody2D _rb;

    public void Init(Action<Projectile> killAction)
    {
        _killAction = killAction;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _lifeTime = _selfDestructTime;
    }

    private void Update()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0) { Impact(); }
        _rb.velocity = transform.up * _projectileSpeed;
    }

    public void Shoot(Vector2 direction, Vector2 position)
    {
        transform.position = position;
        transform.up = direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy");
            Impact();
        }
    }

    private void Impact()
    {
        ResetProjectile();
        _killAction(this);
    }

    public void ResetProjectile()
    {
        _rb.velocity = Vector2.zero;
        _lifeTime = _selfDestructTime;
    }

    void OnDrawGizmos()
    {
        // Draw forward direction
        Gizmos.color = Color.blue;
        Vector3 forwardEnd = transform.position + transform.up * 2;
        Gizmos.DrawLine(transform.position, forwardEnd);
        Gizmos.DrawSphere(forwardEnd, 0.1f); 
    }
}
