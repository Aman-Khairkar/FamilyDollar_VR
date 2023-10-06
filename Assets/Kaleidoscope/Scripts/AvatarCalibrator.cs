using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarCalibrator : MonoBehaviour
{

    public GameObject rightHandAnchor;
    public GameObject rightHandAvatar;

    // Use this for initialization
    void Start()
    {
        if (rightHandAnchor == null)
            rightHandAnchor = GameObject.Find("RightHandAnchor");

        if (rightHandAvatar == null)
            rightHandAvatar = transform.Find("hand_right").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += rightHandAnchor.transform.position - rightHandAvatar.transform.position;
    }
}