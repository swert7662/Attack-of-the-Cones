using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStorm : MonoBehaviour
{
    [SerializeField] private PlayerStats _player;
    [SerializeField] private PlayerParticles _playerParticles;
    [SerializeField] private PowerupStats _powerupStats;

    [SerializeField] private GameEvent _lightningDamageEvent;
    [SerializeField] private GameEvent _lightningArcEvent;
    [SerializeField] private List<GameObject> _lightningStormSourcePoints;

    public LayerMask enemyLayer;

    private float lastActivationTime = float.MinValue;
    private GameObject sourcePoint;
    private ParticleSystem particleEffect;

    private bool abilityReady = false;

    private void Update()
    {
        if (!_powerupStats.LightningStorm) { return; }

        if (!abilityReady)
        {
            CheckCooldown();
        }
        else if (abilityReady && Time.time >= lastActivationTime)
        {
            ActivateAbility();
        }
    }

    private void CheckCooldown()
    {
        bool isReady = _player.IsAlive && Time.time >= lastActivationTime + _powerupStats.LightningStormCooldown;

        if (isReady)
        {
            GetRandomPoint();
            particleEffect = sourcePoint.GetComponent<ParticleSystem>();
            particleEffect.Play();
            abilityReady = true;
            lastActivationTime = Time.time + 1f;
        }
    }

    private void ActivateAbility()
    {
        GameObject target = UtilityMethods.FindNextTargetWith<NewEnemy>(_player.Position, 15f, enemyLayer);
        if (target != null)
        {
            LightningStruck lightningStruck = target.AddComponent<LightningStruck>();
            int damage = (int)Mathf.Ceil(_powerupStats.DamageLevel + (4 * _powerupStats.LightningDamageMultiplier));
            lightningStruck.Initialize(damage,
                                       _powerupStats.ChainAmount,
                                       _powerupStats.ArcRange,
                                       _powerupStats.StunDuration,
                                       _lightningDamageEvent,
                                       _lightningArcEvent,
                                       enemyLayer);

            if (_lightningArcEvent != null)
            {
                // Now use this random source point in your LightningDamageData
                LightningDamageData lightningDamageData = new LightningDamageData(sourcePoint.transform.position, target.transform.position, false);
                _lightningArcEvent.Raise(this, lightningDamageData);
            }

            particleEffect.Stop();
            abilityReady = false;
            lastActivationTime = Time.time;
        }
    }

    private void GetRandomPoint()
    {
        // Choose a random index based on the length of your source points array
        int randomIndex = UnityEngine.Random.Range(0, _lightningStormSourcePoints.Count);
        sourcePoint = _lightningStormSourcePoints[randomIndex];
    }
}
