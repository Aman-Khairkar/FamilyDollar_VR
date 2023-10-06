using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour {

    // Use this for initialization
    private Renderer rend;
    public bool isFadingIn;

	void Start () {
        //rend = GetComponent<Renderer>();
	}
	
	// 191 is 75% of 255
	public void Fade (bool fadeIn) {
        isFadingIn = fadeIn;
        StopAllCoroutines();
        Renderer rend = GetComponent<Renderer>();
        if(rend.materials[0].color.a < 188)//if our alpha is not at 75%, meaning the heatmap hasn't shown up yet
            StartCoroutine("FadeIn", (rend.materials));
	}

    public float fadeSpeed = .025f;
    float lerpAmount;

    IEnumerator FadeIn(Material[] materialsToFadeIn)
    {
        lerpAmount = 0;

        while (lerpAmount < 1)
        {
            for (int i = 0; i < materialsToFadeIn.Length; i++)
            {
                Color c = materialsToFadeIn[i].color;
                if (isFadingIn)
                    c.a = Mathf.Lerp(0, 0.75f, lerpAmount);
                else
                    c.a = Mathf.Lerp(0.75f, 0, lerpAmount);
                materialsToFadeIn[i].color = c;
                lerpAmount += fadeSpeed;
            }
            yield return null;
        }

        //gameObject.SetActive(false);
    }
}
