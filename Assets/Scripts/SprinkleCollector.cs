using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinkleCollector : MonoBehaviour
{
    private ParticleSystem _sprinkles;
    private Transform _collectorTransform;

    [SerializeField] private AudioClip _sprinkleSFX;

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
        Debug.Log("OnParticleSystemStopped");
        ObjectPoolManager.DespawnObject(gameObject);
    }

    private void PlaySFX(AudioClip clip)
    {
        float randomPitch = UnityEngine.Random.Range(0.8f, 1.2f);
        float randomVolume = UnityEngine.Random.Range(0.6f, .8f);
        AudioManager.Instance.PlaySound(clip, .4f, 1);
    }

    private void OnParticleTrigger()
    {
        
        
        int triggerParticles = _sprinkles.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);

        for (int i = 0; i < triggerParticles; i++)
        {
            PlaySFX(_sprinkleSFX);
            ParticleSystem.Particle p = _particles[i];
            p.remainingLifetime = 0;
            _particles[i] = p;
        }

        _sprinkles.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);
    }
}
