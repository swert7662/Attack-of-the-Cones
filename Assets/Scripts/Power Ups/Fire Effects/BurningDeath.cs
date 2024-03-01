using UnityEngine;

[CreateAssetMenu(fileName = "Burning Death", menuName = "Fire Power Ups/Burning Death", order = 2)]
public class BurningDeath : PowerUpEffect
{
    public PowerupStats powerupStats;
    public PowerupList fireList;
    public override void Apply()
    {
        powerupStats.FloorFire = true;
        powerupStats.FireLevel++;
        fireList.RemovePowerup(this);
    }
}
