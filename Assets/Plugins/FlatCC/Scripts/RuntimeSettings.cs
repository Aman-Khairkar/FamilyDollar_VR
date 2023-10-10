using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuntimeSettings : MonoBehaviour {
    public FlatPlayerController FlatPlayerController;

    public GameObject RoamingModeSettings;
    public Slider ViewerHeightSlider;
    public Slider WalkSpeedSlider;
    public Slider TurnSpeedSlider;

    public GameObject ShelfViewModeSettings;

    // Use this for initialization
    void Start ()
    {
        FlatPlayerController = FindObjectOfType<FlatPlayerController>();

        if (FlatPlayerController != null)
        {
            // Set initial slider value for Viewer Height
            if (ViewerHeightSlider != null)
            {
                ViewerHeightSlider.value = PlayerPrefs.GetFloat(FlatPlayerController.VIEWER_HEIGHT_PLAYERPREF_KEY);
            }

            // Set initial slider value for walk speed
            if (WalkSpeedSlider != null)
            {
                WalkSpeedSlider.value = PlayerPrefs.GetFloat(FlatPlayerController.WALK_SPEED_PLAYERPREF_KEY);
            }
        }

        // Set initial slider value for Turn Speed
        if (TurnSpeedSlider != null)
        {
            TurnSpeedSlider.value = PlayerPrefs.GetFloat(FlatMouseLook.TURN_SPEED_PLAYERPREF_KEY);
        }
    }

    public void SetViewerHeight(float newValue)
    {
        if (FlatPlayerController == null)
        {
            return;
        }

        //apply the height number
        FlatPlayerController.SetViewerHeight(newValue);
    }

    public void SetWalkSpeed(float newValue)
    {
        if (FlatPlayerController == null)
        {
            return;
        }

        // Apply the new walk speed value
        FlatPlayerController.SetWalkSpeed(newValue);
    }

    public void SetTurnSpeed(float newValue)
    {
        FlatMouseLook.SetTurnSpeed(newValue);
    }

    public void ShowRoamingModeSettings()
    {
        // Hide the settings for Shelf View Mode
        if (ShelfViewModeSettings != null)
        {
            ShelfViewModeSettings.SetActive(false);
        }

        // Show the settings for Roaming Mode
        if (RoamingModeSettings != null)
        {
            RoamingModeSettings.SetActive(true);
        }
    }

    public void ShowShelfViewModeSettings()
    {
        // Hide the settings for Roaming Mode
        if (RoamingModeSettings != null)
        {
            RoamingModeSettings.SetActive(false);
        }

        // Show the settings for Shelf View Mode
        if (ShelfViewModeSettings != null)
        {
            ShelfViewModeSettings.SetActive(true);
        }
    }
}
