using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriceTagEventHandlers : MonoBehaviour {

    public static float ShelfLocalYScale = 0f;

    public void AddEventHandlers()
    {
        PriceTags.PriceTag.FindShelf += FindShelf;
        PriceTags.PriceTag.FindIsHangingProduct += DetermineIsProductHanging;
        PriceTags.PriceTagManager.GetPriceTagScale += GetPriceTagScale;
    }

    private void OnDestroy()
    {
        PriceTags.PriceTag.FindShelf -= FindShelf;
        PriceTags.PriceTag.FindIsHangingProduct -= DetermineIsProductHanging;
        PriceTags.PriceTagManager.GetPriceTagScale -= GetPriceTagScale;

    }

    public bool DetermineIsProductHanging(GameObject product)
    {
        bool isHangingProduct = false;

        Product productScript = product.GetComponentInChildren<Product>();
        if (productScript != null)
        {
            isHangingProduct = productScript.pegProduct;
        }

        return isHangingProduct;
    }

    public GameObject FindShelf(GameObject ourProduct)
    {
        GameObject shelf = null;

        Collider productCollider = ourProduct.GetComponent<Collider>();

        if (productCollider != null)
        {
            Collider[] overlapColliders;
#if UNITY_2018_3_OR_NEWER && UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
            Bounds productBounds = PSAToText.GetUnstableIntermediateBounds(productCollider);
            productBounds.extents = new Vector3(productCollider.bounds.extents.x * 0.9f,
                                                productCollider.bounds.extents.y + 0.1f,
                                                productCollider.bounds.extents.z);

            Collider[] allColliders = FindObjectsOfType<Collider>();
            List<Collider> overlapCollidersList = new List<Collider>();
            foreach (Collider collider in allColliders)
            {
                // Since we're only looking for shelves, narrow it down to just the shelves to avoid unnecessary calculations
                if (collider.gameObject.layer == LayerMask.NameToLayer("Shelf"))
                {
                    Bounds colliderBounds = PSAToText.GetUnstableIntermediateBounds(collider);
                    if(productBounds.Intersects(colliderBounds))
                    {
                        overlapCollidersList.Add(collider);
                    }
                }
            }
            overlapColliders = overlapCollidersList.ToArray();
#else
            overlapColliders = Physics.OverlapBox(productCollider.bounds.center, productCollider.bounds.extents + new Vector3(0, 0.1f, 0));
#endif

            // Keeps track of whether we overlapped with a normal shelf so we can give those priority over pegboards
            bool foundNormalShelfObject = false;

            foreach (Collider overlapCollider in overlapColliders)
            {
                if (overlapCollider.gameObject.layer == LayerMask.NameToLayer("Shelf"))
                {
                    if (overlapCollider.name.StartsWith("shelf"))
                    {
                        shelf = overlapCollider.gameObject;
                        foundNormalShelfObject = true;
                    }
                    else if (shelf == null || !foundNormalShelfObject)
                    {
                        shelf = overlapCollider.gameObject;
                    }
                }
            }
        }

        return shelf;
    }

    public static float GetPriceTagScale()
    {
        float tagScale = 1;
        
        if (ShelfLocalYScale != 0)
        {
            //the typical shelf height non-metric is 1.58 units
            tagScale = ShelfLocalYScale / 1.58f;
        }

        return tagScale;
    }
}
