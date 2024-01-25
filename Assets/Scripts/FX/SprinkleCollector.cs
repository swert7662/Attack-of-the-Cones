using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinkleCollector : MonoBehaviour
{
    private ParticleSystem _sprinkles;
    private Transform _collectorTransform;
    private int _collectedParticleCount = 0;

    private List<ParticleSystem.Particle> _particles = new List<ParticleSystem.Particle>();

    public static event Action<int> OnSprinklePickup;

    private void Awake()
    {
        _sprinkles = GetComponent<ParticleSystem>();
        _collectorTransform = GameManager.Instance._playerTransform;
        if (_collectorTransform != null)
        {
            _sprinkles.trigger.AddCollider(_collectorTransform);
        }
    }

    private void OnParticleTrigger()
    {
        int triggerParticles = _sprinkles.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);

        OnSprinklePickup?.Invoke(triggerParticles);

        for (int i = 0; i < triggerParticles; i++)
        {
            ParticleSystem.Particle p = _particles[i];
            p.remainingLifetime = 0;
            _particles[i] = p;
            _collectedParticleCount ++;
        }

        _sprinkles.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);

        if (_collectedParticleCount >= _sprinkles.main.maxParticles)
        {
            Debug.Log("Despawning particle system");
            ObjectPoolManager.DespawnObject(gameObject);
        }
    }
    private void DespawnParticleSystem()
    {
        ObjectPoolManager.DespawnObject(gameObject);
    }

    private void OnEnable()
    {
        ResetCollector();
    }

    private void OnDisable()
    {
        ResetCollector();
    }

    private void ResetCollector()
    {
        _collectedParticleCount = 0;
        _particles.Clear();
    }
}
