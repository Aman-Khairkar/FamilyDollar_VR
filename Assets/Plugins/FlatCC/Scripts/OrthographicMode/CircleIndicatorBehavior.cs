using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleIndicatorBehavior : MonoBehaviour {

    bool isToggledOn = false;
    public bool IsToggledOn
    {
        get
        {
            return isToggledOn;
        }
    }

    public Image IndicatorImage;

    public Color OnColor;
    public Color OffColor;

	// Use this for initialization
	void Start () {
        if (IndicatorImage == null)
        {
            IndicatorImage = GetComponent<Image>();
        }

        IndicatorImage.color = isToggledOn ? OnColor : OffColor;
	}
	
    public void ChangeColor(bool onColor)
    {
        isToggledOn = onColor;
        IndicatorImage.color = onColor ? OnColor : OffColor;
    }
}
