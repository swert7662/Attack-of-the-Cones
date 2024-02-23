using UnityEngine;

[CreateAssetMenu(fileName = "Health Up", menuName = "Power Ups/Health Up", order = 3)]
public class HealthUpEffect : PowerUpEffect
{
    public int amount;
    public Player player;
    public override void Apply()
    {
        player.Health += amount;
    }
}
