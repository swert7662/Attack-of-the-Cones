using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class EnemyTest : MonoBehaviour
{
    public Transform playerTransform;
    public float stepSize = 5f;
    public float pauseTime = 0.5f;
    public float directionOffset = 0f;

    public float attackRange = 1f;
    public float attackCooldown = 1f;
    public float attackLungeDistance = 0.5f;
    public float attackScale = 1.1f;

    private Vector3 targetPosition;

    void Start()
    {
        MoveTowardsPlayerAsync();
    }

    async void MoveTowardsPlayerAsync()
    {
        while (true)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= attackRange)
            {
                await AttackPlayer(); // Call the attack method
                await Task.Delay((int)(attackCooldown * 1000)); // Pause, can be used for attack cooldown
                continue; // Skip the rest of the loop to avoid moving
            }

            // Calculate direction towards player
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction = (directionOffset != 0f) ? Quaternion.Euler(0, 0, directionOffset) * direction : direction; // This is to make the movement more interesting

            // Calculate step size and targetPosition
            float adjustedStepSize = Mathf.Min(stepSize, Vector3.Distance(transform.position, playerTransform.position));
            targetPosition = transform.position + direction * Mathf.Min(stepSize, distanceToPlayer);


            // Move towards target position
            while (Vector3.Distance(transform.position, targetPosition) > attackRange)
            {
                if (!gameObject.activeInHierarchy) return; // Safety check if the GameObject is still active
                transform.position = Vector3.Slerp(transform.position, targetPosition, adjustedStepSize * Time.deltaTime);
                await Task.Yield();
            }

            // Pause at target position
            await Task.Delay((int)(pauseTime * 1000));
        }
    }

    async Task AttackPlayer()
    {
        Debug.Log("Attacking player!"); // Alternative for logging
        Vector3 originalPosition = transform.position;
        Vector3 attackPosition = Vector3.Lerp(transform.position, playerTransform.position, attackLungeDistance); // Adjust 0.1f for more or less movement

        Vector3 originalScale = transform.localScale;

        // Attack the player
        transform.localScale *= attackScale;
        transform.position = attackPosition;
        await Task.Delay(100); // Short pause at attack position

        // Move back to the original position
        transform.localScale = originalScale;
        transform.position = originalPosition;
    }
}
