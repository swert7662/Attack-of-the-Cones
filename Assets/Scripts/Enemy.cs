using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _health = 100;
    [SerializeField] private float _deathTime = 2f;

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotationSpeed = 1.5f;

    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _attackSpeed = 1.5f;
    [SerializeField] private float _attackPause = 0.5f; // Duration of attack pause

    private Transform _target;
    private bool _isAttacking;
    private bool _isDying;

    private void Awake()
    {
        // If many enemies this might be expensive. Consider passing the player reference to enemies through a manager or when spawning them.
        _target = FindObjectOfType<PlayerController>().transform;
        OrientToPlayer();

        _isAttacking = false;
        _isDying = false;
    }

    private void FixedUpdate()
    {
        if (_health <= 0 && !_isDying)
        {
            StartCoroutine(Die());
        }

        if (!_isAttacking)
        {
            if (IsPlayerInRange()) { StartCoroutine(PerformAttack()); }
            else { MoveTowardsPlayer(); }
        }
    }

    private IEnumerator Die()
    {
        _isDying = true;
        Debug.Log("Dying");
        yield return new WaitForSeconds(_deathTime);
        Destroy(gameObject);
    }

    private bool IsPlayerInRange()
    {
        return (Vector3.Distance(transform.position, _target.position) <= _attackRange && !_isAttacking);
    }

    private IEnumerator PerformAttack()
    {
        _isAttacking = true;
        Debug.Log("Attack");
        yield return new WaitForSeconds(_attackPause); // Wait for the duration of the attack pause
        _isAttacking = false;
        OrientToPlayer();
    }

    /*private void MoveTowardsPlayer()
    {
        var direction = (_target.position - transform.position);
        transform.up = Vector3.MoveTowards(transform.up, direction, _rotationSpeed * Time.deltaTime); //MoveTowards has a slower turn effect than lerp
        transform.position += transform.up * _speed * Time.deltaTime;
    }*/
    private void MoveTowardsPlayer()
    {
        // Calculate direction
        Vector3 direction = (_target.position - transform.position).normalized;

        // Calculate rotation towards target
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        // Move towards target
        transform.position += direction * _speed * Time.deltaTime;
    }


    private void OrientToPlayer()
    {
        transform.up = _target.position - transform.position;
        Debug.Log("Orient");
    }

    void OnDrawGizmos()
    {
        if (_target != null)
        {
            // Calculate the normalized direction towards the target
            Vector3 directionToTarget = (_target.position - transform.position).normalized;

            // Draw forward direction using the calculated direction
            Gizmos.color = Color.red;
            Vector3 forwardEnd = transform.position + directionToTarget * 2; // Adjust the multiplier to control the length of the line
            Gizmos.DrawLine(transform.position, forwardEnd);
            Gizmos.DrawSphere(forwardEnd, 0.1f);
        }

        // Draw attack range circle
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }

}
