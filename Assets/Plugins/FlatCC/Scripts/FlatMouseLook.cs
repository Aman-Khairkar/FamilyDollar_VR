using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatMouseLook : MonoBehaviour
{
    public const string TURN_SPEED_PLAYERPREF_KEY = "TurnSpeed";
    private bool rotationIsFrozen = false;
    private static bool cursorIsLocked = true;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public static float sensitivity = 5f;

    float minimumX = 0f;
    float maximumX = 360f;

    float minimumY = -60f;
    float maximumY = 45f;

    float rotationY = 0f;

    void Start()
    {
        Rigidbody ourRigidbody = GetComponent<Rigidbody>();

        if (ourRigidbody != null)
        {
            ourRigidbody.freezeRotation = true;
        }
    }

    void Awake()
    {
        /*
         * Make sure that we don't try to update the cursor state if this isn't part of the active flat character controller
         * (meaning it doesn't know the previous settings and is about to be deleted anyway)
         */

        // Find the FlatPlayerController on or above this object
        FlatPlayerController flatPlayerController = GetComponent<FlatPlayerController>();
        if (flatPlayerController == null)
        {
            flatPlayerController = GetComponentInParent<FlatPlayerController>();
        }
        // If we don't have a FlatPlayerController yet, or this is the active one
        if (FlatPlayerController.FlatCharacterControllerInstance == null
            || FlatPlayerController.FlatCharacterControllerInstance == flatPlayerController.gameObject)
        {
            // Then it's fine to update the cursor state
            UpdateCursorState();
        }
    }

    private void FixedUpdate()
    {
        bool changeCursorState = false;

        //handle cursor lock
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            cursorIsLocked = false;
            changeCursorState = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            cursorIsLocked = true;
            changeCursorState = true;
        }

        // Update the state of the cursor
        if (changeCursorState)
        {
            UpdateCursorState();
        }
    }

    public void UpdateCursorState()
    {
        if (cursorIsLocked && !rotationIsFrozen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Update()
    {
        // Freeze/unfreeze rotation using the Space bar
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rotationIsFrozen = !rotationIsFrozen;
            UpdateCursorState();
        }
       
        // If the rotation isn't frozen by the Space bar and the character isn't animating right now
        if (!rotationIsFrozen && !FlatPlayerController.IsAnimating)
        {
            if (axes == RotationAxes.MouseXAndY)
            {
                float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;

                rotationY += Input.GetAxis("Mouse Y") * sensitivity;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

            }
            else if (axes == RotationAxes.MouseX)
            {
                transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);
            }
            else
            {
                rotationY += Input.GetAxis("Mouse Y") * sensitivity;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
            }
        }
    }

    public static void SetTurnSpeed(float newValue)
    {
        // Apply the new turn speed value
        sensitivity = newValue;

        // Update this value in the player prefs
        PlayerPrefs.SetFloat(TURN_SPEED_PLAYERPREF_KEY, newValue);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Call this after the flat character controller's rotation has been set by something other than this script.
    /// This will update the rotation variable so that when the player moves their camera, it will move based off of
    /// the new rotation instead of based off of the old rotation.
    /// </summary>
    public void UpdateRotationTracker()
    {
        if (axes == RotationAxes.MouseY)
        {
            rotationY = -transform.localEulerAngles.x;
        }
    }
}
