using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserControllerNetworkedUser : MonoBehaviour
{
    private LineRenderer laser;
    public Transform particlePoint;

    public float maxDistance = 100f;
    public GameObject selectedObject = null;
    public LayerMask Mask;
    public LayerMask Teleportation;

    public UserManager player;

    // Start is called before the first frame update
    void Start()
    {
         Mask = LayerMask.NameToLayer("Product");
        Teleportation = LayerMask.NameToLayer("Teleport");
        laser = GetComponent<LineRenderer>();
        player = GetComponentInParent<UserManager>();
#if UNITY_EDITOR
        this.transform.localPosition = new Vector3(0.1f, 0, 0.31f);
#endif
    }

        float rayDistance;
    // Update is called once per frame
    void Update()
    {
        if (player.isInteractableUser)
        {
            rayDistance = 0.001f;
        }
        else
        {
            float rayDistance = maxDistance;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxDistance, Mask))
            {
                rayDistance = hit.distance;
                if (hit.collider.gameObject != selectedObject && selectedObject != null)
                    selectedObject.SendMessage("UnSelect", SendMessageOptions.DontRequireReceiver);

                selectedObject = hit.collider.gameObject;
                selectedObject.SendMessage("Select", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                if (selectedObject != null)
                {
                    selectedObject.SendMessage("UnSelect", SendMessageOptions.DontRequireReceiver);
                    selectedObject = null;
                }
            }
            //teleportation will be controlled here - FigJ
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxDistance, Teleportation))
            {
                rayDistance = hit.distance;
                if (hit.collider.gameObject != selectedObject && selectedObject != null)
                    selectedObject.SendMessage("UnSelect", SendMessageOptions.DontRequireReceiver);

                selectedObject = hit.collider.gameObject;
                selectedObject.SendMessage("Select", SendMessageOptions.DontRequireReceiver);
            }

            laser.SetPosition(1, Vector3.forward * rayDistance);

            if (particlePoint != null)
                particlePoint.localPosition = Vector3.forward * rayDistance;
        }
    }

    public void DisableAndEnableLine(bool b)
    {
        laser.enabled = b;
    }
}
