using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfDetector : MonoBehaviour {
    private static GameObject _shelfInFocus = null;
    public static GameObject ShelfInFocus
    {
        get
        {
            return _shelfInFocus;
        }
    }
    public LayerMask shelfLayerMask;
    public CircleIndicatorBehavior CircleIndicator;

	// Use this for initialization
	void Start ()
    {
		if (CircleIndicator == null)
        {
            CircleIndicator = FindObjectOfType<CircleIndicatorBehavior>();
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (FlatControllerShelfDependencies.Instance.ShelfCreatorInProject)
        {
            bool shelfSeen = false;
            RaycastHit hit;
            // Get a ray from the camera pointing in the direction where the user is looking
            if (Camera.main != null)
            {
                Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                // If the user is looking at something
                if (Physics.Raycast(cameraRay, out hit, float.MaxValue, shelfLayerMask))
                {
                    // If this is a shelf
                    GameObject shelfParent = FlatControllerShelfDependencies.Instance.GetShelfParentIfItExists(hit.transform);
                    if (shelfParent != null)
                    {
                        // Set the shelf in focus
                        SetShelfInFocus(shelfParent);
                        shelfSeen = true;
                    }
                }
            }

            // If we didn't see a shelf this frame but we were looking at one before
            if (!shelfSeen && ShelfInFocus != null)
            {
                // Clear the shelf in focus
                SetShelfInFocus(null);
            }
        }
    }

    public void SetShelfInFocus(GameObject newShelfInFocus)
    {
        // Update the shelf in focus
        _shelfInFocus = newShelfInFocus;

        // Update the Circle Indicator if there is one
        if (CircleIndicator != null)
        {
            bool activeColor = ShelfInFocus != null;
            CircleIndicator.ChangeColor(activeColor);
        }
    }
}
