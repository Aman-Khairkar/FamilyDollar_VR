using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyModeToggle : MonoBehaviour
{
    FlatPlayerController CharacterController;

    public Button FlyModeToggleButton;
    public Image FlyModeToggleImage;

    public Sprite FlyModeImage;
    public Sprite WalkModeImage;

    void OnEnable()
    {

        if (CharacterController == null)
        {
            CharacterController = FindObjectOfType<FlatPlayerController>();
        }

        if (FlyModeToggleButton == null)
        {
            FlyModeToggleButton = GetComponent<Button>();
        }

        if (FlyModeToggleImage == null)
        {
            FlyModeToggleImage = GetComponent<Image>();
        }

        // Subscribe to the OnFlyModeSet event
        if (CharacterController != null)
        {
            CharacterController.OnFlyModeSet += UpdateButtonImage;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the OnFlyModeSet event
        if (CharacterController != null)
        {
            CharacterController.OnFlyModeSet -= UpdateButtonImage;
        }
    }

    public void OnClick()
    {
        CharacterController.ToggleFlyMode();
    }

    public void UpdateButtonImage(bool isFlyMode)
    {
        FlyModeToggleImage.sprite = isFlyMode ? FlyModeImage : WalkModeImage;
    }
}
