using UnityEngine;

[CreateAssetMenu(fileName = "Lightning Bullets", menuName = "Lightning Power Ups/Lightning Bullets", order = 1)]
public class LightningBullets : PowerUpEffect
{
    public PowerupStats powerupStats;
    public PowerupList lightningList;
    public PowerUpEffect chainLightning;
    public override void Apply()
    {
        powerupStats.LightningBullets = true;
        powerupStats.LightningLevel++;
        lightningList.RemovePowerup(this);
        lightningList.AddPowerup(chainLightning);
    }
}
