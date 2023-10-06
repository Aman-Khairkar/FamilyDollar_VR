using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropagateProductTransform : MonoBehaviour
{
    [Tooltip("The transform to copy to all products that share a name with this one. Defaults to whatever child holds the mesh components.")]
    public Transform propagationTrans;
    public StrategiesManager manager;

    private void Start()
    {
        if (manager == null)
        {
            manager = FindObjectOfType<StrategiesManager>();
        }
    }

    [ContextMenu("Propagate Transform to All Strategies In StrategiesManager list")]
    public void PropagateTransformToAllStrategies()
    {
        if (manager == null)
            manager = FindObjectOfType<StrategiesManager>();

        if (propagationTrans == null)
            propagationTrans = transform.GetComponentInChildren<MeshFilter>().transform;

        List<Transform> productsWithThisName = new List<Transform>();
        foreach (GameObject strat in manager.strategies)
        {
            foreach (PropagateProductTransform product in strat.GetComponentsInChildren<PropagateProductTransform>())
            {
                if (product.name == this.name)
                {
                    if (propagationTrans.name.Contains("F_"))
                        productsWithThisName.Add(product.transform);
                    else if (propagationTrans.name.Contains("Position"))
                        productsWithThisName.Add(product.transform.GetChild(0));
                    else if (propagationTrans.name.Contains("Mesh"))
                        productsWithThisName.Add(product.transform.GetComponentInChildren<MeshFilter>().transform);
                    else
                        Debug.LogError("No children found matching propagation trans' name!");
                }
            }
        }

        foreach (Transform t in productsWithThisName)
        {
            t.localPosition = propagationTrans.localPosition;
            t.localRotation = propagationTrans.localRotation;
            t.localScale = propagationTrans.localScale;

            MeshRenderer mr = t.GetComponentInChildren<MeshRenderer>();
            if (mr && mr.transform.GetComponent<MeshFilter>().sharedMesh != null)
            {
                mr.sharedMaterials[0].color = Color.white;
                //Material newMat = Resources.
                //mr.materials[0] = Resources.Load()
            }
        }
    }

    //these methods determine which object in the product hierarchy we will match to.

    [ContextMenu("Set PropagateTransform to First child (Product)")]
    public void SetPropagateTransformToFirstChild()
    {
        propagationTrans = transform.GetChild(0);

        PropagateTransformToAllStrategies();
        Debug.Log("Finished propagating product transforms.");
    }

    [ContextMenu("Set PropagateTransform to Mesh")]
    public void SetPropagateTransformToMesh()
    {
        propagationTrans = transform.GetComponentInChildren<MeshFilter>().transform;

        PropagateTransformToAllStrategies();
        Debug.Log("Finsihed propagating mesh transforms.");
    }
    /*
    [ContextMenu("Set PropagateTransform to Colliders")]
    public void SetPropagateTransformToColliders()
    {
        propagationTrans = transform.GetComponent<BoxCollider>();

        if (manager == null)
            manager = FindObjectOfType<StrategiesManager>();

        if (propagationTrans == null)
            propagationTrans = transform.GetComponentInChildren<BoxCollider>();

        List<BoxCollider> productsWithThisName = new List<BoxCollider>();
        foreach (GameObject strat in manager.strategies)
        {
            foreach (PropagateProductTransform product in strat.GetComponentsInChildren<PropagateProductTransform>())
            {
                if (product.name == this.name)
                {
                    if (propagationTrans.transform.name.Contains("F_"))
                        productsWithThisName.Add(product.GetComponent<BoxCollider>());
                    else
                        Debug.LogError("No children found matching propagation trans' name!");
                }
            }
        }

        foreach (BoxCollider t in productsWithThisName)
        {
            t.localPosition = propagationTrans.localPosition;
            t.localRotation = propagationTrans.localRotation;
            t.localScale = propagationTrans.localScale;

            MeshRenderer mr = t.GetComponentInChildren<MeshRenderer>();
            if (mr && mr.transform.GetComponent<MeshFilter>().sharedMesh != null)
            {
                mr.sharedMaterials[0].color = Color.white;
                //Material newMat = Resources.
                //mr.materials[0] = Resources.Load()
            }
        }
        Debug.Log("Finished propagating collider transforms.");
    }*/

}