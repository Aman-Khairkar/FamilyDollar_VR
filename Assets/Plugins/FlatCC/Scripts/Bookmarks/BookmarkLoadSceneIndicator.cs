using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookmarkLoadSceneIndicator : MonoBehaviour
{
    static Color LoadSceneImageActiveColor = Color.white;
    static Color LoadSceneImageInactiveColor = Color.gray;

    public Button ToggleButton;
    public Image IndicatorImage;

    // Start is called before the first frame update
    void Start()
    {
        if (IndicatorImage == null)
        {
            IndicatorImage = GetComponent<Image>();
        }

        if (ToggleButton == null)
        {
            ToggleButton = GetComponent<Button>();
        }
    }
    
    public void IndicateActive(bool active)
    {
        IndicatorImage.color = active ? LoadSceneImageActiveColor : LoadSceneImageInactiveColor;
    }

    public void UpdateForEditMode(bool inEditMode)
    {
        ToggleButton.enabled = inEditMode;
    }
}
