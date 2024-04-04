using System.Reflection;
using UnityEngine;
[CreateAssetMenu(fileName = "Powerup Stats", menuName = "ScriptableObjects/PowerupStats", order = 6)]
public class PowerupStats : Stats
{
    [SerializeField] private PowerupStats _baseStats;

    public int DamageLevel = 1;

    //Fire Powerups
    public int FireLevel = 0;

    public bool FloorFire;
    public bool EnemyExplode;
    public bool FireBullets;

    public float FireRange; // Add Powerup
    public float BurnDuration; // Add Powerup
    public float BurnTickRate; // Add Powerup
    public float FireDamageMultiplier; // Add Powerup

    // Lightning Powerups
    public int LightningLevel = 0;

    public bool LightningBullets;
    public bool TeslaCoil;
    public bool LightningStorm;
    
    public float TeslaCoilCooldown; // Add Powerup
    public float LightningStormCooldown; // Add Powerup
    public float LightningBulletChance; // Add Powerup

    public int ChainAmount;
    public float ArcRange; // Add Powerup
    public float StunDuration; // Add Powerup
    public float LightningDamageMultiplier; // Add Powerup
    

    public void ResetBaseValues()
    {
        if (_baseStats == null) { Debug.Log("No base stats found for PowerupStats"); return; }

        DamageLevel = 1;

        //Fire Powerups
        FireLevel = 0;

        FloorFire = _baseStats.FloorFire;
        EnemyExplode = _baseStats.EnemyExplode;
        FireBullets = _baseStats.FireBullets;

        FireRange = _baseStats.FireRange;
        BurnDuration = _baseStats.BurnDuration;
        BurnTickRate = _baseStats.BurnTickRate;
        FireDamageMultiplier = _baseStats.FireDamageMultiplier;

        // Lightning Powerups
        LightningLevel = 0;

        LightningBullets = _baseStats.LightningBullets;
        TeslaCoil = _baseStats.TeslaCoil;
        LightningStorm = _baseStats.LightningStorm;

        TeslaCoilCooldown = _baseStats.TeslaCoilCooldown;
        LightningStormCooldown = _baseStats.LightningStormCooldown;
        LightningBulletChance = _baseStats.LightningBulletChance;

        ChainAmount = _baseStats.ChainAmount;
        ArcRange = _baseStats.ArcRange;
        StunDuration = _baseStats.StunDuration;
        LightningDamageMultiplier = _baseStats.LightningDamageMultiplier;
    }
}
