using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuToggle : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.SetActive(false);
	}
	
    public void ToggleMenu()
    {
        bool isActive = gameObject.activeSelf;
        gameObject.SetActive(!isActive);
    }
}
