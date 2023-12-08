using System;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _projectileSpeed = 1f;

    public float _selfDestructTime = 2f;

    private Action<Projectile> _killAction;
    private Rigidbody2D _rb;
    private Transform _target;
    private ObjectPool<Projectile> _pool;
    private Vector2 _direction;

    public void Init(Action<Projectile> killAction)
    {
        _killAction = killAction;
        //_target = target;
        //if (_target != null) { transform.up = _target.position - transform.position; }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, _target.position, _projectileSpeed);
        _rb.velocity = transform.up * _projectileSpeed;
    }

    public void Shoot(Vector2 direction, Vector2 position)
    {
        //_exploded = false;
        transform.position = position;
        _direction = direction;
        //transform.rotation = _direction == Vector2.left ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        //_selfDestructTime = Time.time + _maxLifetime; ;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) 
        { 
            Debug.Log("Hit Enemy");
            ResetProjectile(); 
            _killAction(this);
        }
    }

    public void ResetProjectile()
    {
        _rb.velocity = Vector2.zero;
    }

    public void SetPool(ObjectPool<Projectile> pool)
    {
        _pool = pool;
    }

}
