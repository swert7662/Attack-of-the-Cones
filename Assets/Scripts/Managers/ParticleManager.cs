using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PowerupStats _powerupStats;
    
    [SerializeField] private ParticleSystem _deathSprinkles;
    [SerializeField] private ParticleSystem _impactParticles;
    [SerializeField] private ParticleSystem _exitParticles;
    [SerializeField] private float _particalOffset = 0.5f;

    [SerializeField] private ParticleSystem _lightningStrike;
    [SerializeField] private GameObject _lineLightningPrefab;
    [SerializeField] private GameObject _explosionVFXPrefab;

    private void OnEnable()
    {
        Projectile.OnProjectileImpact += SpawnImpactParticles;
        Projectile.OnProjectileExit += SpawnExitParticles;
    }

    private void OnDisable()
    {
        Projectile.OnProjectileImpact -= SpawnImpactParticles;
        Projectile.OnProjectileExit -= SpawnExitParticles;
    }     
    // ------------------ Event Handlers ------------------
    public void SpawnDeathVFX(Component sender, object data)
    { 
        if (data is EnemyDeathData deathData)
        {
            Vector3 deathPositon = deathData.Position;
            short expPoints = (short)(deathData.ExpPoints + _player.BonusXP);

            SpawnDeathSprinkles(deathPositon, expPoints);
            if (_powerupStats.EnemyExplode)
            {
                SpawnExplosionEffect(deathPositon, _powerupStats.FireRange);
            }            
        }
        else { Debug.LogWarning("SpawnDeathVFX Failed : Data is not of type EnemyDeathData"); }
    }

    // ------------------ Particle Spawning ------------------
    private void SpawnParticlesAtPoint(GameObject particlePrefab, Vector2 spawnPoint) // Spawn particles at a point
    {
        ObjectPoolManager.SpawnObject(particlePrefab, spawnPoint, Quaternion.identity, ObjectPoolManager.PoolType.Particle);
    }

    private void SpawnImpactParticles(Vector2 spawnPoint, Vector2 direction)
    {
        SpawnImpactExitParticles(_impactParticles.gameObject, spawnPoint, direction);
    }

    private void SpawnExitParticles(Vector2 spawnPoint, Vector2 direction, Vector3 extents)
    {
        SpawnImpactExitParticles(_exitParticles.gameObject, spawnPoint, direction, extents);
    }

    public void SpawnLightningParticles(Component sender, object data)
    {
        if (data is Vector3 vector3)
        {
            Vector2 vector2 = new Vector2(vector3.x, vector3.y); // Correctly convert Vector3 to Vector2
            SpawnParticlesAtPoint(_lightningStrike.gameObject, vector2);
        }
        else { Debug.LogWarning("SpawnLightningParticles Failed : Data is not of type Vector3"); }
    }

    public void SpawnExplosionEffect(Vector3 deathPosition, float fireRange)
    {
        GameObject explosionInstance = Instantiate(_explosionVFXPrefab, deathPosition, Quaternion.identity);
        ParticleSystem particleSystem = explosionInstance.GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            var mainModule = particleSystem.main;
            mainModule.startSize = mainModule.startSize.constant * fireRange;

            Destroy(explosionInstance, particleSystem.main.duration + 0.5f);
        }
        else { Debug.LogError("SpawnExplosionEffect: No ParticleSystem found on the explosion VFX prefab."); }
    }

    // This spawn handles both Impact and Exit particles
    private void SpawnImpactExitParticles(GameObject particlePrefab, Vector2 spawnPoint, Vector2 direction, Vector3? optionalOffset = null)
    {
        // Flip the direction if we have an offset so that they "Exit" from the opposite side
        direction = optionalOffset.HasValue ? -direction : direction;
        Quaternion rotation = Quaternion.FromToRotation(Vector2.right, direction);

        if (optionalOffset.HasValue)
        {
            Vector3 extents = optionalOffset.Value;
            spawnPoint += new Vector2(direction.x * (extents.x + _particalOffset), direction.y * (extents.y + _particalOffset));
        }

        ObjectPoolManager.SpawnObject(particlePrefab.gameObject, spawnPoint, rotation, ObjectPoolManager.PoolType.Particle);
    }

    // ------------------ Lightning Line Connector Spawner ------------------
    public void SpawnLineLightning(Component sender, object data)
    {
        Debug.Log("SpawnLineLightning called");
        if (data is LightningDamageData lightningData)
        {
            GameObject lineConnectorObject = ObjectPoolManager.SpawnObject(_lineLightningPrefab, Vector3.zero, Quaternion.identity, ObjectPoolManager.PoolType.Projectile);
            LineConnector lightningVFX = lineConnectorObject.GetComponent<LineConnector>();
            if (lightningVFX != null)
            {
                if (lightningData.UsePlayerPosition)
                {
                    lightningVFX.SetPlayerOrigin(true);
                    //lightningVFX.SetTarget(lightningData.TargetPosition);
                    lightningVFX.SetPoints(_player.Position, lightningData.TargetPosition);
                }
                else
                {
                    //Debug log stating the origin and target
                    Debug.Log($"Lightning Origin: {lightningData.OriginPosition} Target: {lightningData.TargetPosition}");
                    //lightningVFX.SetOrigin(lightningData.OriginPosition);
                    //lightningVFX.SetTarget(lightningData.TargetPosition);
                    lightningVFX.SetPoints(lightningData.OriginPosition, lightningData.TargetPosition);
                }
            }
        }
    }
    // ------------------ Death Sprinkles ------------------
    public void SpawnDeathSprinkles(Vector3 deathPosition, short expPoints) // Handles how much xp is dropped
    {
        GameObject deathSprinkleInstance = ObjectPoolManager.SpawnObject(_deathSprinkles.gameObject, deathPosition, Quaternion.identity, ObjectPoolManager.PoolType.Particle);

        if (deathSprinkleInstance != null)
        {
            ParticleSystem pSystem = deathSprinkleInstance.GetComponent<ParticleSystem>();
            ParticleSystem.EmissionModule emissionModule = pSystem.emission;
            emissionModule.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0.0f, expPoints) });

            pSystem.Play();
        }
    }
}
