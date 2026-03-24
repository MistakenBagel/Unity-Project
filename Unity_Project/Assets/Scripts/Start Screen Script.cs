using UnityEngine;
using System.Collections;

public class StartFreezeController : MonoBehaviour
{
    public GameObject startPanel; // Assign your "Start Panel" in Inspector
    public float freezeDuration = 3f;

    void Start()
    {
        // Freeze the game
        Time.timeScale = 0f;

        // Start the unfreeze process
        StartCoroutine(UnfreezeAfterDelay());
    }

    IEnumerator UnfreezeAfterDelay()
    {
        // Wait using real time (ignores timeScale)
        yield return new WaitForSecondsRealtime(freezeDuration);

        // Unfreeze the game
        Time.timeScale = 1f;

        // Disable the panel
        if (startPanel != null)
        {
            startPanel.SetActive(false);
        }
    }
}