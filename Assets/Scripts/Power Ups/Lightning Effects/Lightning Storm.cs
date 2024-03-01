using UnityEngine;

[CreateAssetMenu(fileName = "Lightning Storm", menuName = "Lightning Power Ups/Lightning Storm", order = 4)]
public class LightningStorm : PowerUpEffect
{
    public PowerupStats powerupStats;
    public PowerupList lightningList;
    public override void Apply()
    {
        powerupStats.LightningStorm = true;
        powerupStats.LightningLevel++;
        lightningList.RemovePowerup(this);
    }
}
