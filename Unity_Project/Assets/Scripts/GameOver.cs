using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [Header("UI Panel to Enable on Death")]
    public GameObject panel;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object we collided with has the tag "Hazard"
        if (collision.gameObject.CompareTag("Hazard"))
        {
            // Disable the player GameObject
            gameObject.SetActive(false);

            // Enable the UI panel
            if (panel != null)
            {
                panel.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Panel is not assigned in the inspector!");
            }
        }
    }
}