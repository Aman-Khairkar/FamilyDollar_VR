using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointerOpeningScene : MonoBehaviour
{
    public LineRenderer laser;
    public Transform particlePoint;

    public float maxDistance = 100f;
    public GameObject selectedObject = null;
    public GameObject grabbedObject = null;
    public LayerMask Mask;

    public SplashScreenFade faderCanvas;



    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.AddComponent<AnchorFlag>();
        this.transform.localPosition = new Vector3(0.1f, 0, 0.31f);
    }
    // Update is called once per frame
    void Update()
    {
        float rayDistance = maxDistance;
        RaycastHit hit;


        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayDistance, Mask))
        {
            rayDistance = hit.distance;
            /*if (hit.collider.gameObject != selectedObject && selectedObject != null)
                selectedObject.SendMessage("UnSelect", SendMessageOptions.DontRequireReceiver);
            */
            selectedObject = hit.collider.gameObject;
//            selectedObject.SendMessage("Select", SendMessageOptions.DontRequireReceiver);

            if (Input.GetKeyDown(KeyCode.JoystickButton15) || Input.GetKeyDown(KeyCode.Space))
            {
                selectedObject.SendMessage("DoAction");
            }
        }
        else
        {
            if (selectedObject != null)
            {
            //    selectedObject.SendMessage("UnSelect", SendMessageOptions.DontRequireReceiver);
                selectedObject = null;
            }
        }


        laser.SetPosition(1, Vector3.forward * rayDistance);
        if (particlePoint != null)
            particlePoint.localPosition = Vector3.forward * rayDistance;

    }

}
