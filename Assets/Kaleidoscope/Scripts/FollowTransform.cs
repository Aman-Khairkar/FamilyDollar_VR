using UnityEngine;
using System.Collections;

public class FollowTransform : MonoBehaviour {

	public enum Mode{
		WorldToWorld,
		WorldToScreen,
		ScreenToWorld
	}

	public Transform follow;

	public Mode mode;
	public Camera cameraOverride;
    public float snapToUnits;

    public bool followRotation;
	public bool followScale;

	void Awake(){
		if(cameraOverride == null)
			cameraOverride = Camera.main;
	}

	void LateUpdate(){
		if(follow == null)
			return;

		switch (mode) {
			case Mode.WorldToWorld:
				transform.position = follow.position;
				break;
			case Mode.WorldToScreen:
				transform.position = cameraOverride.WorldToScreenPoint(follow.position);
				break;
			default:
				break;
		}

        //if(snapToUnits != 0)
        //    transform.position = Utilities.SnapToUnits(transform.position, snapToUnits);

        if(followRotation)
            transform.rotation = Quaternion.Euler(new Vector3(0,0,-follow.rotation.eulerAngles.y));
			
		if (followScale)
			transform.localScale = follow.lossyScale;
	}
}
