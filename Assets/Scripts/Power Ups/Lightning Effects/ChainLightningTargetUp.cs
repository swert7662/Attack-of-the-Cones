using UnityEngine;

[CreateAssetMenu(fileName = "Chain Lightning Target Up", menuName = "Lightning Power Ups/Chain Lightning Target Up", order = 1)]
public class ChainLightningTargetUp : PowerUpEffect
{
    public int amount;
    public PowerupStats powerupStats;
    public override void Apply()
    {
        powerupStats.ChainLightningTargetCount += amount;
    }
}
