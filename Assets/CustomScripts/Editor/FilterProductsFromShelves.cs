using PriceTags;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class FilterProductsFromShelves : EditorWindow
{
    private static List<GameObject> productsFound = new List<GameObject>();

    private static int productLayer = LayerMask.NameToLayer("Products");

    [MenuItem("CustomMenu/Single Click Setup")]
    public static void SingleClickSetup()
    {
        FindProducts();

        DestroyRigidbodyComponent();
        DestroyDuplicateSelectableProductComponent();
        DestroyDuplicatePropagateProductTransformComponent();

        AssignStrategyManager();
        SetOutlineColor();

        UpdateProduct_Layer_And_Tag();
    }

    [MenuItem("CustomMenu/Products")]
    public static void FindProducts()
    {
        productsFound.Clear();

        productsFound = FindObjectsOfType<GameObject>().Where(x => x.name.StartsWith("F_") && x.transform.parent.name != "Products").ToList();

        var productTransfrom = GameObject.FindGameObjectWithTag("ProductsHolder");

        Debug.LogWarning("Parent Name: " + productTransfrom.name);

        if (productsFound.Count == 0)
        {
            Debug.Log("All Products are under holder");
            return;
        }

        foreach (GameObject p in productsFound)
        {
            Debug.Log(p.name);
            p.transform.SetParent(productTransfrom.transform);

            p.AddComponent<SelectableProduct>();
            p.AddComponent<PropagateProductTransform>();
            p.AddComponent<Outline>();
        }

    }

    [MenuItem("CustomMenu/Components/Rigidbody")]
    public static void ValidateRigidbodyComponent()
    {
        var productTransfrom = GameObject.FindGameObjectWithTag("ProductsHolder");

        if (productTransfrom.transform.childCount <= 0)
        {
            Debug.LogWarning("No Products under " + productTransfrom.name + " please add products first");
            return;
        }

        for (int i = 0; i < productTransfrom.transform.childCount; i++)
        {
            var child = productTransfrom.transform.GetChild(i);
            if (child.GetComponent<Rigidbody>() == null)
            {
                child.gameObject.AddComponent<Rigidbody>();
            }

            child.GetComponent<Rigidbody>().isKinematic = true;
        }

        

            Debug.Log("Rigidbody component Added on" + productTransfrom.transform.childCount + " products and set to iskinematics");
    }

    [MenuItem("CustomMenu/Components/DestoryRigidbody")]
    public static void DestroyRigidbodyComponent()
    {
        var productTransfrom = GameObject.FindGameObjectWithTag("ProductsHolder");

        if (productTransfrom.transform.childCount <= 0)
        {
            Debug.LogWarning("No Products under " + productTransfrom.name + " please add products first");
            return;
        }

        for (int i = 0; i < productTransfrom.transform.childCount; i++)
        {
            var child = productTransfrom.transform.GetChild(i);
            if (child.GetComponent<Rigidbody>() == true)
            {
                DestroyImmediate(child.GetComponent<Rigidbody>());
            }
        }
     }

        [MenuItem("CustomMenu/Components/DestoryDuplicate_SelectableProduct")]
    public static void DestroyDuplicateSelectableProductComponent()
    {
        var productTransfrom = GameObject.FindGameObjectWithTag("ProductsHolder");

        if (productTransfrom.transform.childCount <= 0)
        {
            Debug.LogWarning("No Products under " + productTransfrom.name + " please add products first");
            return;
        }

        for (int i = 0; i < productTransfrom.transform.childCount; i++)
        {
            var child = productTransfrom.transform.GetChild(i);

            var list = child.GetComponents<SelectableProduct>().ToList();

            if (list.Count > 1)
            {
                DestroyImmediate(child.GetComponent<SelectableProduct>());
                Debug.Log("Multiple Component found on " + child.name + " removing duplicate");
            }
            list.Clear();
        }
    }

    [MenuItem("CustomMenu/Components/DestoryDuplicate_PropagateProductTransform")]
    public static void DestroyDuplicatePropagateProductTransformComponent()
    {
        var productTransfrom = GameObject.FindGameObjectWithTag("ProductsHolder");

        if (productTransfrom.transform.childCount <= 0)
        {
            Debug.LogWarning("No Products under " + productTransfrom.name + " please add products first");
            return;
        }

        for (int i = 0; i < productTransfrom.transform.childCount; i++)
        {
            var child = productTransfrom.transform.GetChild(i);

            var list = child.GetComponents<PropagateProductTransform>().ToList();

            if (list.Count > 1)
            {
                DestroyImmediate(child.GetComponent<PropagateProductTransform>());
                Debug.Log("Multiple Component found on " + child.name + " removing duplicate");
            }
            list.Clear();
        }
    }

    [MenuItem("CustomMenu/Components/AssignRef")]
    public static void AssignStrategyManager()
    {
        var productTransfrom = GameObject.FindGameObjectWithTag("ProductsHolder");

        if (productTransfrom.transform.childCount <= 0)
        {
            Debug.LogWarning("No Products under " + productTransfrom.name + " please add products first");
            return;
        }

        for (int i = 0; i < productTransfrom.transform.childCount; i++)
        {
            var child = productTransfrom.transform.GetChild(i);

            var list = child.GetComponent<PropagateProductTransform>();

            var strategyMan = GameObject.FindObjectOfType<StrategiesManager>();

            if (strategyMan != null && list != null)
            {
                list.manager = strategyMan;
            }
            
        }
    }

    [MenuItem("CustomMenu/Components/SetOutlineColor")]
    public static void SetOutlineColor()
    {
        var productTransfrom = GameObject.FindGameObjectWithTag("ProductsHolder");

        if (productTransfrom.transform.childCount <= 0)
        {
            Debug.LogWarning("No Products under " + productTransfrom.name + " please add products first");
            return;
        }

        for (int i = 0; i < productTransfrom.transform.childCount; i++)
        {
            productTransfrom.transform.GetChild(i).GetComponent<Outline>().OutlineColor = new Color(0, 235, 235,255);
        }

        Debug.Log("All outline color updated");
    }

    [MenuItem("CustomMenu/LayerAndTag/UpdateProductLayer_And_Tag")]
    public static void UpdateProduct_Layer_And_Tag()
    {
        var productTransfrom = GameObject.FindGameObjectWithTag("ProductsHolder");

        if (productTransfrom.transform.childCount <= 0)
        {
            Debug.LogWarning("No Products under " + productTransfrom.name + " please add products first");
            return;
        }

        for (int i = 0; i < productTransfrom.transform.childCount; i++)
        {
            productTransfrom.transform.GetChild(i).gameObject.layer = productLayer;
            productTransfrom.transform.GetChild(i).gameObject.tag = "Product";
        }
    }

    //[MenuItem("CustomMenu/Optional/DisablePriceTags")]
    //public static void DisablePriceTags()
    //{
    //    var pricetags = GameObject.FindObjectsOfType<PriceTag>();

    //    foreach (var item in pricetags)
    //    {
    //        item.gameObject.SetActive(false);
    //    }
    //}

    //[MenuItem("CustomMenu/Optional/EnablePriceTags")]
    //public static void EnablePriceTags()
    //{
    //    var pricetags = GameObject.FindObjectsOfType<PriceTag>();

    //    foreach (var item in pricetags)
    //    {
    //        item.gameObject.SetActive(true);
    //    }
    //}
}
