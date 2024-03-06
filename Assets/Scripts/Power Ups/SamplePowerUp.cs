using UnityEngine;

[CreateAssetMenu(fileName = "Sample Power Up", menuName = "Power Ups/Sample", order = 1)]
public class SamplePowerUp : PowerUpEffect
{
    public PowerupStats powerupStats; // Reference to PowerupStats
    public StatModifier[] modifiers; // Array of modifiers to apply

    public override void Apply()
    {
        foreach (StatModifier modifier in modifiers)
        {
            powerupStats.ApplyModifier(modifier);
        }
    }
}