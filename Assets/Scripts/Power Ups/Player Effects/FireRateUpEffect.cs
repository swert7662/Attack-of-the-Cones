using UnityEngine;

[CreateAssetMenu(fileName = "Fire Rate Up", menuName = "Power Ups/Fire Rate Up", order = 2)]
public class FireRateUpEffect : PowerUpEffect
{
    public float amount;
    public Player player;
    public override void Apply()
    {
        player.FireRate *= amount;
    }
}
