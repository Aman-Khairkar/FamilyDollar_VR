using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneButtonBehavior : MonoBehaviour
{
    public Text TextLabel;
    private string sceneName;
    public string SceneName
    {
        get
        {
            return sceneName;
        }
    }

    public Image ButtonImage;
    public static Color NormalColor =      new Color(0.196f, 0.196f, 0.196f);
    public static Color HighlightedColor = new Color(0.42f, 0.42f, 0.42f);

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (ButtonImage == null)
        {
            ButtonImage = GetComponent<Image>();
        }

        // Make sure the first scene's button starts out being the active color
        UpdateButtonColor(SceneManager.GetActiveScene());
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Set the name of the scene this button will go to, and update the text on the UI.
    /// </summary>
    /// <param name="newSceneName">The name of the scene to which this button should switch.</param>
    public void SetSceneName(string newSceneName)
    {
        sceneName = newSceneName;
        TextLabel.text = sceneName;
    }

    public void OnClick()
    {
        SceneManager.LoadScene(SceneName);
    }

    /// <summary>
    /// Update the scene name shown in the lower right in case the scene has been changed by something other than SwitchSceneManager.
    /// </summary>
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateButtonColor(scene);
    }

    /// <summary>
    /// Update the color of this button based on whether its SceneName matches the name of the current scene.
    /// </summary>
    /// <param name="scene">The scene to check for a name match with (usually the active scene)./param>
    public void UpdateButtonColor(Scene scene)
    {
        // If this button is for the specified scene
        if (SceneName == scene.name)
        {
            // Highlight it
            ButtonImage.color = HighlightedColor;
        }
        else
        {
            // Return the color to normal
            ButtonImage.color = NormalColor;
        }
    }
}
