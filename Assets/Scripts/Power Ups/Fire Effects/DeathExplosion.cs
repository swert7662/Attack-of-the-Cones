using UnityEngine;

[CreateAssetMenu(fileName = "Death Explosion", menuName = "Fire Power Ups/Death Explosion", order = 3)]
public class DeathExplosion : PowerUpEffect
{
    public PowerupStats powerupStats;
    public PowerupList fireList;
    public override void Apply()
    {
        powerupStats.EnemyExplode = true;
        powerupStats.FireLevel++;
        fireList.RemovePowerup(this);
    }
}
