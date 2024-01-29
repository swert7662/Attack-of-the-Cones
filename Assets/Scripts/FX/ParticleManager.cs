using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _deathSprinkles;
    [SerializeField] private ParticleSystem _impactParticles;
    [SerializeField] private ParticleSystem _exitParticles;
    [SerializeField] private float _particalOffset = 0.5f;

    [SerializeField] private ParticleSystem _lightningStrike;

    private ParticleSystem _cachedDeathSprinkles;

    private void OnEnable()
    {
        NewEnemy.OnEnemyDeath += SpawnDeathSprinkles;

        Projectile.OnProjectileImpact += SpawnImpactParticles;
        Projectile.OnProjectileExit += SpawnExitParticles;

        ElectricSpawner.OnLightningStrike += SpawnLightningParticles;
    }

    private void OnDisable()
    {
        NewEnemy.OnEnemyDeath -= SpawnDeathSprinkles;

        Projectile.OnProjectileImpact -= SpawnImpactParticles;
        Projectile.OnProjectileExit -= SpawnExitParticles;

        ElectricSpawner.OnLightningStrike -= SpawnLightningParticles;
    }       

    private void Awake()
    {
        // Assuming the ParticleSystem is the main component of the prefab
        _cachedDeathSprinkles = _deathSprinkles.GetComponent<ParticleSystem>();
    }

    private void SpawnImpactParticles(Vector2 spawnPoint, Vector2 direction)
    {
        SpawnImpactExitParticles(_impactParticles.gameObject, spawnPoint, direction);
    }

    private void SpawnExitParticles(Vector2 spawnPoint, Vector2 direction, Vector3 extents)
    {
        SpawnImpactExitParticles(_exitParticles.gameObject, spawnPoint, direction, extents);
    }

    private void SpawnLightningParticles(Vector2 spawnPoint)
    {
        SpawnParticlesAtPoint(_lightningStrike.gameObject, spawnPoint);
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

    private void SpawnParticlesAtPoint(GameObject particlePrefab, Vector2 spawnPoint) // Spawn particles at a point
    {
        ObjectPoolManager.SpawnObject(particlePrefab, spawnPoint, Quaternion.identity, ObjectPoolManager.PoolType.Particle);
    }

    private void SpawnDeathSprinkles(Vector3 deathPosition, short exp) // Handles how much xp is dropped
    {
        GameObject deathSprinkleInstance = ObjectPoolManager.SpawnObject(_deathSprinkles.gameObject, deathPosition, Quaternion.identity, ObjectPoolManager.PoolType.Particle);

        if (deathSprinkleInstance != null)
        {
            ParticleSystem.EmissionModule emissionModule = _cachedDeathSprinkles.emission;
            emissionModule.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0.0f, exp) });

            _cachedDeathSprinkles.Play();
        }
    }
}
