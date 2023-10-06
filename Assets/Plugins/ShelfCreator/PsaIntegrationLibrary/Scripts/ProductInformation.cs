using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ProductInformation : MonoBehaviour
{

    public int HFacings;
    public int VFacings;
    public int DFacings;

    public Vector3 Size;

    private const int MinFacings = 1;
    private const int MaxFacings = 50;

    private Transform _mesh;

    public bool Added = false;

    void Start()
    {
        _mesh = FindGameObjectInChildWithTag(transform.gameObject, "PositionRenderer");

        if (_mesh == null) return;
        Size = _mesh.localScale; 
    }

    /// <summary>
    /// Adds facings for either horizontal or vertical facings.
    /// </summary>
    /// <param name="isHFacing">Tells if the hfacing or vfacing is updated.</param>
    public bool AddFacing(bool isHFacing)
    {
        if (isHFacing)
        {
            //Do not add any more HFacings if we hit the max. 
            if (HFacings == MaxFacings) return false;
            HFacings += 1;
            return true;
        }
        else
        {
            //Do not add any more VFacings if we hit the max. 
            if (VFacings == MaxFacings) return false;
            VFacings += 1; 
            return true;
        }

        //Update the size
        if(_mesh != null)
            Size = _mesh.localScale;
    }

    /// <summary>
    /// Removes facings for either horizontal or vertical facings.
    /// </summary>
    /// <param name="isHFacing">Tells if the hfacing or vfacing is updated.</param>
    public void RemoveFacing(bool isHFacing)
    {
        if (isHFacing)
        {
            //Do not remove any more HFacings if we hit the min. 
            if (HFacings == MinFacings) return;
            HFacings -= 1;
        }
        else
        {
            //Do not remove any more HFacings if we hit the min. 
            if (VFacings == MinFacings) return;
            VFacings -= 1;
        }

        //Update the size
        if (_mesh != null)
            Size = _mesh.localScale;
    }

    /// <summary>
    /// checks and retuns true if the facings can be removed.
    /// </summary>
    /// <param name="isHFacing"></param>
    /// <returns></returns>
    public bool CanRemoveFacing(bool isHFacing)
    {
        if (isHFacing)
        {
            //Do not remove any more HFacings if we hit the min. 
            return !(HFacings == MinFacings);
        }
        else
        {
            //Do not remove any more VFacings if we hit the min. 
            return !(VFacings == MinFacings);
        }
    }

    /// <summary>
    /// Gets a child object based on a tag
    /// </summary>
    /// <param name="parent">Parent object</param>
    /// /// <param name="tag">Tag of the child</param>
    private Transform FindGameObjectInChildWithTag(GameObject parent, string tag)
    {
        Transform t = parent.transform;

        for (int i = 0; i < t.childCount; i++)
        {
            if (t.GetChild(i).gameObject.tag == tag)
            {
                return t.GetChild(i);
            }

        }

        return null;
    }

}
