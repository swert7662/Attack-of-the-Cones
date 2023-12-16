using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinkleCollector : MonoBehaviour
{
    private ParticleSystem _sprinkles;
    private Transform _collectorTransform;

    private List<ParticleSystem.Particle> _particles = new List<ParticleSystem.Particle>();

    private void Awake()
    {
        _sprinkles = GetComponent<ParticleSystem>();
        _collectorTransform = GameManager.Instance._playerTransform;
        if (_collectorTransform != null)
        {
            _sprinkles.trigger.AddCollider(_collectorTransform);
        }
    }

    private void OnParticleSystemStopped()
    {
        ObjectPoolManager.DespawnObject(gameObject);
    }

    private void OnParticleTrigger()
    {
        Debug.Log("OnParticleTrigger");

        int triggerParticles = _sprinkles.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);

        for (int i = 0; i < triggerParticles; i++)
        {
            ParticleSystem.Particle p = _particles[i];
            p.remainingLifetime = 0;
            _particles[i] = p;
        }

        _sprinkles.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);
    }
}
