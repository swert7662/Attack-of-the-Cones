using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpSelect : MonoBehaviour
{
    [SerializeField] private GameEvent powerUpSelectedEvent;
    [SerializeField] private Image _image;

    //private Powerup selectedPowerup; // Keep track of the selected powerup
    private PowerUpEffect selectedPowerup;

    public void Selection()
    {
        //Collectible.PowerUpType powerUpType = selectedPowerup.Type;
        //Debug.Log("Powerup selected: " + powerUpType);
        //powerUpSelectedEvent.Raise(this, powerUpType);
        if (selectedPowerup != null)
        {
            selectedPowerup.Apply();
            powerUpSelectedEvent.Raise();
        }
    }

    public void InitializeWithPowerup(PowerUpEffect powerup)
    {
        selectedPowerup = powerup;
        _image.sprite = powerup.Image;        
    }
}
