using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetComponents : MonoBehaviour {
    public Renderer meshRenderer;

	// Use this for initialization
	void Start () {
		if (meshRenderer == null)
        {
            Transform rotateParent = transform.Find("RotateParent");
            if (rotateParent != null)
            {
                meshRenderer = rotateParent.GetComponentInChildren<Renderer>();
            }
        }
	}
}
