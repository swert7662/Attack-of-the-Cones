using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float totalTimeInSeconds = 300; // 5 minutes
    private float currentTime;
    public TextMeshProUGUI timerText;
    private bool isTimerRunning;
    private int minutes;
    private int seconds;

    void Start()
    {
        currentTime = totalTimeInSeconds;
        UpdateTimerDisplay();
        StartTimer();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();

            if (currentTime <= 0)
            {
                currentTime = 0;
                isTimerRunning = false;
                timerText.text = "Time's Up!";
                // Implement difficulty level change or any other event here
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        
        if (currentTime > 60)
        {
            minutes = Mathf.FloorToInt(currentTime / 60);
        }
        else
        {
            minutes = 0;
        }
        
        seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);

        //float milliseconds = (currentTime % 1) * 1000;

        //if (currentTime > 30)
        //{
        //    timerText.text = string.Format("{0:0} : {1:00}", minutes, seconds);
        //}
        //else
        //{
        //    timerText.text = string.Format("0 : {0:00}.", seconds) + "<size=50%><voffset=-0.2em>" + string.Format("{0:00}", (int)milliseconds) + "</voffset></size>";
        //}
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void PauseTimer()
    {
        isTimerRunning = false;
    }

    // Call this from GameManager when game pauses
    public void ToggleTimer(bool isPaused)
    {
        isTimerRunning = !isPaused;
    }
}