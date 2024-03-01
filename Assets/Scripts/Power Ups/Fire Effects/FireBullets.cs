using UnityEngine;

[CreateAssetMenu(fileName = "Fire Bullets", menuName = "Fire Power Ups/Fire Bullets", order = 1)]
public class FireBullets : PowerUpEffect
{
    public PowerupStats powerupStats;
    public PowerupList fireList;
    public override void Apply()
    {
        powerupStats.FireBullets = true;
        powerupStats.FireLevel++;
        fireList.RemovePowerup(this);
    }
}
