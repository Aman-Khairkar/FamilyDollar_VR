using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeUser : MonoBehaviour
{
    public GameObject fadeCanvas;
    Animator fadeAnim;
    public void Fade()
    {
        //fadeCanvas.SetActive(true);
        fadeAnim = fadeCanvas.GetComponent<Animator>();
        fadeAnim.Play("fade");
    }

}
