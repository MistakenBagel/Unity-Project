using UnityEngine;

public class SceneIncrementButton : MonoBehaviour
{
    public LoadSceneButton playButtonScript; // Reference to Play Button's script
    public int maxSceneNumber = 6;

    public void AddSceneValue()
    {
        // Try to convert the scene name to a number
        int sceneNumber;

        if (int.TryParse(playButtonScript.sceneName, out sceneNumber))
        {
            sceneNumber++;

            // Clamp the value to max
            if (sceneNumber > maxSceneNumber)
                sceneNumber = maxSceneNumber;

            // Save the new value back
            playButtonScript.sceneName = sceneNumber.ToString();
        }
        else
        {
            Debug.LogWarning("Scene name is not a number.");
        }
    }
}