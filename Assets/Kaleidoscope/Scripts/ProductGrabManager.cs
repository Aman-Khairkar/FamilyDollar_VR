using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductGrabManager : MonoBehaviour {


    public Rigidbody rb;
    public BoxCollider bc;
    public VRTK.VRTK_InteractableObject io;
    bool isGrabbed = false;

	// Use this for initialization
	void Awake () {
        bc = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        io = GetComponent<VRTK.VRTK_InteractableObject>();
        //bc.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
        isGrabbed = io.IsGrabbed();
        if(isGrabbed && rb.isKinematic)
        {
            Debug.Log("no longer kinematic");
            rb.isKinematic = false;
        }
        //else
        //{
        //    if (rb.isKinematic)
        //        rb.isKinematic = true;
        //}

	}
}
