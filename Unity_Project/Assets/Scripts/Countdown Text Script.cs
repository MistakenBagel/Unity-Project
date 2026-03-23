using UnityEngine;
using TMPro;
using System.Collections;

public class TMPCountdown : MonoBehaviour
{
    private TextMeshProUGUI textComponent;

    public int startNumber = 3;
    public bool unfreezeAfterCountdown = true;

    void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        int current = startNumber;

        while (current > 0)
        {
            textComponent.text = current.ToString();
            yield return new WaitForSecondsRealtime(1f); // works even when frozen
            current--;
        }

        textComponent.text = "";

        // Unfreeze the game if needed
        if (unfreezeAfterCountdown)
        {
            Time.timeScale = 1f;
        }
    }
}