using UnityEngine;
[CreateAssetMenu(fileName = "Powerup Stats", menuName = "ScriptableObjects/PowerupStats", order = 6)]
public class PowerupStats : ScriptableObject
{
    public int DamageLevel = 1;

    //Fire Powerups
    public int FireLevel = 0;

    public bool FloorFire = false;
    public bool EnemyExplode = false;
    public bool FireBullets = false;

    public float FireRange = 1f; // Add Powerup
    public float BurnDuration = 1f; // Add Powerup
    public float BurnTickRate = 0.5f; // Add Powerup
    public float FireDamageMultiplier = 1f; // Add Powerup

    // Lightning Powerups
    public int LightningLevel = 0;

    public bool LightningBullets = false;
    public bool TeslaCoil = false;
    public bool LightningStorm = false;
    
    public float TeslaCoilCooldown = 3f; // Add Powerup
    public float LightningStormCooldown = 5f; // Add Powerup
    public float LightningBulletChance = .2f; // Add Powerup

    public int ChainAmount = 0;
    public float ArcRange = 5f; // Add Powerup
    public float StunDuration = .2f; // Add Powerup
    public float LightningDamageMultiplier = 1f; // Add Powerup
    

    public void ResetBaseValues()
    {
        DamageLevel = 1;

        //Fire Powerups
        FireLevel = 0;

        FloorFire = false;
        EnemyExplode = false;
        FireBullets = false;

        FireRange = 1f;
        BurnDuration = 1f;
        BurnTickRate = 0.5f;
        FireDamageMultiplier = 1f;

        // Lightning Powerups
        LightningLevel = 0;

        LightningBullets = false;
        TeslaCoil = false;
        LightningStorm = false;

        TeslaCoilCooldown = 3f;
        LightningStormCooldown = 5f;
        LightningBulletChance = .2f;
        
        ChainAmount = 0;
        ArcRange = 5f;
        StunDuration = .2f;
        LightningDamageMultiplier = 1f;
    }

}
