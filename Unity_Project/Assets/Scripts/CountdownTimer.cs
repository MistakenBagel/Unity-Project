using UnityEngine;
using UnityEngine.UI; // Only needed if you want to display the timer in UI

public class CountdownTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float startTime = 60f; // Set your countdown time in seconds
    private float currentTime;

    [Header("UI (Optional)")]
    public Text timerText; // Assign a UI Text if you want to show the countdown

    private bool timerActive = true;

    void Start()
    {
        currentTime = startTime;
        UpdateTimerUI();
    }

    void Update()
    {
        // Check if any object with tag "Player" exists
        if (GameObject.FindWithTag("Player") == null)
        {
            timerActive = false;
            return; // Stop counting if no player is found
        }
        else
        {
            timerActive = true;
        }

        if (!timerActive) return;

        // Countdown
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0)
                currentTime = 0;

            UpdateTimerUI();
        }
        else
        {
            // Freeze the scene
            Time.timeScale = 0f;
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // Optional: Reset the timer and unfreeze the scene
    public void ResetTimer()
    {
        currentTime = startTime;
        Time.timeScale = 1f;
        timerActive = true;
        UpdateTimerUI();
    }
}