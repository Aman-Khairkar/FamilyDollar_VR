using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Examples;
using VRTK;

public class StratgeySwitchVRTK : MonoBehaviour {

    public VRTK_InteractableObject linkedObject;
    public StrategySwitch switcher;

    protected virtual void OnEnable()
    {
        linkedObject = (linkedObject == null ? GetComponent<VRTK_InteractableObject>() : linkedObject);

        if (linkedObject != null)
        {
            linkedObject.InteractableObjectUsed += InteractableObjectUsed;
            linkedObject.InteractableObjectUnused += InteractableObjectUnused;
        }

    }

    protected virtual void OnDisable()
    {
        if (linkedObject != null)
        {
            linkedObject.InteractableObjectUsed -= InteractableObjectUsed;
            linkedObject.InteractableObjectUnused -= InteractableObjectUnused;
        }
    }

    protected virtual void Update()
    {
   
    }

    protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("USED");
        //switcher.SwitchStrategy();
        //linkedObject.ForceStopInteracting();
        
    }

    protected virtual void InteractableObjectUnused(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("unused");
    }
}
