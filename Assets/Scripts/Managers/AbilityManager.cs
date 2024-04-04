using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class AbilityManager : MonoBehaviour
{
    
    [SerializeField] private PlayerStats _player;
    [SerializeField] private PowerupStats _powerupStats;
    [SerializeField] private ParticleSystemForceField _suctionField;

    [SerializeField] private GameEvent _lightningDamageEvent;
    [SerializeField] private GameEvent _lightningArcEvent;    

    [SerializeField] private Explosion ExplosionPrefab;
    [SerializeField] private BurnArea BurnAreaPrefab;

    [SerializeField] private GameObject _burningEffectPrefab;

    [SerializeField] private PowerupList basePlayerPowerupList;
    [SerializeField] private PowerupList activePlayerPowerupList;
    [SerializeField] private PowerupList baseFirePowerupList;
    [SerializeField] private PowerupList activeFirePowerupList;
    [SerializeField] private PowerupList baseLightningPowerupList;
    [SerializeField] private PowerupList activeLightningPowerupList;    

    public GameObject Player;
    public LayerMask enemyLayer;

    #region Awake, Start, Update, OnEnable, OnDisable
    private void Awake()
    {
        if (Player == null) { Debug.LogError("Couldnt find a PlayerController!"); }
        if (_player == null) { Debug.LogError("Player is null!"); }
        if (_powerupStats == null) { Debug.LogError("PowerupStats is null!"); }
        if (_suctionField == null) { Debug.LogError("SuctionField is null!"); }
    }

    private void Start()
    {
        _powerupStats.ResetBaseValues();
        activePlayerPowerupList.Powerups = new List<PowerUpEffect>(basePlayerPowerupList.Powerups);
        activeFirePowerupList.Powerups = new List<PowerUpEffect>(baseFirePowerupList.Powerups);
        activeLightningPowerupList.Powerups = new List<PowerUpEffect>(baseLightningPowerupList.Powerups);

        _suctionField.endRange = _player.SuctionRange;
    }

    private void OnEnable()
    {
        Projectile.OnAdditionalEffectsTrigger += HandleAdditionalEffectsTrigger;
    }

    private void OnDisable()
    {
        Projectile.OnAdditionalEffectsTrigger -= HandleAdditionalEffectsTrigger;
    }
    #endregion
    
    #region Handle Events
    private void HandleAdditionalEffectsTrigger(GameObject target, Vector2 position)
    {
        if (_powerupStats.LightningBullets)
        {
            LightningBulletHit(target, position);
        }
        if (_powerupStats.FireBullets)
        {
            FireBulletHit(target);
        }
    }

    public void HandleOnDeathEvents(Component sender, object data)
    {
        if (data is not EnemyDeathData) { Debug.LogError("HandleOnDeathEvents failed: Data is not of type EnemyDeathData"); return; }
        EnemyDeathData enemyDeathData = (EnemyDeathData)data;

        if (_powerupStats.EnemyExplode)
        {
            Explosion explosion = Instantiate(ExplosionPrefab, enemyDeathData.Position, Quaternion.identity);
            int damage = (int)Mathf.Ceil(_powerupStats.DamageLevel + (3 * _powerupStats.FireDamageMultiplier));
            explosion.Initialization(damage, 
                                     _powerupStats.FireRange, 
                                     enemyLayer);
        }

        if (_powerupStats.FloorFire)
        {
            Instantiate(BurnAreaPrefab, enemyDeathData.Position, Quaternion.identity);
        }
    }
    public void HandlePowerupActivated()
    {
        CalculateDamage();
    }

    private void CalculateDamage()
    {
        float calculatedDamage = _powerupStats.DamageLevel;

        // Use Mathf.Ceil to round up to the nearest integer
        int newDamage = (int)Mathf.Ceil(calculatedDamage);

        _player.Damage = newDamage;        
        _suctionField.endRange = _player.SuctionRange;
    }
    #endregion

    // --------------------- On Hit Effects ---------------------
    #region On Hit Effects
    private void LightningBulletHit(GameObject target, Vector2 position)
    {
        if (UnityEngine.Random.value <= _powerupStats.LightningBulletChance)
        {
            LightningStruck lightningStruck = target.AddComponent<LightningStruck>();
            int damage = (int)Mathf.Ceil(_powerupStats.DamageLevel + _powerupStats.LightningDamageMultiplier);
            lightningStruck.Initialize(damage,
                                       _powerupStats.ChainAmount,
                                       _powerupStats.ArcRange,
                                       _powerupStats.StunDuration,
                                       _lightningDamageEvent,
                                       _lightningArcEvent,
                                       enemyLayer);
        }
    }

    private void FireBulletHit(GameObject target)
    {
        bool alreadyBurning = false;

        for (int i = target.transform.childCount - 1; i >= 0; i--)
        {
            if (target == null) { return; } 

            Burning burningEffect = target.transform.GetChild(i).GetComponent<Burning>();
            if (burningEffect != null)
            {
                burningEffect.ResetTimer();
                alreadyBurning = true;
                break; // Exit the loop as we've found an existing effect
            }
        }

        if (!alreadyBurning)
        {
            Instantiate(_burningEffectPrefab, target.transform.position, Quaternion.identity, target.transform);
        }
        /*Burning burningEffect = target.GetComponent<Burning>();
        if (burningEffect == null)
        {
            int burnDamage = (int)Mathf.Ceil(_powerupStats.DamageLevel * 0.5f * _powerupStats.FireDamageMultiplier);
            burningEffect = target.AddComponent<Burning>();
            burningEffect.Initialization(burnDamage,
                                         _powerupStats.BurnDuration,
                                         _powerupStats.BurnTickRate);
        }
        else
        {
            burningEffect.ResetTimer();
        }
        */
    }
    #endregion
    
}

