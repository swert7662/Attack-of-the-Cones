using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI shadowTimerText;

    public float totalTimeInSeconds = 0; // 5 minutes
    private float currentTime;
    public TextMeshProUGUI timerText;
    private int minutes;
    private int seconds;

    void Start()
    {
        currentTime = totalTimeInSeconds;
        UpdateTimerDisplay();
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        UpdateTimerDisplay();
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
        shadowTimerText.text = timerText.text;

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
}
