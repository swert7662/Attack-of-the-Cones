using UnityEngine;

[CreateAssetMenu(fileName = "Speed Up", menuName = "Power Ups/Speed Up", order = 4)]
public class SpeedUpEffect : PowerUpEffect
{
    public float amount;
    public Player player;
    public override void Apply()
    {
        player.Speed *= amount;
    }
}
