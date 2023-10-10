using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicScrollViewFitter : MonoBehaviour
{
    public RectTransform PanelBackgroundRect;
    public RectTransform ContentRect;
    public RectTransform ScrollViewRect;
    public float YPadding;

    private float baseOffsetMinY;
    private float lastContentRectHeight;
    private float lastScrollViewRectHeight;

    // Start is called before the first frame update
    void Start()
    {
        if (PanelBackgroundRect == null)
        {
            PanelBackgroundRect = GetComponent<RectTransform>();
        }

        // Remember the original height of our rectangle so that the matched height of the content rect is additive
        baseOffsetMinY = PanelBackgroundRect.offsetMin.y;
        UpdateHeight();
    }

    // Update is called once per frame
    void Update()
    {
        // If the size of the content rect has changed since we last updated our height
        if (ContentRect.rect.height != lastContentRectHeight
            || ScrollViewRect.rect.height != lastScrollViewRectHeight)
        {
            UpdateHeight();
        }
    }

    public void UpdateHeight()
    {
        // Match the height of the content rect, but only until it would get bigger than the scroll view rect
        float heightToMatch = Mathf.Min(ContentRect.rect.height, ScrollViewRect.rect.height);

        // Move the lower-left corner of our rectangle to encompass the scroll view area
        PanelBackgroundRect.offsetMin = new Vector2(
                                                PanelBackgroundRect.offsetMin.x,
                                                baseOffsetMinY - heightToMatch - (heightToMatch > 0 ? YPadding : 0));
        
        // Update our memory of the rect heights
        lastContentRectHeight = ContentRect.rect.height;
        lastScrollViewRectHeight = ScrollViewRect.rect.height;
    }
}
