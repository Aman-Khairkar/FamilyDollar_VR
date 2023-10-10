using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookmarkIndicator : MonoBehaviour
{
    public Image Indicator;

    public Sprite InactiveSprite;
    public Sprite ActiveSprite;
    public bool IsTransitionComplete;

    // Start is called before the first frame update
    void Start()
    {
        if (Indicator == null)
        {
            Indicator = GetComponent<Image>();
        }
    }

    public IEnumerator IndicateActive(bool active)
    {
        if (active)
        {
            Indicator.sprite = ActiveSprite;
            yield return new WaitUntil(() => IsTransitionComplete);
            //Changing color on active sprite may not reflect.
            //That's the reason changing sprire back to inactive.
            Indicator.sprite = InactiveSprite;
            Indicator.color = Color.green;
        }
        else
        {
            IsTransitionComplete = false;
            Indicator.sprite = InactiveSprite;
            Indicator.color = Color.white;
            yield return null;
        }
    }
}
