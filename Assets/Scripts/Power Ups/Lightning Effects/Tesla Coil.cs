using UnityEngine;

[CreateAssetMenu(fileName = "Tesla Coil", menuName = "Lightning Power Ups/Tesla Coil", order = 5)]
public class TeslaCoil : PowerUpEffect
{
    public PowerupStats powerupStats;
    public PowerupList lightningList;

    public override void Apply()
    {
        powerupStats.TeslaCoil = true;
        powerupStats.LightningLevel++;
        lightningList.RemovePowerup(this);
    }
}
