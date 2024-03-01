using UnityEngine;

[CreateAssetMenu(fileName = "Fire Damage Up", menuName = "Fire Power Ups/Fire Damage Up", order = 4)]
public class FireDamageUp : PowerUpEffect
{
    public float amount;
    public PowerupStats powerupStats;
    public override void Apply()
    {
        powerupStats.FireDamageMultiplier += amount;
        powerupStats.FireLevel++;
    }
}
