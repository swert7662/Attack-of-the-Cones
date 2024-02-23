using UnityEngine;
[CreateAssetMenu(fileName = "Powerup Stats", menuName = "ScriptableObjects/PowerupStats", order = 6)]
public class PowerupStats : ScriptableObject
{
    public int DamageLevel = 1;

    //Fire Powerups
    public int FireballLevel = 0;

    // Lightning Powerups
    public int ChainLightningLevel = 0;
    public int ChainLightningTargetCount = 2;
    public float LightningDamageMultiplier = 1f;

    public void ResetBaseValues()
    {
        DamageLevel = 1;

        FireballLevel = 0;

        ChainLightningLevel = 0;
        ChainLightningTargetCount = 2;
        LightningDamageMultiplier = 1f;
    }

}
