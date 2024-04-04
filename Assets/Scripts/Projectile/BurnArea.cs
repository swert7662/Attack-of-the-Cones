using System.Collections;
using UnityEngine;

public class BurnArea : MonoBehaviour
{
    [SerializeField] private PowerupStats _powerupStats;
    [SerializeField] private LayerMask TargetLayerMask;
    private BoxCollider2D BoxCollider2D;

    private int Damage;
    private float TickTimer; 
    private float Timer;   

    private ParticleSystem BurnAreaEffect;
    private ParticleSystem.EmissionModule EmissionModule;
    private float BaseEmissionRate;

    private bool despawning = false;

    private void Awake()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
        BurnAreaEffect = GetComponent<ParticleSystem>();
        EmissionModule = BurnAreaEffect.emission;        
        BaseEmissionRate = EmissionModule.rateOverTime.constant;
    }

    private void OnEnable()
    {
        BurnAreaEffect.Play();
        UpdateEffect();
        ResetTimer();
        ResetTickTimer();
    }

    private void UpdateEffect()
    {
        Damage = (int)Mathf.Ceil(_powerupStats.DamageLevel * 2f * _powerupStats.FireDamageMultiplier);
        gameObject.transform.localScale = new Vector3(_powerupStats.FireRange, 1, 1);
        EmissionModule.rateOverTime = BaseEmissionRate * _powerupStats.FireRange;
    }

    private void Update()
    {
        if (despawning) { return; }

        Timer -= Time.deltaTime;
        TickTimer -= Time.deltaTime;

        if (Timer <= TickTimer)
        {
            EmissionModule.enabled = false;
            StartCoroutine(DespawnAfterDelay(5f));
        }

        else if (TickTimer <= 0)
        {
            ResetTickTimer();
            ApplyDamage();
        }
    }

    private IEnumerator DespawnAfterDelay(float delay)
    {
        despawning = true;
        yield return new WaitForSeconds(delay); 
        Despawn();
    }

    private void ApplyDamage()
    {
        Bounds bounds = BoxCollider2D.bounds;
        Collider2D[] hits = Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0f, TargetLayerMask);
        foreach (Collider2D hit in hits)
        {
            IHealth enemyHealth = hit.GetComponent<IHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.Damage(Damage, DamageType.Fire);
            }
        }
    }

    private void Despawn()
    {
        Destroy(gameObject);
        //ObjectPoolManager.DespawnObject(gameObject);
    }

    public void ResetTimer()
    {
        Timer = _powerupStats.BurnDuration;
        despawning = false;
        EmissionModule.enabled = true;
        StopAllCoroutines();
    }

    private void ResetTickTimer()
    {
        TickTimer = _powerupStats.BurnTickRate;
    }

    private void OnDisable()
    {
        Despawn();
    }
}
