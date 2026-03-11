using UnityEngine;

public class SceneDecrementButton : MonoBehaviour
{
    public LoadSceneButton playButtonScript; // Reference to Play Button script
    public int minSceneNumber = 1;

    public void SubtractSceneValue()
    {
        int sceneNumber;

        if (int.TryParse(playButtonScript.sceneName, out sceneNumber))
        {
            sceneNumber--;

            // Clamp to minimum
            if (sceneNumber < minSceneNumber)
                sceneNumber = minSceneNumber;

            // Save new value
            playButtonScript.sceneName = sceneNumber.ToString();
        }
        else
        {
            Debug.LogWarning("Scene name is not a number.");
        }
    }
}