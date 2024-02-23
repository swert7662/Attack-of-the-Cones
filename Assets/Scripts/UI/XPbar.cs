using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    [SerializeField] private Image _leftFill, _centerFill, _rightFill;
    [SerializeField] private TextMeshProUGUI _levelText, _shadowLevelText;
    [SerializeField] private FloatVariable _playerCurrentXP;
    [SerializeField] private float maxXP;
    [SerializeField] GameEvent _levelUpEvent;

    private float LeftWidth, CenterWidth, RightWidth, TotalWidth;
    //private float currentXP = 0;
    private float currentLevel = 1;

    private void Awake()
    {
        _playerCurrentXP.Value = 0;
        LeftWidth = _leftFill.rectTransform.rect.width;
        CenterWidth = _centerFill.rectTransform.rect.width;
        RightWidth = _rightFill.rectTransform.rect.width;
        TotalWidth = LeftWidth + CenterWidth + RightWidth;
    }

    // Call this method to update the XP and the bar's fill levels
    public void UpdateXP()
    {
        UpdateBarFills();

        if (_playerCurrentXP.Value >= maxXP)
        {
            LevelUp();
            _levelUpEvent.Raise(this, "Player");
        }
    }
    public void LevelUp()
    {
        _playerCurrentXP.Value -= maxXP; // Reset current XP, keeping any overflow
        maxXP += 25; // Increase max XP by a fixed amount (e.g., 50)
        currentLevel++;

        _levelText.text = "Lvl " + currentLevel.ToString();
        _shadowLevelText.text = "Lvl " + currentLevel.ToString();

        UpdateBarFills(); // Update the bar to reflect the new level
    }

    private void UpdateBarFills()
    {
        float xpPercentage = _playerCurrentXP.Value / maxXP;
        float filledWidth = xpPercentage * TotalWidth;

        if (filledWidth < LeftWidth)
        {
            _leftFill.fillAmount = Mathf.Clamp(filledWidth / LeftWidth, 0, 1);
            _centerFill.fillAmount = 0;
            _rightFill.fillAmount = 0;
        }
        else if (filledWidth < LeftWidth + CenterWidth)
        {
            _leftFill.fillAmount = 1;
            _centerFill.fillAmount = Mathf.Clamp((filledWidth - LeftWidth) / CenterWidth, 0, 1);
            _rightFill.fillAmount = 0;
        }
        else
        {
            _leftFill.fillAmount = 1;
            _centerFill.fillAmount = 1;
            _rightFill.fillAmount = Mathf.Clamp((filledWidth - LeftWidth - CenterWidth) / RightWidth, 0, 1);
        }

        // Level up logic can go here, if applicable
    }
}
