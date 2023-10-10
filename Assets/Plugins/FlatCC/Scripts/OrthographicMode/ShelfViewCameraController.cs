using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShelfViewCameraController : MonoBehaviour {

    public const string ORTHOGRAPHIC_SIZE_PLAYERPREF_KEY = "OrthographicDPI";

    private GameObject currentShelf;
    public Camera ShelfViewCamera;

    private Vector2 previousMousePosition;

    private Vector2 mousePanSensitivity = new Vector2(0.0085f, 0.0085f);

    // Set some limits for zooming
    const float minDpi = 1; // Do not set this to zero.
    const float maxDpi = 300;
    //float defaultOrthographicSize;

    public InputField dpiInputField;

	// Use this for initialization
	void Start ()
    {
		if (ShelfViewCamera == null)
        {
            ShelfViewCamera = GetComponent<Camera>();
        }

        //if our DPI value is available in the PlayerPrefs
        if (PlayerPrefs.HasKey(ORTHOGRAPHIC_SIZE_PLAYERPREF_KEY))
        {
            ShelfViewCamera.orthographicSize = PlayerPrefs.GetFloat(ORTHOGRAPHIC_SIZE_PLAYERPREF_KEY);
        }

        //defaultOrthographicSize = ShelfViewCamera.orthographicSize;

        // Activate / deactivate this camera and gameObject according to whether we're in shelf view mode
        ShelfViewCamera.enabled = OrthographicSwitch.InShelfViewMode;
        gameObject.SetActive(OrthographicSwitch.InShelfViewMode);
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 currentMousePosition = Input.mousePosition;
        Vector2 mouseDelta = currentMousePosition - previousMousePosition;

        // Pan by dragging cursor
        if (Input.GetMouseButton(0))
        {
;           Vector3 translation = new Vector3(-mouseDelta.x * mousePanSensitivity.x, -mouseDelta.y * mousePanSensitivity.y, 0f);
            ShelfViewCamera.transform.Translate(translation);
        }
        // If not dragging cursor, we can pan with the arrow keys
        else if (Input.anyKey)
        {
            Vector3 translation = new Vector3(0f, 0f, 0f);

            // Pan horizontally with arrow keys
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                translation.x -= mousePanSensitivity.x;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                translation.x += mousePanSensitivity.x;
            }

            // Pan vertically with arrow keys
            if (Input.GetKey(KeyCode.DownArrow))
            {
                translation.y -= mousePanSensitivity.y;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                translation.y += mousePanSensitivity.y;
            }

            ShelfViewCamera.transform.Translate(translation);
        }

        // Zoom with scroll wheel
        float mouseScroll = Input.mouseScrollDelta.y;
        if (mouseScroll != 0)
        {
            // Zoom slower as the camera gets closer to the shelf
            float scrollSensitivity = ScrollSensitivity(ShelfViewCamera.orthographicSize);
            float newOrthographicSize = ShelfViewCamera.orthographicSize - (mouseScroll * scrollSensitivity);

            // Find min and max orthographic size based on the current screen height
            float minOrthographicSize = TranslateDpiToOrthographicSize(maxDpi);
            float maxOrthographicSize = maxDpi == 0 ? 10f : TranslateDpiToOrthographicSize(minDpi);

            // Make sure that we don't zoom too far into or away from the shelf
            newOrthographicSize = Mathf.Clamp(newOrthographicSize, minOrthographicSize, maxOrthographicSize);

            // Apply the new zoom level
            ShelfViewCamera.orthographicSize = newOrthographicSize;
            // Update the DPI input field to the new value
            UpdateDpiInputField();
        }

        previousMousePosition = currentMousePosition;
	}

    public void ViewShelf(GameObject shelf)
    {
        // Update the shelf we're currently viewing
        currentShelf = shelf;

        // If we're not viewing any shelf, don't bother with the rest
        if (currentShelf == null)
        {
            return;
        }

        // Reset to default camera size
        //ShelfViewCamera.orthographicSize = defaultOrthographicSize;
        // Update the DPI input field to the new value
        UpdateDpiInputField();

        // Move the camera to look at this shelf
        MoveCameraToCurrentShelf();
    }

    private void MoveCameraToCurrentShelf()
    {
        // Apply the proper rotation to match the shelf
        transform.rotation = currentShelf.transform.rotation;

        // Find and apply the correct camera position
        // Start centered on the bottom-left corner of the shelf
        Vector3 newCameraPosition = currentShelf.transform.position;
        // Move back so we can see everything
        newCameraPosition += -1 * currentShelf.transform.forward;
        // Line up the shelf with the bottom of the screen
        newCameraPosition += ShelfViewCamera.orthographicSize * transform.up.normalized;
        // Line up the shelf with the left of the screen
        newCameraPosition += ShelfViewCamera.orthographicSize * transform.right.normalized * (ShelfViewCamera.pixelRect.width / ShelfViewCamera.pixelRect.height);
        // Apply the position
        transform.position = newCameraPosition;
    }

    /// <summary>
    /// Scale the amount we zoom based on the camera's current orthographic size so that the camera zooms slower when it's already zoomed in very far.
    /// </summary>
    /// <param name="currentZoom">The current orthographic size of the camera.</param>
    /// <returns>The factor by which the mouse scroll should be scaled based on the current zoom of the camera.</returns>
    private static float ScrollSensitivity(float currentZoom)
    {
        float scrollSensitivity;

        if (currentZoom > 8)
        {
            scrollSensitivity = 0.5f;
        }
        else if (currentZoom > 5)
        {
            scrollSensitivity = 0.25f;
        }
        else if (currentZoom > 2)
        {
            scrollSensitivity = 0.1f;
        }
        else if (currentZoom > 0.85)
        {
            scrollSensitivity = 0.05f;
        }
        else if (currentZoom > 0.5)
        {
            scrollSensitivity = 0.025f;
        }
        else
        {
            scrollSensitivity = 0.01f;
        }

        return scrollSensitivity;
    }

    private void UpdateDpiInputField(bool forceUpdate = false)
    {
        if (dpiInputField != null)
        {
            float newDpiValue = TranslateOrthographicSizeToDpi(ShelfViewCamera.orthographicSize);
            string newValueString = string.Format("{0:0.##}", newDpiValue);
            if (dpiInputField.text != newValueString || forceUpdate)
            {
                dpiInputField.text = newValueString;
            }
        }

        // Update this value in the player prefs
        PlayerPrefs.SetFloat(ORTHOGRAPHIC_SIZE_PLAYERPREF_KEY, ShelfViewCamera.orthographicSize);
        PlayerPrefs.Save();
    }

    public void UpdateFromDpiInputField(string dpiInputFieldText)
    {
        float dpi = float.Parse(dpiInputFieldText);
        dpi = Mathf.Clamp(dpi, minDpi, maxDpi);

        float newOrthographicSizeValue = TranslateDpiToOrthographicSize(dpi);
        ShelfViewCamera.orthographicSize = newOrthographicSizeValue;

        UpdateDpiInputField();
    }

    private static float TranslateOrthographicSizeToDpi(float orthographicSize)
    {
        float dpi;
        if (orthographicSize == 0)
        {
            dpi = maxDpi;
        }
        else
        {
            dpi = 0.0254f * (Screen.height / orthographicSize) * 0.5f;
        }
        return dpi;
    }

    private static float TranslateDpiToOrthographicSize(float dpi)
    {
        float orthoSize;
        if (dpi == 0)
        {
            orthoSize = 10f;
        }
        else
        {
            orthoSize = 0.0254f * (Screen.height / dpi) * 0.5f;
        }
        
        return orthoSize;
    }
}
