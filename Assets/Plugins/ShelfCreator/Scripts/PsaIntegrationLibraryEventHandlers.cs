using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JDA;
using System;
using System.IO;
using PriceTags;
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class PsaIntegrationLibraryEventHandlers : MonoBehaviour
{
    public PSAToText psaToText;
    public struct SpawnedPlanogram
    {
        public GameObject PlanogramGameObject;
        public PSA.Planogram PlanogramPsaData;

        //this is to save reference to the GameObjects created to represent segments.
        //We can get rid of this if we move to a 'clean scene' generation system.
        public List<GameObject> SegmentGOs;

        //this is to save reference to the GameObjects created during position created.
        //We can get rid of this if we move to a 'clean scene' generation system.
        public List<GameObject> PositionsGOs;
        
        //to handle special operations based on bars and pegboards
        public List<GameObject> BarAndPegboardGOs;

        //to handle special operations based on normal shelves
        public List<GameObject> ShelfFixtureGOs;

        public SpawnedPlanogram(GameObject planogramGameObject, PSA.Planogram planogramPsaData)
        {
            PlanogramGameObject = planogramGameObject;
            PlanogramPsaData = planogramPsaData;

            SegmentGOs = new List<GameObject>();
            PositionsGOs = new List<GameObject>();
            BarAndPegboardGOs = new List<GameObject>();
            ShelfFixtureGOs = new List<GameObject>();
        }
    }
    List<SpawnedPlanogram> planograms = new List<SpawnedPlanogram>();
    
    public GameObject priceTagsObject;
    public GameObject togglePriceTagsPrefab;

#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
    public void AddEventHandlers() {
        PsaManager.Instance.OnPsaLoaded += OnPsaLoadedEvent;
        PsaShelfBuilder.OnPsaShelfBuilt += OnPsaShelfBuilt;
        PsaShelfBuilder.OnPlanogramObjectCreated += PlanogramObjectCreated;
        PsaShelfBuilder.OnProductPositionObjectCreated += ProductPositionObjectCreated;
        PsaShelfBuilder.OnFixtureObjectedCreated += FixtureObjectCreated;
        PsaShelfBuilder.OnShelfBackObjectCreated += BackObjectCreated;
        PsaShelfBuilder.OnShelfBaseObjectCreated += BaseObjectCreated;
        PsaShelfBuilder.OnSegmentObjectCreated += SegmentObjectCreated;
    }

    /// <summary>
    /// Set up the price tags object so that we can spawn price tags with the shelf
    /// </summary>
    public void SetUpPriceTags()
    {
        PriceTags.PriceTags PriceTags = ScriptableObject.CreateInstance("PriceTags") as PriceTags.PriceTags;
        priceTagsObject = PriceTags.Create(transform);

        SpawnPriceTags spawnPriceTags = priceTagsObject.GetComponent<SpawnPriceTags>();
        spawnPriceTags.AddEventHandlers();
    }
    
    public void OnDestroy() {
        PsaManager.Instance.OnPsaLoaded -= OnPsaLoadedEvent;
        PsaShelfBuilder.OnPsaShelfBuilt -= OnPsaShelfBuilt;
        PsaShelfBuilder.OnPlanogramObjectCreated -= PlanogramObjectCreated;
        PsaShelfBuilder.OnProductPositionObjectCreated -= ProductPositionObjectCreated;
        PsaShelfBuilder.OnFixtureObjectedCreated -= FixtureObjectCreated;
        PsaShelfBuilder.OnShelfBackObjectCreated -= BackObjectCreated;
        PsaShelfBuilder.OnShelfBaseObjectCreated -= BaseObjectCreated;
        PsaShelfBuilder.OnSegmentObjectCreated -= SegmentObjectCreated;
    }

    public void OnPsaLoadedEvent(PSA psaData)
    {
        planograms.Clear();
    }

    public void OnPsaShelfBuilt(GameObject topLevelPsaGameObject, PSA psaBuilt, List<int> pickedPlanograms, string shelfGameObjectName)
    {
        // Do some final processing on the planograms before we turn them into prefab instances
        for (int i = 0; i < planograms.Count; i++)
        {
            // Do everything we need to do with the shelf now that it has all the products and everything on it
            psaToText.ProcessPlanogram(planograms[i]);

            // Remove the topLevelPsaGameObject from being this planogram's parent
            planograms[i].PlanogramGameObject.transform.parent = null;
        }

        // Properly place price tags (needs to happen after products are marked as hanging/not, 
        //   but before we clean up products' high-level colliders and rigidbodies)
        PriceTagManager priceTagsManager = priceTagsObject.GetComponent<PriceTagManager>();
        foreach (PriceTag priceTag in priceTagsManager.PriceTags)
        {
            priceTag.RefreshPosition();
        }

        priceTagsManager.ScaleTags();

        priceTagsManager.PriceTags.Clear();

        //if the Hierarchy is not designated as organized by Segment and layer,
        //simplify it to allow easier access to each individual product GO
        if (!PSAToText.OrganizeProductsBySegmentAndLayer)
        {
            for (int i = 0; i < planograms.Count; i++)
            {
                psaToText.SimplifyHierarchy(planograms[i]);
            }
        }

        List<GameObject> newlySpawnedShelves = new List<GameObject>();
        // Turn shelves into prefab instances and fix any broken references
        for (int i = 0; i < planograms.Count; i++)
        {
            // Clean up the components we're done with. Only do so if the hierarchy has not been simplified
            if(PSAToText.OrganizeProductsBySegmentAndLayer)
                psaToText.CleanUpComponents(planograms[i]);

            //set the shelf to be the true size
            psaToText.SetTrueScale(planograms[i].PlanogramGameObject);

            // Replace the planogram with its prefab
            GameObject prefab = psaToText.ReplacePlanogramWithPrefab(planograms[i].PlanogramGameObject, planograms[i].PlanogramPsaData);
            newlySpawnedShelves.Add(prefab);

            // Re-add the price tag scripts to the manager's list after losing the references with the shelf's prefabification
            PriceTag[] tagsOnShelf = prefab.GetComponentsInChildren<PriceTag>();
            priceTagsManager.PriceTags.AddRange(tagsOnShelf);
        }

        DestroyImmediate(topLevelPsaGameObject);
        
        // If there isn't already a TogglePriceTags object in the scene
        if (FindObjectOfType<TogglePriceTags>() == null && togglePriceTagsPrefab != null)
        {
            // Add one
#if UNITY_EDITOR
            PrefabUtility.InstantiatePrefab(togglePriceTagsPrefab);
#else
            Instantiate(togglePriceTagsPrefab);
#endif
        }

        // If we're rebuilding an existing shelf
        if (PSAToText.IsRebuildingExistingShelf)
        {
            // Delete all the extra shelf objects we just generated
            for (int i = 0; i < newlySpawnedShelves.Count; i++)
            {
                DestroyImmediate(newlySpawnedShelves[i]);
            }
            newlySpawnedShelves.Clear();

            // If we have a reference to the shelf we're rebuilding
            if (PSAToText.ShelfSelectedForRebuild != null)
            {
                // Find the proper prefab that corresponds to the shelf
                GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(PSAToText.ShelfSelectedForRebuild);

                // If we found it
                if (prefab != null)
                {
                    // Get the data we need from the existing shelf GO so we can replace it
                    Transform savedParent = PSAToText.ShelfSelectedForRebuild.transform.parent;
                    Vector3 savedLocalPosition = PSAToText.ShelfSelectedForRebuild.transform.localPosition;
                    Quaternion savedLocalRotation = PSAToText.ShelfSelectedForRebuild.transform.localRotation;

                    // Generate a replacement from the prefab
                    GameObject replacementShelf = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    replacementShelf.transform.parent = savedParent;
                    replacementShelf.transform.localPosition = savedLocalPosition;
                    replacementShelf.transform.localRotation = savedLocalRotation;

                    // Destroy the original shelf in the scene and remove our reference to it
                    DestroyImmediate(PSAToText.ShelfSelectedForRebuild);
                    PSAToText.ShelfSelectedForRebuild = null;
                }
            }
        }

        // Save the PSA file
        psaToText.SavePsaFileInAssetsFolder();
    }

    public void FixtureObjectCreated(GameObject fixtureGameObject, PSA.Planogram planogramPsaData, PSA.Fixture fixturePsaData)
    {
        SpawnedPlanogram latestPlanogram = planograms[planograms.Count - 1];

        //if the fixture is a shelf type
        if (fixturePsaData.Type == PSA.Fixture.FixtureType.Shelf)
        {
            fixtureGameObject.tag = "Shelf";

            latestPlanogram.ShelfFixtureGOs.Add(fixtureGameObject);

            if (PriceTagEventHandlers.ShelfLocalYScale == 0)
            {
                PriceTagEventHandlers.ShelfLocalYScale = fixtureGameObject.transform.localScale.y;
            }
        }
        #region Other fixture types
        /*
        else if(fixturePsaData.Type == PSA.Fixture.FixtureType.Pegboard)
        {
        }
        else if(fixturePsaData.Type == PSA.Fixture.FixtureType.Bin)
        {
        }
        else if(fixturePsaData.Type == PSA.Fixture.FixtureType.Chest)
        {
        }
        else if(fixturePsaData.Type == PSA.Fixture.FixtureType.CurvedRod)
        {
        }
        else if(fixturePsaData.Type == PSA.Fixture.FixtureType.GravityFeed)
        {
        }
        else if(fixturePsaData.Type == PSA.Fixture.FixtureType.LateralRod)
        {
        }
        else if(fixturePsaData.Type == PSA.Fixture.FixtureType.MultiRowPegboard)
        {
        }
        else if(fixturePsaData.Type == PSA.Fixture.FixtureType.Obstruction)
        {
        }
        else if(fixturePsaData.Type == PSA.Fixture.FixtureType.PolygonalShelf)
        {
        }
        else if(fixturePsaData.Type == PSA.Fixture.FixtureType.Rod)
        {
        }
        */
        #endregion
        else if (fixturePsaData.Type == PSA.Fixture.FixtureType.Sign)
        {
            fixtureGameObject.tag = "POSM";
            fixtureGameObject.AddComponent<GetComponents>();
        }
        else if (fixturePsaData.Type == PSA.Fixture.FixtureType.Bar)
        {
            // Tag as Bar
            fixtureGameObject.tag = "Bar";

            latestPlanogram.BarAndPegboardGOs.Add(fixtureGameObject);
        }
        else if (fixturePsaData.Type == PSA.Fixture.FixtureType.Pegboard)
        {
            fixtureGameObject.tag = "Pegboard";

            latestPlanogram.BarAndPegboardGOs.Add(fixtureGameObject);
        }

        // Set all fixtures to static to improve performance
        fixtureGameObject.isStatic = true;
    }

    public void PlanogramObjectCreated(GameObject planogramGameObject, PSA.Planogram planogramPsaData, int planogramIndex)
    {
        //RenderLSPHandler.StrategyNames.Add(planogramGameObject.name);

        //give the shelf parent the ScenarioIdentifier script
        ScenarioIdentifier scenID = planogramGameObject.AddComponent<ScenarioIdentifier>();
        scenID.ourScenario = planogramIndex;

        planograms.Add(new SpawnedPlanogram(planogramGameObject, planogramPsaData));
    }

    public void ProductPositionObjectCreated(GameObject positionGameObject, PSA.Planogram planogramPsaData, PSA.Position positionPsaData, PSA.Product productPsaData)
    {
        positionGameObject.AddComponent<GetComponents>();
        positionGameObject.AddComponent<Position>();

        List<float> positionRowXVals = new List<float>();
        List<float> positionRowYVals = new List<float>();

        for (int i = 0; i < positionGameObject.transform.childCount; i++)
        {
            Transform productRow = positionGameObject.transform.GetChild(i);
            // If this is actually a product row
            if (productRow.name.StartsWith("ProductRow"))
            {
                // Save the x and y positions of this depth row
                positionRowXVals.Add(productRow.localPosition.x);
                positionRowYVals.Add(productRow.localPosition.y);

                // Process each of the RotateParent GameObjects under it
                for (int j = 0; j < productRow.childCount; j++)
                {
                    Transform rotateParent = productRow.GetChild(j);

                    // Attach necessary scripts
                    rotateParent.gameObject.AddComponent<Product>();

                    // Name the product correctly
                    if (j == productRow.childCount - 1)
                    {
                        // Mark the front-most product with an "F"
                        rotateParent.name = "F_" + productPsaData.UPC;

                    }
                    else
                    {
                        // Mark all of the ones behind the front product with a "B"
                        rotateParent.name = "B_" + productPsaData.UPC;
                    }

                    // TODO: Find out if this product is 3D
                    bool is3D = false;

                    // Prepare the product for VR
                    psaToText.ApplyOVRComponents(rotateParent.gameObject, productPsaData, positionPsaData, is3D);
                }
            }
            // If this is not a ProductRow, but instead a RotateParent on the top level
            else if (productRow.name.StartsWith("RotateParent"))
            {
                // Then this is a cap
                Transform rotateParent = productRow;

                CapIdentifier capIDScript = rotateParent.gameObject.AddComponent<CapIdentifier>();

                // Name the product correctly
                if (!positionRowXVals.Contains(rotateParent.localPosition.x))
                {
                    // Mark this as an X Cap
                    rotateParent.name = "XCap_" + productPsaData.UPC;
                    capIDScript.capStyle = CapIdentifier.CapStyle.x;
                }
                else if (!positionRowYVals.Contains(rotateParent.localPosition.y))
                {
                    // Mark this as a Y Cap
                    rotateParent.name = "YCap_" + productPsaData.UPC;
                    capIDScript.capStyle = CapIdentifier.CapStyle.y;
                }
                else
                {
                    // Mark this as a Z Cap
                    rotateParent.name = "ZCap_" + productPsaData.UPC;
                    capIDScript.capStyle = CapIdentifier.CapStyle.z;
                }
            }
        }

        planograms[planograms.Count - 1].PositionsGOs.Add(positionGameObject);
    }

    public void BackObjectCreated(GameObject shelfBackGameObject, PSA.Planogram planogramPsaData)
    {
        // Set the shelf back to static to improve performance
        shelfBackGameObject.isStatic = true;
    }

    public void BaseObjectCreated(GameObject shelfBaseGameObject, PSA.Planogram planogramPsaData)
    {
        // Set the shelf base to static to improve performance
        shelfBaseGameObject.isStatic = true;
    }

    public void SegmentObjectCreated(GameObject segmentGameObject, PSA.Planogram planogramPsaData, PSA.Segment segmentPsaData)
    {
        // Make sure the segment doesn't have a zero scale
        psaToText.AdjustSegment(planogramPsaData, segmentGameObject);

        //add the GO to our list of segments
        planograms[planograms.Count - 1].SegmentGOs.Add(segmentGameObject);
    }
#endif
}
