using UnityEngine;
using UnityEngine.EventSystems;
//using VRTK;
using System.Collections;
using System.Collections.Generic;

public class SelectableProduct : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler*/
{

    Material mat;

    public GameObject dup = null;
    public ProductDoppelGanger pdg = null;

    bool isSelected = false;
    bool materialHighlighted = false;
    public bool useOutlineHighlighter = true;


    public float dupScalar = 1f;
    public bool firstSpin = false;
    public float customXRot = 0f;
    public float customYRot = 0f;
    public float customZRot = 0f;

    public Outline highlighter;

    void Start()
    {
        if (!highlighter)
            highlighter = GetComponent<Outline>();
    }

    
    public void TriggerPress()
    {
        if (materialHighlighted)
            SelectHighlightedProduct();
    }

    public void TriggerRelease()
    {
        if (isSelected)
            DeselectProductAndDestroyDuplicate();
    }

    void SelectHighlightedProduct()
    {
        if (!dup)
        {
            isSelected = true;
            UnSelect();
            Duplicate();

        }
    }
    void DeselectProductAndDestroyDuplicate()
    {
        if (pdg && isSelected)
        {
            isSelected = false;
            Destroy(dup.gameObject);
            transform.GetComponentInChildren<MeshRenderer>().enabled = true;
        }
    }
    void SpinDuplicate()
    {
        if (dup && isSelected)
            pdg.SpinIt();
    }

    //TODO: create script to handle picked up object to be able to put it in a cart 
    public GameObject Duplicate()
    {
        highlighter.Unhighlight();
        dup = GameObject.Instantiate(gameObject);
        dup.transform.localScale = new Vector3(dup.transform.localScale.x * dupScalar,
            dup.transform.localScale.y * dupScalar,
            dup.transform.localScale.z * dupScalar);

        SelectableProduct dupSPO = dup.GetComponent<SelectableProduct>();
        DestroyImmediate(dupSPO);
        Outline dupOutline = dup.GetComponent<Outline>();
        transform.GetComponentInChildren<MeshRenderer>().enabled = true;
        dupOutline.Unhighlight();
        DestroyImmediate(dupOutline);
        pdg = dup.AddComponent<ProductDoppelGanger>();

        pdg.Init(GeneralManager.Instance.selectingPlayerRoot);

        if (customYRot != 0 || customXRot != 0 || customZRot != 0)
        {
            pdg.transform.rotation = Quaternion.Euler(
                customXRot == 0 ? customXRot : pdg.transform.rotation.x,
                customYRot == 0 ? customYRot : pdg.transform.rotation.y,
                customZRot == 0 ? customZRot : pdg.transform.rotation.z);
        }
        if (firstSpin)
            pdg.SpinIt();
        return dup;
    }

    public void Select()
    {
        materialHighlighted = true;
        highlighter.Highlight();
    }

    public void UnSelect()
    {
        materialHighlighted = false;
        highlighter.Unhighlight();
    }
}
