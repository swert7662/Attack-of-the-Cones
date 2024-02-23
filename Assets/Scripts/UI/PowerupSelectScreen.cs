using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSelectScreen : MonoBehaviour
{
    [SerializeField] private GameObject choicePrefab; // The Choice prefab

    [SerializeField] private PowerupList playerPowerupList;
    [SerializeField] private PowerupList firePowerupList;
    [SerializeField] private PowerupList lightningPowerupList;

    [SerializeField] private float padding = 50f; // Padding between choices
    [SerializeField] private int numberOfChoicesToShow = 3; // Number of choices to show, adjustable in the Inspector

    private PowerupList selectedPowerupList;
    private float choiceWidth; // Cached width of a choice prefab

    private void Awake()
    {
        choiceWidth = choicePrefab.GetComponent<RectTransform>().rect.width;
    }

    private void OnEnable()
    {
        //DisplayChoices();
    }

    public void HandlePowerupCategorySelection(Component sender, object categoryObject)
    {
        if (categoryObject is string categoryString)
        {
            if (Enum.TryParse<PowerupList.PowerUpCategory>(categoryString, out var categoryEnum))
            {
                SetSelectedPowerupList(categoryEnum);
                DisplayChoices();
            }
            else
            {
                Debug.LogError($"Invalid category string: {categoryString}");
            }
        }
        else
        {
            Debug.LogError("Category object is not a string.");
        }
    }

    private void SetSelectedPowerupList(PowerupList.PowerUpCategory category)
    {
        switch (category)
        {
            case PowerupList.PowerUpCategory.Player:
                selectedPowerupList = playerPowerupList;
                break;
            case PowerupList.PowerUpCategory.Fire:
                selectedPowerupList = firePowerupList;
                break;
            case PowerupList.PowerUpCategory.Lightning:
                selectedPowerupList = lightningPowerupList;
                break;
            default:
                Debug.LogError($"No powerup list available for the category: {category}");
                selectedPowerupList = null;
                break;
        }
    }


    private void DisplayChoices()
    {
        ClearExistingChoices(); // Clear existing choices before displaying new ones

        int availablePowerupCount = Mathf.Min(numberOfChoicesToShow, selectedPowerupList.Powerups.Count);
        if (availablePowerupCount == 0)
        {
            Debug.LogError("No powerups available to display");
            return;
        }

        //List<Powerup> selectedPowerups = powerupList.SelectRandomPowerups(availablePowerupCount);
        List<PowerUpEffect> selectedPowerups = selectedPowerupList.SelectRandomPowerups(availablePowerupCount);

        float totalWidthNeededForChoicesOnly = availablePowerupCount * choiceWidth;
        float totalPaddingNeeded = (availablePowerupCount - 1) * padding;
        float totalWidthNeeded = totalWidthNeededForChoicesOnly + totalPaddingNeeded;
        float startPosition = -totalWidthNeeded / 2 + choiceWidth / 2;

        for (int i = 0; i < selectedPowerups.Count; i++)
        {
            GameObject choiceInstance = Instantiate(choicePrefab, transform);
            RectTransform choiceRT = choiceInstance.GetComponent<RectTransform>();

            // Calculate the position for each choice
            float positionX = startPosition + i * (choiceWidth + padding);
            choiceRT.anchoredPosition = new Vector2(positionX, 0);

            PowerUpSelect powerUpSelectScript = choiceInstance.GetComponent<PowerUpSelect>();
            if (powerUpSelectScript != null && selectedPowerups[i] != null)
            {
                powerUpSelectScript.InitializeWithPowerup(selectedPowerups[i]);
            }
            else { Debug.LogError("PowerUpSelect script not found or selected powerup is null"); }
        }
    }

    private void ClearExistingChoices()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }    
}
