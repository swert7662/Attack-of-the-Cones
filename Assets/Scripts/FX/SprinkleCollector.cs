using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinkleCollector : MonoBehaviour
{
    [SerializeField] private PlayerStats _player;
    [SerializeField] private FloatVariable _playerCurrentXP;
    [SerializeField] private GameEvent _xpPickup;

    private ParticleSystem _sprinkles;
    private int _collectedParticleCount = 0;

    private List<ParticleSystem.Particle> _particles = new List<ParticleSystem.Particle>();

    private void Awake()
    {
        _sprinkles = GetComponent<ParticleSystem>();        

        if (!_player){ Debug.LogError("Player not found by SprinkleCollector"); }

        _sprinkles.trigger.AddCollider(_player.Collider);

        if (!_playerCurrentXP) { Debug.LogError("PlayerCurrentXP not found by SprinkleCollector"); }        
    }

    private void OnParticleTrigger()
    {
        if (_player.IsAlive == false) { return; }

        int triggerParticles = _sprinkles.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);

        _playerCurrentXP.Value += triggerParticles;        

        for (int i = 0; i < triggerParticles; i++)
        {
            _xpPickup.Raise(this, 1);
            ParticleSystem.Particle p = _particles[i];
            p.remainingLifetime = 0;
            _particles[i] = p;
            _collectedParticleCount ++;
        }

        _sprinkles.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);

        short count = _sprinkles.emission.GetBurst(0).maxCount;
        if (_collectedParticleCount >= (int)count)
        {
            DespawnParticleSystem();
        }
    }
    private void DespawnParticleSystem()
    {
        Debug.Log("Despawning " + this.ToString());
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
