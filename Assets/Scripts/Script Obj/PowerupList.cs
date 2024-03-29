using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Powerup List", menuName = "ScriptableObjects/PowerupList", order = 5)]

public class PowerupList : ScriptableObject
{
    public PowerUpCategory Category;
    public List<PowerUpEffect> Powerups;  

    public PowerUpEffect SelectRandomPowerup()
    {
        if (Powerups == null || Powerups.Count == 0)
        {
            Debug.LogError("No powerups available in the list.");
            return null;
        }

        int randomIndex = Random.Range(0, Powerups.Count);
        return Powerups[randomIndex];
    }

    public List<PowerUpEffect> SelectRandomPowerups(int count)
    {
        List<PowerUpEffect> selectedPowerups = new List<PowerUpEffect>();
        List<PowerUpEffect> availablePowerups = new List<PowerUpEffect>(this.Powerups);

        for (int i = 0; i < count && availablePowerups.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availablePowerups.Count);
            selectedPowerups.Add(availablePowerups[randomIndex]);
            availablePowerups.RemoveAt(randomIndex);
        }

        return selectedPowerups;
    }
    public void AddPowerup(PowerUpEffect powerUpEffect)
    {
        if (!Powerups.Contains(powerUpEffect))
        {
            Powerups.Add(powerUpEffect);
        }
    }

    public void RemovePowerup(PowerUpEffect powerUpEffect)
    {
        if (Powerups.Contains(powerUpEffect))
        {
            Powerups.Remove(powerUpEffect);
        }
    }

    public enum PowerUpCategory
    {
        None,
        Player,
        Fire,
        Lightning
    }
}
