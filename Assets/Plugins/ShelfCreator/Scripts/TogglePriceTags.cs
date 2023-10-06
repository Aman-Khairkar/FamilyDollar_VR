using PriceTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePriceTags : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Dollar)
            || ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.Alpha4)))
        {
            PriceTagManager.Instance.TogglePriceTags();
        }
	}
}
