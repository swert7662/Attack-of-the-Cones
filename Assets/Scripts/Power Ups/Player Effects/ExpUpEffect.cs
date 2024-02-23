using UnityEngine;

[CreateAssetMenu(fileName = "Exp Up", menuName = "Power Ups/Exp Up", order = 6)]
public class ExpUpEffect : PowerUpEffect
{
    public short amount;
    public Player player;
    public override void Apply()
    {
        player.BonusXP += amount;
    }
}
