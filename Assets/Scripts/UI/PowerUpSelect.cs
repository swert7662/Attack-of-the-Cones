using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpSelect : MonoBehaviour
{
    [SerializeField] private GameEvent powerUpSelectedEvent;

    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private Image _image;

    private PowerUpEffect selectedPowerup;

    public void Selection()
    {
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
        _title.text = StringTrimmer(powerup.ToString());
        _description.text = powerup.Description;
    }

    private static string StringTrimmer(string text)
    {
        int index = text.IndexOf("("); // Find the index of the opening parenthesis
        if (index != -1)
        {
            text = text.Substring(0, index).Trim(); // Keep only the text before the parenthesis
        }
        return text;
    }
}
