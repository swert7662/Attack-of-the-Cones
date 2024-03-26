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

    [SerializeField] private List<Transform> _lightningStormSourcePoints;
    [SerializeField] private GameEvent _lightningDamageEvent;
    [SerializeField] private GameEvent _lightningArcEvent;
    [SerializeField] private GameEvent _teslaActiveEvent;

    [SerializeField] private Explosion ExplosionPrefab;
    [SerializeField] private BurnArea BurnAreaPrefab;

    [SerializeField] private PowerupList basePlayerPowerupList;
    [SerializeField] private PowerupList activePlayerPowerupList;
    [SerializeField] private PowerupList baseFirePowerupList;
    [SerializeField] private PowerupList activeFirePowerupList;
    [SerializeField] private PowerupList baseLightningPowerupList;
    [SerializeField] private PowerupList activeLightningPowerupList;

    public LayerMask enemyLayer;

    public GameObject Player;

    private bool teslaCoilReadySignaled = false;
    private float lastTeslaCoilActivationTime = float.MinValue;
    private float lastLightningStormActivationTime = float.MinValue;

    #region Awake, Start, Update, OnEnable, OnDisable
    private void Awake()
    {
        //Find the playerController as the gameobject for player always has one
        //Player = GameObject.Find("PlayerController");
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
        //InvokeRepeating(nameof(ActivateTimeBasedAbilities), 2.0f, 2.0f);
    }

    private void Update()
    {
        ActivateTimeBasedAbilities();
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
            BurnArea burnArea = Instantiate(BurnAreaPrefab, enemyDeathData.Position, Quaternion.identity);
            int damage = (int)Mathf.Ceil(_powerupStats.DamageLevel + (2 * _powerupStats.FireDamageMultiplier));
            burnArea.Initialization(damage,
                                    _powerupStats.FireRange,
                                    _powerupStats.BurnDuration,
                                    _powerupStats.BurnTickRate,
                                     enemyLayer);
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

    // --------------------- Time Based Abilities ---------------------
    #region Time Based Abilities
    private void ActivateTimeBasedAbilities()
    {
        bool isTeslaCoilReady = _powerupStats.TeslaCoil && _player.IsAlive && Time.time >= lastTeslaCoilActivationTime + _powerupStats.TeslaCoilCooldown;

        if (isTeslaCoilReady)
        {
            if (!teslaCoilReadySignaled)
            {
                Debug.Log("Tesla coil ready!");
                _teslaActiveEvent.Raise(); // Signal readiness
                teslaCoilReadySignaled = true;
            }

            // Start the coroutine to handle target acquisition and effects after a delay
            StartCoroutine(ActivateTeslaCoilAfterDelay());
        }

        if (_powerupStats.LightningStorm && _player.IsAlive && Time.time >= lastLightningStormActivationTime + _powerupStats.LightningStormCooldown)
        {
            lastLightningStormActivationTime = Time.time;
            Debug.Log("Lightning storm activated");
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
                    // Choose a random index based on the length of your source points array
                    int randomIndex = UnityEngine.Random.Range(0, _lightningStormSourcePoints.Count);
                    Transform randomSourcePoint = _lightningStormSourcePoints[randomIndex];

                    // Now use this random source point in your LightningDamageData
                    LightningDamageData lightningDamageData = new LightningDamageData(randomSourcePoint.position, target.transform.position, false);
                    _lightningArcEvent.Raise(this, lightningDamageData);
                }
            }
        }
    }

    private IEnumerator ActivateTeslaCoilAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);

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
            Debug.Log("Tesla coil activated!");
            lastTeslaCoilActivationTime = Time.time;
            _teslaActiveEvent.Raise(); // Acknowledge use
            teslaCoilReadySignaled = false; // Reset the signal for next availability
        }
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
        Burning burningEffect = target.GetComponent<Burning>();
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
    }
    #endregion
    
}

