using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class TPLocation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    //TODO: combine rig offset + playspace to put the polayer on tp loc, not just playspace
    Material mat;
    private Color originalColor;
    //public Color highlightColor = Color.black;
    //public Transform player;


   /* private void OnEnable()
    {
        mat = GetComponent<MeshRenderer>().materials[0];
        gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_EMISSION", Color.black);

        
    } */

    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", Color.black);

        //player = GameObject.Find("LocalPlayer (Clone)").transform;
    }

    void Update()
    {
        /*if (isHighlighted)
        { 
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.Active) || Input.GetKeyDown(KeyCode.Space))
            {
                Teleport();
            }

        } */
    }

   /* public void Teleport()
    {
        //Debug.Log("teleporting to" + transform.position.x + " " + transform.position.z);
        player.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
    }*/


    //public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    //{
    //    var currentPos = transform.localPosition;
    //    var t = 0f;
    //    Debug.Log("moving");
    //    while (t < 1)
    //    {
    //        t += Time.deltaTime / timeToMove;
    //        transform.localPosition = Vector3.Lerp(currentPos, position, t);
    //        yield return null;
    //    }
    //}

    public void OnPointerEnter(PointerEventData data)
    {
        Highlight();
        //player = data.enterEventCamera.transform.root;
        //if (!player)
            //Debug.LogError("NO PLAYER FOUND");
    }

    public void OnPointerExit(PointerEventData data)
    {
        UnHighlight();
    }

    public void Highlight()
    {
        mat.SetColor("_EmissionColor", Color.cyan);
        //gameObject.GetComponent<MeshRenderer>().material.SetColor("_EMISSION", Color.cyan);
        // StartCoroutine(MoveToPosition(transform, new Vector3(transform.position.x, 4.74f, transform.position.z), 1f));
        Debug.Log("Teleport on.");
    }

    public void UnHighlight()
    {
        mat.SetColor("_EmissionColor", Color.black);
        //gameObject.GetComponent<MeshRenderer>().material.SetColor("_EMISSION", Color.black);
        //StartCoroutine(MoveToPosition(transform, new Vector3(transform.position.x, 4.174f, transform.position.z), 1f));
        Debug.Log("Teleport off.");
    }
}
