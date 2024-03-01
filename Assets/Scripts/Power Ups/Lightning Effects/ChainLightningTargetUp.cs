using UnityEngine;

[CreateAssetMenu(fileName = "Chain Lightning Target Up", menuName = "Lightning Power Ups/Chain Lightning Target Up", order = 2)]
public class ChainLightningTargetUp : PowerUpEffect
{
    public int amount;
    public PowerupStats powerupStats;
    public override void Apply()
    {
        powerupStats.ChainAmount += amount;
        powerupStats.LightningLevel++;
    }
}
