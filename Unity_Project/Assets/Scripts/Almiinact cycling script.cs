using UnityEngine;

public class PanelCycler : MonoBehaviour
{
    public GameObject[] panels; // Assign 7 panels in Inspector

    private int currentIndex = -1;

    public void OnButtonPressed()
    {
        if (panels.Length == 0) return;

        // Disable all panels first
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        // Move to next index
        currentIndex = (currentIndex + 1) % panels.Length;

        // Enable only the current panel
        panels[currentIndex].SetActive(true);
    }
}