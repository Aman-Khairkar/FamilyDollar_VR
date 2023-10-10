using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeSettingsUIBehavior : MonoBehaviour {

    private static RuntimeSettingsUIBehavior instance;

    public GameObject canvasObject;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);

        // Only allow one RuntimeSettings object at a time
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Show the canvas gameObject
        canvasObject.SetActive(true);
	}

    private void Awake()
    {
        // Make sure the canvas is well out of the way of teleporting character (just in case the prefab position doesn't stick)
        Vector3 outOfTheWayPosition = transform.position;
        outOfTheWayPosition.y = 1000;
        transform.position = outOfTheWayPosition;
    }

    private void Update()
    {
        // Allow user to toggle the UI
        if (Input.GetKeyUp(KeyCode.G))
        {
            canvasObject.SetActive(!canvasObject.activeSelf);
        }
    }
}
