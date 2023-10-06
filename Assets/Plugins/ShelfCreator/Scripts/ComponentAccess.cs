using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentAccess : MonoBehaviour {
    public static Renderer GetRenderer(GameObject obj)
    {
        Renderer result = null;

        // First, see if the object has a GetComponents script
        GetComponents gc = obj.GetComponent<GetComponents>();
        if(gc != null)
        {
            // If it has the GetComponents script, then pull the renderer from there
            result = gc.meshRenderer;
        }
        
        // If that didn't get us a renderer
        if(result == null)
        {
            // Try looking for one on the object or on its children
            result = obj.GetComponentInChildren<Renderer>();
        }

        return result;
    }
}
