using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMenuController : MonoBehaviour
{
    public GameObject NoScenesWarningLabel;
    public GameObject SceneButtonPrefab;
    public Transform SceneButtonParent;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            // Get the scene name
            string sceneName = "";
            try
            {
                sceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            }
            catch
            {
                // If it didn't work, just say the scene number
                sceneName = "Scene " + (i + 1);
            }

            // Create the scene button
            GameObject sceneButton = Instantiate(SceneButtonPrefab, SceneButtonParent);

            // Set the button text to match the scene name
            SceneButtonBehavior sceneButtonBehavior = sceneButton.GetComponent<SceneButtonBehavior>();
            if (sceneButtonBehavior != null)
            {
                sceneButtonBehavior.SetSceneName(sceneName);
            }
        }

        if (SceneManager.sceneCount > 0)
        {
            // Turn off the warning that there are no scenes, because there are scenes
            NoScenesWarningLabel.SetActive(false);
        }
    }
}
