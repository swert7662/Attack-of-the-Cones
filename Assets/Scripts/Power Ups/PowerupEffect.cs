using UnityEngine;

// This is now your base class for all power-up effects.
[CreateAssetMenu(fileName = "New Power Up", menuName = "Power Ups/NewPowerUp", order = 1)]
public class PowerUpEffect : ScriptableObject
{
    public Sprite Image;
    public PowerupStats powerupStats; // Reference to PowerupStats
    public StatModifier[] modifiers; // Array of modifiers to apply

    // New fields for list management
    public PowerupList removeFromList; // List from which this power-up should remove itself
    public PowerupList addToList; // List to which new effects should be added
    public PowerUpEffect[] effectsToAdd; // Effects to add to the addToList

    public virtual void Apply()
    {
        ApplyStatModifiers();
        RemoveFromList();
        AddEffectsToList();
    }

    protected void ApplyStatModifiers()
    {
        if (powerupStats != null)
        {
            foreach (StatModifier modifier in modifiers)
            {
                powerupStats.ApplyModifier(modifier);
            }
        }
    }

    protected void RemoveFromList()
    {
        if (removeFromList != null)
        {
            removeFromList.RemovePowerup(this);
        }
    }

    protected void AddEffectsToList()
    {
        if (addToList != null && effectsToAdd != null)
        {
            foreach (var effect in effectsToAdd)
            {
                addToList.AddPowerup(effect);
            }
        }
    }
}

[System.Serializable]
public class StatModifier
{
    public string statName; // Must match the field name in PowerupStats
    public float value; // The value to add or multiply
    public bool isAdditive; // true for additive, false for multiplicative
}
