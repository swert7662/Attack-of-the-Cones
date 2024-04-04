using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaCoil : MonoBehaviour
{
    [SerializeField] private PlayerStats _player;
    [SerializeField] private PlayerParticles _playerParticles;
    [SerializeField] private PowerupStats _powerupStats;

    [SerializeField] private GameEvent _lightningDamageEvent;
    [SerializeField] private GameEvent _lightningArcEvent;

    public LayerMask enemyLayer;

    private float lastTeslaCoilActivationTime = float.MinValue;

    private bool abilityReady = false;

    private void Update()
    {
        if (!_powerupStats.TeslaCoil) { return; }

        if (!abilityReady)
        {
            CheckCooldown();
        }
        else if (abilityReady && Time.time >= lastTeslaCoilActivationTime)
        {
            ActivateAbility();
        }
    }

    private void CheckCooldown()
    {
        bool isTeslaCoilReady =  _player.IsAlive && Time.time >= lastTeslaCoilActivationTime + _powerupStats.TeslaCoilCooldown;

        if (isTeslaCoilReady)
        {
            _playerParticles.TogglePlayerElectricParticles();
            abilityReady = true;
            lastTeslaCoilActivationTime = Time.time + .3f;
        }
    }

    private void ActivateAbility()
    {
        GameObject target = UtilityMethods.FindNextTargetWith<NewEnemy>(_player.Position, _powerupStats.ArcRange, enemyLayer);
        if (target != null)
        {
            LightningStruck lightningStruck = target.AddComponent<LightningStruck>();
            int damage = (int)Mathf.Ceil(_powerupStats.DamageLevel + (2 * _powerupStats.LightningDamageMultiplier));
            lightningStruck.Initialize(damage,
                                       _powerupStats.ChainAmount,
                                       _powerupStats.ArcRange,
                                       _powerupStats.StunDuration,
                                       _lightningDamageEvent,
                                       _lightningArcEvent,
                                       enemyLayer);

            if (_lightningArcEvent != null)
            {
                LightningDamageData lightningDamageData = new LightningDamageData(_player.Position, target.transform.position, true);
                _lightningArcEvent.Raise(this, lightningDamageData);
            }

            _playerParticles.TogglePlayerElectricParticles();
            abilityReady = false;
            lastTeslaCoilActivationTime = Time.time;            
        }
    }
}
