using UnityEngine;

[CreateAssetMenu(fileName = "Lightning Damage Up", menuName = "Lightning Power Ups/Lightning Damage Up", order = 3)]
public class LightningDamageUp : PowerUpEffect
{
    public float amount;
    public PowerupStats powerupStats;
    public override void Apply()
    {
        powerupStats.LightningDamageMultiplier += amount;
        powerupStats.LightningLevel++;
    }
}
