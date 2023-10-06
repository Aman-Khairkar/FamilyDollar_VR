using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayOnFloor : MonoBehaviour {

    public Transform head;
    public Transform myTrans;
    public Transform floor;

    public float xOffset = 0;
    public float yOffset = 0;
    public float zOffset = 0;

    // Use this for initialization
    void Start () {
        myTrans = transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        myTrans.position = new Vector3(head.position.x - xOffset, floor.position.y + yOffset, head.position.z- zOffset);
	}
}
