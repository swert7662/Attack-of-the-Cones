using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    
    [SerializeField] private ParticleSystem _deathSprinkles;
    [SerializeField] private ParticleSystem _impactParticles;
    [SerializeField] private ParticleSystem _exitParticles;
    [SerializeField] private float _particalOffset = 0.5f;

    [SerializeField] private ParticleSystem _lightningStrike;

    //private ParticleSystem _cachedDeathSprinkles;

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

    public void SpawnDeathSprinkles(Component sender, object data) // Handles how much xp is dropped
    {
        if (data is EnemyDeathData)
        {
            EnemyDeathData deathData = (EnemyDeathData)data;
            Vector3 deathPositon = deathData.Position;
            // Debug statement showing the amount of xp dropped in deathData and the player's bonus xp
            Debug.Log($"Death Sprinkles: {deathData.ExpPoints} + {_player.BonusXP}");
            short expPoints = (short)(deathData.ExpPoints + _player.BonusXP);
        
            GameObject deathSprinkleInstance = ObjectPoolManager.SpawnObject(_deathSprinkles.gameObject, deathPositon, Quaternion.identity, ObjectPoolManager.PoolType.Particle);

            if (deathSprinkleInstance != null)
            {
                ParticleSystem pSystem = deathSprinkleInstance.GetComponent<ParticleSystem>();
                ParticleSystem.EmissionModule emissionModule = pSystem.emission;
                emissionModule.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0.0f, expPoints) });

                pSystem.Play();
            }
        }
        else { Debug.LogWarning("SpawnDeathSprinkles Failed : Data is not of type EnemyDeathData");}
    }
}
