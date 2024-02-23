using UnityEngine;

[CreateAssetMenu(fileName = "Suction Up", menuName = "Power Ups/Suction Up", order = 5)]
public class SuctionUpEffect : PowerUpEffect
{
    public float amount;
    public Player player;
    public override void Apply()
    {
        player.SuctionRange *= amount; 
    }
}
