using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    public GameObject panel; // Assign the Panel in the Inspector

    public void ResumeGame()
    {
        // Unfreeze the scene
        Time.timeScale = 1f;

        // Disable the panel
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}