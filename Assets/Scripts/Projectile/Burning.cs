using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Burning : MonoBehaviour
{
    [SerializeField] private PowerupStats _powerupStats;

    private int Damage;
    private float TickTimer;
    private float Timer; 

    private ParticleSystem BurningEffect;
    private ParticleSystem.EmissionModule EmissionModule;

    private bool despawning = false;

    private IHealth EnemyHealth;

    private void Awake()
    {
        BurningEffect = GetComponent<ParticleSystem>();        
        EmissionModule = BurningEffect.emission;
        EnemyHealth = GetComponentInParent<IHealth>();
    }

    private void OnEnable()
    {
        BurningEffect.Play();
        UpdateEffect();
        ResetTimer();
        ResetTickTimer();
    }

    private void UpdateEffect()
    {
        Damage = (int)Mathf.Ceil(_powerupStats.DamageLevel * 0.5f * _powerupStats.FireDamageMultiplier);
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
        if (EnemyHealth != null)
        {
            EnemyHealth.Damage(Damage, DamageType.Fire);
        }
    }

    private void Despawn()
    {
        ResetTimer();
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
