using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfCleanup : MonoBehaviour
{
    #if UNITY_EDITOR
    private Transform myTrans;
    // Use this for initialization


    //Run in editor by clicking gear icon. Cleans up unnecessary scripts from ShelfParser
    [ContextMenu("CleanUpParserHierarchy")]
    public void CleanUpParserHierarchy()
    {

        int fixtureMask = LayerMask.NameToLayer("Shelf");
        int productMask = LayerMask.NameToLayer("Products");

        myTrans = transform;
        /*ScenarioIdentifier si = GetComponentInChildren<ScenarioIdentifier>();
        if (si)
        {
            if (myTrans != si.transform)
                myTrans = si.transform;
            DestroyImmediate(si);
        }*/
        //all of the transforms under this strategy parent
        Transform[] children = myTrans.GetComponentsInChildren<Transform>();

        //build new parent objects for products and shelf geo
        GameObject productsParent = new GameObject
        {
            name = "Products"
        };
        productsParent.transform.SetParent(myTrans);
        productsParent.transform.position = Vector3.zero;
        GameObject fixturesParent = new GameObject
        {
            name = "Fixtures"
        };
        fixturesParent.transform.SetParent(myTrans);
        fixturesParent.transform.position = Vector3.zero;

        //will be populated with empty parent objects and then deleted
        List<Transform> segments = new List<Transform>();


        foreach (Transform t in children)
        {
            //Remove all components except transform(duh), mesh stuff, and box colliders
            foreach (var comp in t.gameObject.GetComponents<Component>())
            {
                if (!(comp is Transform) && (!(comp is MeshRenderer)) && (!(comp is MeshFilter)) && (!(comp is BoxCollider)))
                {
                    DestroyImmediate(comp);
                }
            }

            //shelf geo
            if (t.name.Contains("shelf") || t.name == "Fixture" || t.name == "ShelfAccent")
            {
                t.SetParent(fixturesParent.transform);
                DestroyImmediate(t.GetComponent<BoxCollider>());
                t.gameObject.layer = fixtureMask;
            }
            //products
            else if (t.name.Contains("F_"))
            {
                t.SetParent(productsParent.transform);

                //if there's a mesh filter named cubeold or cubeold instance, then this is an empty product and shouldn't be selectable
                MeshFilter mf = t.GetComponentInChildren<MeshFilter>();
                if (mf == null || mf.sharedMesh == null)
                {
                    if (mf == null)
                    {
                        Debug.LogError("No Mesh Filter on Object! " + t.name);
                        continue;
                    }
                    if(mf.sharedMesh == null)
                    {
                        Debug.LogError("No Mesh assigned to Mesh Filter on Object! " + t.name);
                        continue;
                    }
                }
                if (mf.sharedMesh.name == "Cubeold" || mf.sharedMesh.name == "Cubeold Instance")
                {
                    DestroyImmediate(t.GetComponent<BoxCollider>());
                }
                else //this is an actual product
                {
                    t.gameObject.AddComponent<SelectableProduct>();
                    PropagateProductTransform ppt = t.gameObject.AddComponent<PropagateProductTransform>();
                    ppt.manager = FindObjectOfType<StrategiesManager>();
                    t.gameObject.AddComponent<Outline>();
                    t.GetComponent<Outline>().OutlineColor = Color.cyan;
                    t.gameObject.tag = "Product";
                    t.gameObject.layer = productMask;
                }
            }
            if (t.name == "Product")//parent of the mesh, child of the F_###(UPC number)
            {
                //DestroyImmediate(t.gameObject.GetComponent<Product>());//removes TVS-specific product.cs script
            }
            if (t.name == "Segment")
            {
                segments.Add(t);
            }
        }
        //delete old parent objects
        foreach (Transform t in segments)
        {
            DestroyImmediate(t.gameObject);
        }
        Debug.Log("Shelf Cleanup completed successfully.");
    }
#endif
}
