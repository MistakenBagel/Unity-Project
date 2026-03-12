using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public float startTime = 60f;   // Set the time in seconds in the Inspector
    private float currentTime;

    public Text timerText; // Optional UI Text to display the timer

    void Start()
    {
        currentTime = startTime;
        Time.timeScale = 1f; // Ensure the game runs normally at start
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            currentTime = 0;
            UpdateTimerDisplay();
            FreezeScene();
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void FreezeScene()
    {
        Time.timeScale = 0f; // Freezes the entire game
    }
}