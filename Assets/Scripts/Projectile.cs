using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _projectileSpeed = 1f;

    public float _selfDestructTime = 2f;

    private Action<Projectile> _killAction;
    private Rigidbody2D _rb;
    private Transform _target;

    public void Init(Action<Projectile> killAction, Transform target)
    {
        _killAction = killAction;
        _target = target;
        if (_target != null) { transform.up = _target.position - transform.position; }
        //else { transform.up = transform.parent.up; } // Fallback direction if no target
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, _target.position, _projectileSpeed);
        //transform.position += transform.up * _projectileSpeed * Time.deltaTime;
        _rb.velocity = transform.up * _projectileSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) { Debug.Log("Hit Enemy"); _killAction(this); }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) { Debug.Log("Triggered Enemy"); _killAction(this); }
    }

    public void ResetProjectile()
    {
        _rb.velocity = Vector2.zero;
    }
}
