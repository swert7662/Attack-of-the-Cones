using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _particlePrefab;
    [SerializeField] private GameObject _particleSecondaryEffect;
    [SerializeField] private float _particalOffset;

    //private CapsuleCollider2D _collider;

    private void Awake()
    {
        //_collider = GetComponent<CapsuleCollider2D>();
    }

    //public void SpawnImpactParticles(Vector3 spawnPosition, Vector3 direction)
    //{
    //    Quaternion rotation = Quaternion.FromToRotation(Vector2.right, direction);
    //    ObjectPoolManager.SpawnObject(_particlePrefab, spawnPosition, rotation, ObjectPoolManager.PoolType.Particle);
    //}

    //public void SpawnExitParticles(Vector3 spawnPosition, Vector3 direction, CapsuleCollider2D collider)
    //{
    //    direction = -direction;
    //    Quaternion rotation = Quaternion.FromToRotation(Vector2.right, direction);
    //    spawnPosition += new Vector3(direction.x * (collider.bounds.extents.x + _particalOffset), direction.y * (collider.bounds.extents.y + _particalOffset));
    //    ObjectPoolManager.SpawnObject(_particleSecondaryEffect, spawnPosition, rotation, ObjectPoolManager.PoolType.Particle);
    //}
}
