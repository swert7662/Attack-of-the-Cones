using UnityEngine;

[CreateAssetMenu(fileName = "New Power Up", menuName = "Power Ups/NewPowerUp", order = 2)]
public class PlayerPowerUpEffect : PowerUpEffect
{
    public PlayerStats playerStats; // Reference to Player stats

    protected override void ApplyStatModifiers()
    {
        // Apply modifiers to powerupStats if available
        base.ApplyStatModifiers(); // Call the base method in case there are non-player specific stats to apply

        // Now, apply modifiers to playerStats
        if (playerStats != null)
        {
            foreach (StatModifier modifier in modifiers)
            {
                // Assuming playerStats has a similar ApplyModifier method or logic to modify player-specific stats
                playerStats.ApplyModifier(modifier);
            }
        }
    }
}
