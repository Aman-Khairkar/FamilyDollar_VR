using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthographicSwitch : MonoBehaviour {

    public static bool InShelfViewMode;

    public GameObject FlatCharacterController;

    public Camera FlatCharacterControllerCamera;
    public Camera ShelfViewCamera;

    public ShelfViewCameraController ShelfViewCameraController;

    public RuntimeSettings RuntimeSettings;

    // Use this for initialization
    void Start() {
        if (FlatCharacterController == null)
        {
            FlatCharacterController = GameObject.FindGameObjectWithTag("CharacterController");
        }
        if (FlatCharacterControllerCamera == null)
        {
            FlatCharacterControllerCamera = Camera.main;
        }
        if (ShelfViewCameraController == null)
        {
            ShelfViewCameraController = FindObjectOfType<ShelfViewCameraController>();
        }
        if (ShelfViewCamera == null && ShelfViewCameraController != null)
        {
            ShelfViewCamera = ShelfViewCameraController.gameObject.GetComponent<Camera>();
        }

        // Make sure the settings menu is showing the correct options
        if (InShelfViewMode)
        {
            RuntimeSettings.ShowShelfViewModeSettings();
        }
        else
        {
            RuntimeSettings.ShowRoamingModeSettings();
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.O))
        {
            // If we're already in shelf view mode
            if (InShelfViewMode)
            {
                // Toggle shelf view mode off
                ExitShelfViewMode();
            }
            // If we're not in shelf view mode and we're looking at a shelf
            else if (ShelfDetector.ShelfInFocus != null)
            {
                // Toggle shelf view mode on
                EnterShelfViewMode();
            }
        }
    }

    private void EnterShelfViewMode()
    {
        // Mark that we're now in shelf view mode
        InShelfViewMode = true;

        // Activate and place the shelf view camera's gameObject
        ShelfViewCameraController.gameObject.SetActive(true);
        ShelfViewCameraController.ViewShelf(ShelfDetector.ShelfInFocus);

        //Switch to use the Shelf View Camera
        ShelfViewCamera.enabled = true;
        FlatCharacterControllerCamera.enabled = false;

        // Disable the Flat Character Controller so the character isn't moving around while the player is looking at the shelf
        FlatCharacterController.SetActive(false);

        // Show the appropriate settings menu
        RuntimeSettings.ShowShelfViewModeSettings();

        // Unlock the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ExitShelfViewMode()
    {
        // Mark that we're no longer in shelf view mode
        InShelfViewMode = false;

        // Re-enable the Flat Character Controller
        FlatCharacterController.SetActive(true);

        //Switch to use the Flat Character Controller Camera
        FlatCharacterControllerCamera.enabled = true;
        ShelfViewCamera.enabled = false;

        // Deactivate the shelf view camera's gameObject
        ShelfViewCameraController.gameObject.SetActive(false);

        // Show the appropriate settings menu
        RuntimeSettings.ShowRoamingModeSettings();

        // Re-lock the cursor if we need to
        FlatMouseLook[] mouseLookScripts = FlatCharacterController.GetComponentsInChildren<FlatMouseLook>();
        foreach(FlatMouseLook mouseLookScript in mouseLookScripts)
        {
            mouseLookScript.UpdateCursorState();
        }
    }
}
