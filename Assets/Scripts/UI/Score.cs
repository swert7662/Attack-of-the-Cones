using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI shadowScoreText;
    public TextMeshProUGUI scoreText;

    private int currentScore = 0;

    void Start()
    {
        ResetScore();
    }

    public void AddPoint(Component sender, object data)
    {
        if (data is int)
        {
            currentScore += (int)data;
        }        
        UpdateScoreDisplay();
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        // Ensure the score doesn't exceed 999999
        currentScore = Mathf.Clamp(currentScore, 0, 999999);

        // Update the score display with the formatted string
        scoreText.text = $"Score : {currentScore:000000}";
        if (shadowScoreText != null)
        {
            shadowScoreText.text = scoreText.text;
        }
    }
}
