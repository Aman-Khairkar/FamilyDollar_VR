using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProductDoppelGanger : MonoBehaviour
{
    [SerializeField]
    private bool rotateByThirtyDegrees = false;
    public Transform parentController;
    public Transform tAnchor;

    private Vector2 currentRotation;
    private Vector2 lastMousePosition;

    public void Init(Transform playerRoot)
    {
        //Find the anchor in the player's hierarchy. attach AnchorFlag.cs to whatever obj you want to parent the clones to
        tAnchor = playerRoot.GetComponentInChildren<AnchorFlag>(false).transform;

        parentController = tAnchor.transform.parent;
        //Debug.LogError("anchor root = " + parentController);
        transform.SetParent(tAnchor);

        //sanitize the transform of the clone before we scoot it into the center
        transform.localPosition = Vector3.zero; //offset?
        transform.localRotation = Quaternion.Euler(0, 180f, 0);

        //using the box collider bounds, calculate the distance between our current pivot position and the center of the bounds
        Bounds b = GetComponent<BoxCollider>().bounds;
       // Vector3 diff = b.center - transform.position;
        //scoot the object such that the center of the bounds now matches the anchor transform
      //  transform.position -= diff;
      //  transform.localPosition += new Vector3(0, 0, b.extents.z / 2f);
      //  transform.localPosition += new Vector3(0, (b.extents.y / 2f) + 0.1f, 0);
        //rotate slightly towards the player
        //if (!rotateByThirtyDegrees)
        //{
        //    transform.localRotation = Quaternion.Euler(new Vector3(30f, transform.localRotation.y, 20f));
        //}
        tAnchor.localRotation = Quaternion.Euler(new Vector3(-90, tAnchor.localRotation.y, tAnchor.localRotation.z));

    }

    //flips the clone 180 degrees. somehwat error prone due to non-consistent pivots
    public void SpinIt()
    {
        //Debug.LogError("SPINNING");
        //Debug.LogError("parenting tanchor to transform " + transform.name);
        tAnchor.SetParent(transform);
        tAnchor.localPosition = Vector3.zero;
        tAnchor.localRotation = Quaternion.identity;

        Bounds b = GetComponent<BoxCollider>().bounds;
        tAnchor.position = b.center;
        // Debug.LogError("parenting tanchor to parentcontroller " + parentController.name);
        tAnchor.SetParent(parentController);
        // Debug.LogError("parenting transform to tanchor" + tAnchor.name);
        transform.SetParent(tAnchor);
        //  Debug.LogError("rotating");
        tAnchor.Rotate(Vector3.up, 180f, Space.Self);
        //  Debug.LogError("parenting transform to parentcontroller " + parentController.name);
        transform.SetParent(parentController);
        //  Debug.Log("tanchor " + tAnchor.name + "  active status: " + tAnchor.gameObject.activeSelf);
        //  Debug.Log("parentcontroller " + parentController.name + " active status: " + parentController.gameObject.activeSelf);
    }

    public void DestroyMe()
    {
        Destroy(this.gameObject);
    }


    //was gonna do a free rotate once, turned out to be harder than i thought.
    //public void FreeRotate()
    //{

    //    currentRotation.x += Input.GetAxis("Mouse X");
    //    Debug.Log("currentrotation.x = " + currentRotation.x);
    //    currentRotation.y -= Input.GetAxis("Mouse Y");
    //    Debug.Log("currentrotation.y = " + currentRotation.y);
    //    currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
    //    currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
    //    //dup.transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);

    //    Bounds b = GetComponent<BoxCollider>().bounds;
    //    transform.RotateAround(b.center, new Vector3(currentRotation.y, currentRotation.x, 0), 10f);
    //    Debug.Log("b.center = " + b.center + " " + " transform position = " + transform.position);

    //}

}

