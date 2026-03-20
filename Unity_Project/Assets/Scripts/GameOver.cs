using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [Header("UI Panel to Enable on Death")]
    public GameObject panel;

    private bool isDead = false; // prevents double-triggering

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        // Check for hazards
        if (collision.gameObject.CompareTag("Hazard"))
        {
            Die();
        }
    }

    // This is what GooJunior will call
    public void Die()
    {
        if (isDead) return;

        isDead = true;

        // Disable player
        gameObject.SetActive(false);

        // Enable UI panel
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