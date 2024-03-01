using UnityEngine;

public class BurnArea : MonoBehaviour
{
    [SerializeField] private BoxCollider2D BoxCollider2D;
    [SerializeField] private SpriteRenderer SpriteRenderer;

    private int Damage;
    private float Range; 
    private float MaxLifetime; 
    private float tickRate; 

    private float tickTimer; 
    private float timer;
    private LayerMask targetLayerMask;

    public void Initialization(int damage, float range, float duration, float tickRate, LayerMask targetLayerMask)
    {
        this.Damage = damage;
        this.Range = range;
        this.MaxLifetime = duration;
        this.tickRate = tickRate;
        this.targetLayerMask = targetLayerMask;

        Vector2 newSize = new Vector2(range, BoxCollider2D.size.y);
        BoxCollider2D.size = newSize;
        Vector2 newSpriteSize = new Vector2(range, 2.46f);
        SpriteRenderer.size = newSize;

        timer = MaxLifetime;
        tickTimer = tickRate;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        tickTimer -= Time.deltaTime;

        if (timer <= 0)
        {
            Destroy(gameObject); // Destroy the area effect after its lifetime ends
        }
        else if (tickTimer <= 0)
        {
            tickTimer = tickRate;
            ApplyDamage();
        }
    }

    private void ApplyDamage()
    {
        Bounds bounds = BoxCollider2D.bounds;
        Collider2D[] hits = Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0f, targetLayerMask);
        foreach (Collider2D hit in hits)
        {
            IHealth enemyHealth = hit.GetComponent<IHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.Damage(Damage, DamageType.Fire);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Set the color of the Gizmo to visually indicate the burn area
        Gizmos.DrawWireSphere(transform.position, Range); // Draw a wireframe sphere with the specified range
    }
}
