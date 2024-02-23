using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowerupManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PowerupStats _powerupStats;
    [SerializeField] private ParticleSystemForceField _suctionField;

    [SerializeField] private GameObject _chainLightning;
    [SerializeField] private Projectile _fireball;

    private void Awake()
    {
        if (_player == null) { Debug.LogError("Player is null!"); }
        if (_powerupStats == null) { Debug.LogError("PowerupStats is null!"); }
        if (_suctionField == null) { Debug.LogError("SuctionField is null!"); }
    }

    private void Start()
    {
        _powerupStats.DamageLevel = 1;
        _powerupStats.FireballLevel = 0;
        _powerupStats.ChainLightningLevel = 0;

        _suctionField.endRange = _player.BaseSuctionRange;
    }

    private void OnEnable()
    {
        Projectile.OnAdditionalEffectsTrigger += HandleAdditionalEffectsTrigger;
        Collectible.OnPowerupPickup += HandlePowerupPickup;
    }

    private void OnDisable()
    {
        Projectile.OnAdditionalEffectsTrigger -= HandleAdditionalEffectsTrigger;
        Collectible.OnPowerupPickup -= HandlePowerupPickup;
    }

    public void HandlePowerupPickup(Collectible.PowerUpType powerUp)
    {
        switch (powerUp)
        {
            case Collectible.PowerUpType.Damage:
                _powerupStats.DamageLevel++;
                break;
            case Collectible.PowerUpType.FireRate:
                _player.FireRate *= 1.2f;
                break;
            case Collectible.PowerUpType.Health:
                _player.Health += 50;
                break;
            case Collectible.PowerUpType.Speed:
                _player.Speed *= 1.2f;
                break;
            case Collectible.PowerUpType.Suction:
                _suctionField.endRange *= 1.3f;
                break;   
            case Collectible.PowerUpType.ExpUp:
                _player.BonusXP += 2;
                break;
            case Collectible.PowerUpType.ChainLightning:
                HandleChainLightningPickup();
                break;
            case Collectible.PowerUpType.Fireball:
                HandleFireballPickup();
                break;
            default:
                Debug.LogError("Powerup type not handled");
                break;
        }
        CalculateDamage();
    }
    public void HandlePowerupSelection(Component sender, object powerUp)
    {
        if (powerUp is not Collectible.PowerUpType)
        {
            Debug.LogError("Powerup type not handled");
        }

        HandlePowerupPickup((Collectible.PowerUpType)powerUp);
    }

    public void CalculateDamage()
    {
        // Ensure the base multiplier is 1, and it increases by 1.5 for each fireball level
        float fireMultiplier = 1 + (_powerupStats.FireballLevel * 1.5f);
        float calculatedDamage = _powerupStats.DamageLevel * fireMultiplier;

        // Use Mathf.Ceil to round up to the nearest integer
        int newDamage = (int)Mathf.Ceil(calculatedDamage);

        _player.Damage = newDamage;
        _suctionField.endRange = _player.SuctionRange;
    }

    private void HandleChainLightningPickup()
    {
        if(_powerupStats.ChainLightningLevel <= 5)
        {
            _powerupStats.ChainLightningLevel++;
        }
    }
    private void HandleFireballPickup()
    {
        if (_powerupStats.FireballLevel == 0) 
        { 
            _player.SetProjectile(_fireball); 
        }

        _powerupStats.FireballLevel++;
    }

    private void HandleAdditionalEffectsTrigger(GameObject target, Vector2 position)
    {
        if (_powerupStats.ChainLightningTargetCount > 0)
        {
            ChainLightning(target, position);
        }
    }

    private void ChainLightning(GameObject target, Vector2 position)
    {
        if (GameManager.Instance.IsCooldownElapsed("ElectricSpawner", 2f)) // 1f is the cooldown duration in seconds
        {
            GameObject lightningGO = ObjectPoolManager.SpawnObject(_chainLightning, position, Quaternion.identity, ObjectPoolManager.PoolType.Projectile);
            lightningGO.GetComponent<ChainLightningSpawner>().StartChainAttack(target);
        }
    }
}
