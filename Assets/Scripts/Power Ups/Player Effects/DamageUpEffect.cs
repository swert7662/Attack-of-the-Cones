using UnityEngine;

[CreateAssetMenu(fileName = "Damage Up", menuName = "Power Ups/Damage Up", order = 1)]
public class DamageUpEffect : PowerUpEffect
{
    public int amount;
    //public PowerupStats powerupStats;
    public override void Apply()
    {
        powerupStats.DamageLevel += amount;
    }
}
