using System.Linq;
using System.Collections;
using System.Collections.Generic;

using JDA;
using UnityEngine;

public static class PsaShelfBuilder {
    public delegate void PsaShelfBuilt(GameObject topLevelGameObject, PSA psaToBuild, List<int> pickedPlanograms, string shelfGameObjectName);
    public delegate void PlanogramObjectCreatedEvent(GameObject planogramGameObject, PSA.Planogram planogramPsaData, int planogramIndex);
    public delegate void ProductPositionObjectCreatedEvent(GameObject positionGameObject, PSA.Planogram planogramPsaData, PSA.Position positionPsaData, PSA.Product productPsaData);
    public delegate void ShelfBackObjectCreatedEvent(GameObject shelfBackGameObject, PSA.Planogram planogramPsaData);
    public delegate void ShelfBaseObjectCreatedEvent(GameObject shelfBaseGameObject, PSA.Planogram planogramPsaData);
    public delegate void FixtureObjectCreatedEvent(GameObject fixtureGameObject, PSA.Planogram planogramPsaData, PSA.Fixture fixturePsaData);
    public delegate void SegmentObjectCreatedEvent(GameObject segmentGameObject, PSA.Planogram planogramPsaData, PSA.Segment segmentPsaData);
    public delegate void UpdateProgressEvent(float progress, string statusMessage, int currentShelfIndex, int totalShelfCount);

    public static PsaShelfBuilt OnPsaShelfBuilt;
    public static PlanogramObjectCreatedEvent OnPlanogramObjectCreated;
    public static ProductPositionObjectCreatedEvent OnProductPositionObjectCreated;
    public static ShelfBackObjectCreatedEvent OnShelfBackObjectCreated;
    public static ShelfBaseObjectCreatedEvent OnShelfBaseObjectCreated;
    public static FixtureObjectCreatedEvent OnFixtureObjectedCreated;
    public static SegmentObjectCreatedEvent OnSegmentObjectCreated;
    public static UpdateProgressEvent OnProgressChange;
    
    public enum PositionBuildOption
    {
        SimplePositionCube,
        SeparateProductObjects,
        CombinedProductObjects
    }

    public static float SpaceBetweenPlanograms = 50.0f;

    public static float CurrentProgress = 0f;
    public static string StatusMessage = "";
    public static int CurrentShelfBuildIndex = -1;
    public static int TotalShelfCount = 0;
    public static float CurrentStepIncrement = 0;

    public const float CREATE_PLANOGRAM_OBJECT_INCREMENT = 0.05f;
    public const float BUILD_PRODUCT_POSITIONS_INCREMENT = 0.5f;
    public const float BUILD_SHELF_FIXTURES_INCREMENT = 0.1f;
    public const float BUILD_SEGMENTS_INCREMENT = 0.05f;
    public const float BUILD_SHELF_BACK_INCREMENT = 0.05f;
    public const float BUILD_SHELF_BASE_INCREMENT = 0.05f;
    public const float FINALIZE_SHELVES_INCREMENT = 0.2f;

    public static bool BuildInProgress = false;
    

    /// <summary>
    /// Takes the PSA data and a set of prefabs for creating the PSA into a game object
    /// </summary>
    /// <param name="psaToBuild">The PSA file we are building the shelf from</param>
    /// <param name="pickedPlanograms">List of strategies picked fro building</param>
    /// <param name="shelfGameObjectName">What to name the parent gameobject of the shelf</param>
    /// <param name="buildPositionInSingleMesh">Decides whether to build product positions quickly in single mesh or create a new object for each product in position</param>
    /// <param name="productPositionPrefab">Prefab used for creating Product Position objects</param>
    /// <param name="fixturePrefab">Prefab used for creating Fixture objects</param>
    /// <param name="posmPrefab">Prefab used for creating POSM objects</param>
    /// <param name="segmentPrefab">Prefab used for creating Segment objects</param>
    /// <param name="shelfAccentsPrefab">Prefab used for Shelf Accents objects (Base and Back)</param>
    /// <param name="shelfMaterial">Material for Shelfs</param>
    /// <param name="backMaterial">Material for Back objects</param>
    /// <param name="baseMaterial">Material for Base objects</param>
    /// <returns>Top level GameObject of new PSA shelf</returns>
    public static IEnumerator BuildPsaShelf(PSA psaToBuild, List<int> pickedPlanograms, string shelfGameObjectName, PositionBuildOption positionBuildOption,
                                    GameObject productPositionPrefab, GameObject fixturePrefab, GameObject posmPrefab, 
                                    GameObject segmentPrefab, GameObject shelfAccentsPrefab,
                                    Material shelfMaterial, Material backMaterial, Material baseMaterial,
                                    bool performanceMode = false, Mesh simplifiedBoxMesh = null, string materialsFolderParentPath = "")
    {
        if (psaToBuild == null || pickedPlanograms == null || pickedPlanograms.Count == 0 
            || productPositionPrefab == null || fixturePrefab == null || posmPrefab == null 
            || segmentPrefab == null || shelfAccentsPrefab == null) {
                yield break;
        }

        BuildInProgress = true;

        CurrentProgress = 0;
        StatusMessage = "Starting shelf build";
        TotalShelfCount = pickedPlanograms.Count;
        UpdateProgressBar();
        yield return null;

        float planPositionX = 0.0f;
        GameObject parentObject = new GameObject(shelfGameObjectName);
        List<string> planos = new List<string>(psaToBuild.TheProject.Planograms.Keys);
        float progressIncrement = 1.0f / pickedPlanograms.Count / 8.0f;

        for (int i = 0; i < pickedPlanograms.Count; i++)
        {
            CurrentShelfBuildIndex = i;

            int planoId = pickedPlanograms[i];
            PSA.Planogram planogram = psaToBuild.TheProject.Planograms[planos[planoId]];

            IncrementProgressBar(0, "Creating planogram object");
            CurrentStepIncrement = CREATE_PLANOGRAM_OBJECT_INCREMENT / TotalShelfCount;
            yield return null;
            GameObject planoObject = CreatePlanogramObject(planogram, planoId);
            OrientPlanogramObject(planoObject, parentObject, planPositionX);

            IncrementProgressBar(CurrentStepIncrement, "Creating position objects");
            CurrentStepIncrement = BUILD_PRODUCT_POSITIONS_INCREMENT / TotalShelfCount;
            yield return null;
            SpawnProductPositions(positionBuildOption, productPositionPrefab, planogram, psaToBuild.TheProject.Products, planoId, planoObject, performanceMode, simplifiedBoxMesh, materialsFolderParentPath);
            
            // Create the shelf
            IncrementProgressBar(CurrentStepIncrement, "Creating fixture objects");
            CurrentStepIncrement = BUILD_SHELF_FIXTURES_INCREMENT / TotalShelfCount;
            yield return null;
            SpawnFixtures(fixturePrefab, posmPrefab, shelfMaterial, planogram, planoId, planoObject, materialsFolderParentPath);
            
            // spawn the segments
            IncrementProgressBar(CurrentStepIncrement, "Creating segment objects");
            CurrentStepIncrement = BUILD_SEGMENTS_INCREMENT / TotalShelfCount;
            yield return null;
            SpawnSegments(segmentPrefab, planogram, planoObject);

            // Spawn the back of the planogram
            IncrementProgressBar(CurrentStepIncrement, "Creating shelf back");
            CurrentStepIncrement = BUILD_SHELF_BACK_INCREMENT / TotalShelfCount;
            yield return null;
            SpawnBack(shelfAccentsPrefab, backMaterial, psaToBuild, planogram, planoObject, materialsFolderParentPath);

            // Spawn the base of the planogram
            IncrementProgressBar(CurrentStepIncrement, "Creating shelf base");
            CurrentStepIncrement = BUILD_SHELF_BASE_INCREMENT / TotalShelfCount;
            yield return null;
            SpawnPlanogramBase(shelfAccentsPrefab, baseMaterial, planogram, planoObject, materialsFolderParentPath);

            // Set the nex plano x position
            planPositionX = planPositionX + planogram.Width + SpaceBetweenPlanograms;

            IncrementProgressBar(CurrentStepIncrement, "Shelf " + (i + 1) + "/" + pickedPlanograms.Count + " complete");
            CurrentStepIncrement = 0;
            yield return null;
        }

        CurrentShelfBuildIndex = -1;

        IncrementProgressBar(0, "Finalizing shelf setup");
        CurrentStepIncrement = FINALIZE_SHELVES_INCREMENT;
        yield return null;
        if (OnPsaShelfBuilt != null)
        {
            OnPsaShelfBuilt(parentObject, psaToBuild, pickedPlanograms, shelfGameObjectName);
        }

        PsaManager.TempTopLevelPsaGameObject = parentObject;
        
        CurrentProgress = 1;
        StatusMessage = "Shelf build complete!";
        UpdateProgressBar();
        yield return null;

        BuildInProgress = false;
    }

    private static GameObject CreatePlanogramObject(PSA.Planogram planogram, int planoId)
    {
        // Change the movement period to 0 to fix certain shelves
        planogram.Fields["Movement period"] = "0";

        // Create a parent gameObject to place everything under. Name it the name of our game title so we can carry it over to the next scene for the save system.
        GameObject planoObject = new GameObject(planogram.Name);

        if (OnPlanogramObjectCreated != null)
        {
            OnPlanogramObjectCreated(planoObject, planogram, planoId);
        }
        
        return planoObject;
    }

    /// <summary>
    /// After you createa planogram game object, we can re-parent the object and set it's new positionings
    /// </summary>
    /// <param name="planoObject">Gameobject representing the planogram</param>
    /// <param name="parentObject">Parent Game object for PSA holding reference to all the planograms</param>
    /// <param name="planoPositionX">Offset of this planogram inside the parent</param>
    private static void OrientPlanogramObject(GameObject planoObject, GameObject parentObject, float planoPositionX) {
        planoObject.transform.SetParent(parentObject.transform);
        planoObject.transform.position = new Vector3(planoPositionX, 0, 0);
    }

    /// <summary>
    /// Create the product gameobjects from position data in the planogram.
    /// </summary>
    /// <param name="thePlanogram">The planogram from which to get the fixture data.</param>
    /// <param name="positionParent">The transform to be set as the parent of all of the positions we spawn.</param>
    private static void SpawnProductPositions(PositionBuildOption positionBuildOption, GameObject productPositionPrefab, PSA.Planogram planogram, 
                                            Dictionary<string, PSA.Product> products, int planoId, GameObject planoObject, bool performanceMode = false,
                                            Mesh simplifiedBoxMesh = null, string materialsFolderParentPath = "")
    {
        //get a list of the positions in the strategy
        List<PSA.Position> positions = planogram.AllPositions();

        //// Break up the position-building step into smaller increments
        //float individualPositionProgressIncrement = CurrentStepIncrement;
        //if (positions.Count > 0)
        //{
        //    individualPositionProgressIncrement /= positions.Count;
        //}
        //// If something went wrong and we got a negative number
        //if (individualPositionProgressIncrement < 0)
        //{
        //    // Better to not decrease the progress bar and just let the outer loop handle the update instead
        //    individualPositionProgressIncrement = 0;
        //}

        for (int i = 0; i < positions.Count; i++)
        {
            //get the product upc
            string productUPC = positions[i].UPC;
            //get the product ID
            string productID = positions[i].ID;
            //combine ID:UPC
            //this is what we have to do to match up the textures with the name of the position
            string productKey = (productID + ":" + productUPC);

            //if our product exists and it actually has facings
            if (products.ContainsKey(productKey))
            {
                PSA.Product ourProduct = products[productKey];
                PSA.Position ourPosition = positions[i];
                
                if((ourPosition.DFacings > 0) || (ourPosition.HFacings > 0) || (ourPosition.VFacings > 0))
                {
                    GameObject spawnedGameObject = PsaProductPostionBuilder.SpawnPositionGameObject(planogram, ourProduct, ourPosition, 
                                                                                                    planoId, productKey, i, 
                                                                                                    planoObject, productPositionPrefab);

                    switch (positionBuildOption)
                    {
                        case PositionBuildOption.CombinedProductObjects:
                            {
                                PsaProductPostionBuilder.BuildIndividualProductsPerPosition(planogram, ourProduct, ourPosition,
                                                                                        planoId, i, productKey, spawnedGameObject,
                                                                                        planoObject, productPositionPrefab,
                                                                                        true, simplifiedBoxMesh, materialsFolderParentPath);
                                break;
                            }
                        case PositionBuildOption.SeparateProductObjects:
                            {
                                PsaProductPostionBuilder.BuildIndividualProductsPerPosition(planogram, ourProduct, ourPosition,
                                                                                        planoId, i, productKey, spawnedGameObject,
                                                                                        planoObject, productPositionPrefab,
                                                                                        false, simplifiedBoxMesh, materialsFolderParentPath);
                                break;
                            }
                        case PositionBuildOption.SimplePositionCube:
                            {
                                PsaProductPostionBuilder.BuildInSinglePosition(ourProduct, ourPosition, spawnedGameObject);
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }

                    // Performance mode, turns off Shadows for gameobjects' MeshRenderers that are Products
                    if (performanceMode)
                    {
                        MeshRenderer[] spawnedGO_childrenMeshRenderers = spawnedGameObject.GetComponentsInChildren<MeshRenderer>();
                        foreach (MeshRenderer meshRenderer in spawnedGO_childrenMeshRenderers)
                        {
                            meshRenderer.lightProbeUsage = 0;
                            meshRenderer.reflectionProbeUsage = 0;
                            meshRenderer.shadowCastingMode = 0;
                            meshRenderer.receiveShadows = true;
                        }

                    }

                    if (OnProductPositionObjectCreated != null)
                    {
                        OnProductPositionObjectCreated(spawnedGameObject, planogram, ourPosition, ourProduct);
                    }
                }
            }

            //// Update the progress bar
            //IncrementProgressBar(individualPositionProgressIncrement);
            //// Subtract the progress amount that we just took care of
            //CurrentStepIncrement -= individualPositionProgressIncrement;
            //// If something went wrong and the progress bar will try to decrease once we return to the outer loop
            //if (CurrentStepIncrement < 0)
            //{
            //    // Better to not increment it at all next time
            //    CurrentStepIncrement = 0;
            //}
            //yield return null;
        }

        //yield return null;
    }

    /// <summary>
    /// Create the shelf gameobjects from fixture data in the planogram.
    /// </summary>
    /// <param name="thePlanogram">The planogram from which to get the fixture data.</param>
    /// <param name="fixtureParent">The transform to be set as the parent of all of the fixtures we spawn.</param>
    private static void SpawnFixtures(GameObject fixturePrefab, GameObject posmPrefab, Material shelfMaterial, PSA.Planogram planogram, int planoId, 
                                        GameObject parentObject, string materialsFolderParentPath = "")
    {
        //get a list of our fixtures
        List<string> fixturesList = new List<string>();
        //populate our fixture list
        fixturesList = planogram.Fixtures.Keys.ToList();

        //// Break up the fixture-building step into smaller increments
        //float individualFixtureProgressIncrement = CurrentStepIncrement;
        //if (fixturesList.Count > 0)
        //{
        //    individualFixtureProgressIncrement /= fixturesList.Count;
        //}
        //// If something went wrong and we got a negative number
        //if (individualFixtureProgressIncrement < 0)
        //{
        //    // Better to not decrease the progress bar and just let the outer loop handle the update instead
        //    individualFixtureProgressIncrement = 0;
        //}

        // Keep a number that we can add to the end of each sign to give it a unique id
        int signCounter = 0;

#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
         * If we're working in the editor, then the given shelf material is a template we'll need to copy.
         * We'll check if a copy already exists for this planogram, and if not, we'll make our own copy.
         * We'll also apply the user's custom shelf image, if one exists.
         * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
        // Find the shelf material for this strategy if it already exists, or create a new one if it doesn't
        string shelfMaterialPath = materialsFolderParentPath + "/" + ApplyEditorMaterialsToProducts.MaterialsFolderName + "/" + PsaManager.Instance.LoadedPSA.TheProject.Name + "_" + planogram.Name + "_Shelf.mat";
        shelfMaterial = ApplyEditorMaterialsToProducts.FindOrCreateMaterial(shelfMaterial, shelfMaterialPath);
        
        // Find the custom shelf texture if it exists
        Texture2D shelfTexture = Resources.Load("T_Shelf") as Texture2D;
        if (shelfTexture != null)
        {
            // Apply the shelf texture to the shelf material
            shelfMaterial.mainTexture = shelfTexture;
        }
#endif

        //cycle through each fixture in the PSA
        for (int i = 0; i < fixturesList.Count; i++)
        {
            //get the fixture that we're working with
            PSA.Fixture fixture = planogram.Fixtures[fixturesList[i]];
            GameObject fixtureObject = null;
            //if the fixture is a shelf type
            if (fixture.Type == PSA.Fixture.FixtureType.Shelf)
            {
                fixtureObject = BuildShelfFixture(fixturePrefab, shelfMaterial, planogram, fixture, parentObject, i);
            }
            else if (fixture.Type == PSA.Fixture.FixtureType.Sign)
            {
                fixtureObject = BuildSignFixture(posmPrefab, signCounter, planogram, planoId, fixture, parentObject, materialsFolderParentPath);
                signCounter++;
            }
            else if (fixture.Type == PSA.Fixture.FixtureType.Bar)
            {
                fixtureObject = BuildBarFixture(fixturePrefab, planogram, fixture, parentObject, i);
            }
            else if (fixture.Type == PSA.Fixture.FixtureType.Pegboard)
            {
                fixtureObject = BuildPegboardFixture(fixturePrefab, planogram, fixture, parentObject, i);
            }

            if (OnFixtureObjectedCreated != null) {
                OnFixtureObjectedCreated(fixtureObject, planogram, fixture);
            }

            //// Update the progress bar
            //IncrementProgressBar(individualFixtureProgressIncrement);
            //// Subtract the progress amount that we just took care of
            //CurrentStepIncrement -= individualFixtureProgressIncrement;
            //// If something went wrong and the progress bar will try to decrease once we return to the outer loop
            //if (CurrentStepIncrement < 0)
            //{
            //    // Better to not increment it at all next time
            //    CurrentStepIncrement = 0;
            //}
            //yield return null;
        }
        //yield return null;
    }

    private static GameObject BuildShelfFixture(GameObject fixturePrefab, Material shelfMaterial,
                                    PSA.Planogram planogram, PSA.Fixture fixture, GameObject parentObject, int i) {
        //spawn the shelf out of a prefab
        GameObject fixtureObject = GameObject.Instantiate(fixturePrefab);
        fixtureObject.name = "shelf_" + fixture + "_" + parentObject.transform.name + "_" + i; 
        //set our parent to be the same as the products
        fixtureObject.transform.SetParent(parentObject.transform);
        
        // Get the depth of the fixture object, make sure it's not 0
        float nonZeroFixtureDepth = fixture.Depth == 0 ? 0.001f : fixture.Depth;

        //Here we have to determine if the shelf can be
        //get where the shelf is
        /*
        * Note that PSA files keep an object's bottom-left corner as its position 
        * while Unity keeps the object's center as its position 
        * (hence the adjustment by its dimensions / 2).
        */
        fixtureObject.transform.localPosition = new Vector3(fixture.X + fixture.Width / 2,
                                    fixture.Y + fixture.Height / 2,
                                    (planogram.Depth - fixture.Z) - nonZeroFixtureDepth / 2);
        //set our size
        fixtureObject.transform.localScale = new Vector3(fixture.Width, fixture.Height, nonZeroFixtureDepth);

        // Apply the material to the renderer
        Renderer renderer = fixtureObject.GetComponent<Renderer>();
        Material instancedMaterial;
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
        renderer.sharedMaterial = shelfMaterial;
        instancedMaterial = renderer.sharedMaterial;
#else
        renderer.material = shelfMaterial;
        instancedMaterial = renderer.material;
#endif

        //// Find how many copies of the shelf texture can fit across the planogram
        //float howManyAcross = (ourMaterial.mainTexture.height * fixtureObject.transform.localScale.x) / (ourMaterial.mainTexture.width * fixtureObject.transform.localScale.y);
        //howManyAcross = howManyAcross > 0 ? howManyAcross : 1;
        //// Make a vector with the desired scaling for each dimension
        //Vector3 tiling = new Vector3(howManyAcross, ourMaterial.GetTextureScale("_MainTex").y);
        //// Update the material's main texture scale with the new tiling
        //ourMaterial.SetTextureScale("_MainTex", tiling);

        // Get the color of the fixture
        Color fixtureColor = new Color(fixture.Color.R / 255.0f,
                                        fixture.Color.G / 255.0f,
                                        fixture.Color.B / 255.0f);

        // Apply the color to the renderer
        instancedMaterial.SetColor("_Color", fixtureColor);

        return fixtureObject;
    }

    /// <summary>
    /// Spawns a GameObject for the specified bar.
    /// </summary>
    /// <param name="fixturePrefab">The prefab from which to spawn the bar objects.</param>
    /// <param name="planogram">The planogram from which to get the fixture data.</param>
    /// <param name="fixture">The fixture data from the PSA file for the bar we should spawn.</param>
    /// <param name="parentObject">The object that the fixture spawned in this method should be parented to.</param>
    /// <param name="i">The number to append to the name of the fixture we spawn so it can be uniquely identified.</param>
    /// <returns>The newly spawned GameObject representing the specified bar.</returns>
    private static GameObject BuildBarFixture(GameObject fixturePrefab, PSA.Planogram planogram, PSA.Fixture fixture, GameObject parentObject, int i)
    {
        //spawn the bar out of a prefab
        GameObject fixtureObject = GameObject.Instantiate(fixturePrefab);
        fixtureObject.name = "bar_" + fixture + "_" + parentObject.transform.name + "_" + i;
        //set our parent to be the same as the products
        fixtureObject.transform.SetParent(parentObject.transform);


        // Get the depth of the fixture object, make sure it's not 0
        float nonZeroFixtureDepth = fixture.Depth == 0 ? 0.001f : fixture.Depth;
        
        //get where the bar is
        /*
        * Note that PSA files keep an object's bottom-left corner as its position 
        * while Unity keeps the object's center as its position 
        * (hence the adjustment by its dimensions / 2).
        */
        fixtureObject.transform.localPosition = new Vector3(fixture.X + fixture.Width / 2,
                                    fixture.Y + fixture.Height / 2,
                                    (planogram.Depth - fixture.Z) - nonZeroFixtureDepth / 2);
        //set our size
        fixtureObject.transform.localScale = new Vector3(fixture.Width, fixture.Height, nonZeroFixtureDepth);


        // turn off the renderer so that the bar isn't visible
        Renderer renderer = fixtureObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }

        return fixtureObject;
    }

    /// <summary>
    /// Spawns a GameObject for the specified pegboard.
    /// </summary>
    /// <param name="fixturePrefab">The prefab from which to spawn the pegboard objects.</param>
    /// <param name="planogram">The planogram from which to get the fixture data.</param>
    /// <param name="fixture">The fixture data from the PSA file for the pegboard we should spawn.</param>
    /// <param name="parentObject">The object that the fixture spawned in this method should be parented to.</param>
    /// <param name="i">The number to append to the name of the fixture we spawn so it can be uniquely identified.</param>
    /// <returns>The newly spawned GameObject representing the specified pegboard.</returns>
    private static GameObject BuildPegboardFixture(GameObject fixturePrefab, PSA.Planogram planogram, PSA.Fixture fixture, GameObject parentObject, int i)
    {
        //spawn the pegboard out of a prefab
        GameObject fixtureObject = GameObject.Instantiate(fixturePrefab);
        fixtureObject.name = "Pegboard_" + fixture + "_" + parentObject.transform.name + "_" + i;
        //set our parent to be the same as the products
        fixtureObject.transform.SetParent(parentObject.transform);


        // Get the depth of the fixture object, make sure it's not 0
        float nonZeroFixtureDepth = fixture.Depth == 0 ? 0.001f : fixture.Depth;

        //get where the pegboard is
        /*
        * Note that PSA files keep an object's bottom-left corner as its position 
        * while Unity keeps the object's center as its position 
        * (hence the adjustment by its dimensions / 2).
        */
        fixtureObject.transform.localPosition = new Vector3(fixture.X + fixture.Width / 2,
                                    fixture.Y + fixture.Height / 2,
                                    (planogram.Depth - fixture.Z) - nonZeroFixtureDepth / 2);
        //set our size
        fixtureObject.transform.localScale = new Vector3(fixture.Width, fixture.Height, nonZeroFixtureDepth);

        // turn off the renderer so that the pegboard isn't visible
        Renderer renderer = fixtureObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }

        return fixtureObject;
    }

    private static GameObject BuildSignFixture(GameObject posmPrefab, int signCounter, PSA.Planogram planogram, int planoId, 
                                                PSA.Fixture fixture, GameObject parentObject, string materialsFolderParentPath = "") {
        // Spawn the sign out of a prefab
        GameObject fixtureObject = GameObject.Instantiate(posmPrefab);
        fixtureObject.name = planoId + "_" + "Sign_" + fixture.Name + "_" + signCounter;
        fixtureObject.transform.SetParent(parentObject.transform);

        fixtureObject.AddComponent<POSMInformation>();

        /*
        * Note that PSA files keep an object's bottom-left corner as its position 
        * while Unity keeps the object's center as its position. We don't need to
        * account for this here because it is already accounted for in the prefab.
        */
        // Get the depth of the fixture object, make sure it's not 0
        float nonZeroFixtureDepth = fixture.Depth == 0 ? 0.001f : fixture.Depth;
        // Set the scale of the fixture
        fixtureObject.transform.localScale = new Vector3(fixture.Width, fixture.Height, nonZeroFixtureDepth);

        // Get the slope (could be inverted, but 'the side view' doesn't tell us definitively one way or the other).
        float slope = 0;
        try
        {
            string slopeStr = fixture.Fields["Slope"];
            slope = float.Parse(slopeStr);
        }
        catch
        {
            Debug.Log("Unable to parse slope.");
        }

        // Get the angle (y rotation)
        float angle = 0;
        try
        {
            string angleStr = fixture.Fields["Angle"];
            angle = float.Parse(angleStr);
        }
        catch
        {
            Debug.Log("Unable to parse angle.");
        }

        // Get the roll (inverted z rotation)
        float roll = 0;
        try
        {
            string rollStr = fixture.Fields["Roll"];
            roll = float.Parse(rollStr);
        }
        catch
        {
            Debug.Log("Unable to parse roll.");
        }

        // Figure out our rotation
        Vector3 rotation = new Vector3(slope, angle, -roll);
        fixtureObject.transform.localRotation = Quaternion.Euler(rotation);

        //get where the sign is
        fixtureObject.transform.localPosition = new Vector3(fixture.X,
                                                            fixture.Y,
                                                            planogram.Depth - fixture.Z);
        
        // Get a reference to the renderer
        Renderer renderer = fixtureObject.GetComponentInChildren<Renderer>();

        // Find the sign's texture and apply it to all faces of the POSM
        string posmTextureName = "";
        // Get the name of the texture
        posmTextureName = fixture.Fields["Bitmap ID"];
        // If we got a texture name
        if (posmTextureName != "")
        {
            // Then we just care about the last part, the file name
            posmTextureName = System.IO.Path.GetFileNameWithoutExtension(posmTextureName);
        }
        else
        {
            // Otherwise we'll try using the name of the fixture
            posmTextureName = fixture.Name;
        }

#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
        Material[] materials = ApplyEditorMaterialsToProducts.CreatePosmMaterials(posmTextureName, materialsFolderParentPath, renderer);
        renderer.materials = materials;
#else
        ApplyImagesToProducts.Instance.ApplyPosmTextures(renderer, posmTextureName);

        // Ensure that each texture is shown in the correct orientation
        // Guess at whether it's a perpendicular or a header sign based on the y rotation
        if (rotation.y == 90)
        {
            UVManager.SetPerpendicularPosmUVs(fixtureObject);
        }
        else if (fixture.Fields[PSA.Fixture.FieldNames.Desc1].StartsWith("Speed Shelf"))
        {
            UVManager.SetCustomPosmUVs(fixtureObject);
        }
        else
        {
            UVManager.SetHeaderPosmUVs(fixtureObject);
        }
#endif

        return fixtureObject;
    }

    private static void SpawnSegments(GameObject segmentPrefab, PSA.Planogram planogram, GameObject parentObject)
    {
        foreach(PSA.Segment segment in planogram.Segments)
        {
            // Creat an object to represent our segment
            GameObject segmentObject = GameObject.Instantiate(segmentPrefab);

            // Set the shelf parent as the segment's parent
            segmentObject.transform.SetParent(parentObject.transform);

            // Find the scale of the segment object
            Vector3 scale = new Vector3(segment.Width, segment.Height, segment.Depth);
            if(segment.Height == 0)
            {
                scale.y = planogram.Height;
            }
            segmentObject.transform.localScale = scale;

            // Find the position of the segment object
            float offsetX = float.Parse(segment.Fields[PSA.Segment.FieldNames.OffsetX]);
            float offsetY = float.Parse(segment.Fields[PSA.Segment.FieldNames.OffsetY]);
            Vector3 position = new Vector3(segment.X + scale.x / 2 + offsetX, 
                                            segment.Y + scale.y / 2 + offsetY, 
                                            segment.Z + scale.z / 2);
            segmentObject.transform.localPosition = position;

            if (OnSegmentObjectCreated != null) {
                OnSegmentObjectCreated(segmentObject, planogram, segment);
            }
        }
    }

    private static void SpawnBack(GameObject shelfAccentPrefab, Material backMaterial, 
                          PSA psa, PSA.Planogram planogram, GameObject parentObject, string materialsFolderParentPath = "")
    {
        string drawBackString = planogram.Fields["Draw back"];
        bool drawShelfBack = drawBackString.Equals("1") || drawBackString.Equals("true");
        // Only spawn the shelf back if the planogram says it should be shown
        if (drawShelfBack)
        {
            GameObject back = GameObject.Instantiate(shelfAccentPrefab);
            back.transform.SetParent(parentObject.transform);

            // Get the depth of the planogram back
            float backDepth = 0.1f;
            try
            {
                string backDepthStr = planogram.Fields["Back depth"];
                backDepth = float.Parse(backDepthStr);
            }
            catch
            {
                backDepth = 0.1f;
            }

            // Make sure the back depth is greater than 0
            backDepth = backDepth > 0 ? backDepth : 0.1f;

            back.transform.localPosition = new Vector3(planogram.Width / 2, planogram.Height / 2, planogram.Depth + backDepth / 2);// furthestBackProduct + backDepth / 2);
            back.transform.localScale = new Vector3(planogram.Width, planogram.Height, backDepth);

            // Apply the back material to the planogram back object
            Renderer backRenderer = back.GetComponentInChildren<Renderer>();
            Material ourMaterial;
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
            // Find the shelf back material for this strategy if it already exists, or create a new one if it doesn't
            string backMaterialPath = materialsFolderParentPath + "/" + ApplyEditorMaterialsToProducts.MaterialsFolderName + "/" + PsaManager.Instance.LoadedPSA.TheProject.Name + "_" + planogram.Name + "_Back.mat";
            backRenderer.sharedMaterial = ApplyEditorMaterialsToProducts.FindOrCreateMaterial(backMaterial, backMaterialPath);
            ourMaterial = backRenderer.sharedMaterial;

            // Find the shelf back texture if it exists
            Texture2D backTexture = Resources.Load("T_Back") as Texture2D;
            if (backTexture != null)
            {
                // Apply the shelf back texture to the shelf material
                ourMaterial.mainTexture = backTexture;
            }
#else
            backRenderer.material = backMaterial;
            ourMaterial = backRenderer.material;
#endif

            // Figure out how long in Unity units the back texture should be to make 4-foot segments
            float segmentLength = 4 * 12; // inches
            // If the units are in metric instead
            if (psa.TheProject.Measurement == PSA.Project.MeasurementSystem.Metric)
            {
                //Note that segments when in metric are usually either 1 meter or 1.25 meters

                //Get the number of Segments in order to figure out whether 1m or 1.25m shelves are in use
                int segmentsCount = planogram.Segments.Count;

                //Find what the length of the back would be if the segments were each 1meter or 1.25meters 
                float backLengthIfSegments1m = segmentsCount * 100;
                float backLengthIfSegments125cm = segmentsCount * 125;

                //Find the differences between the estimated lengths and the true length of the back
                float diff1m = Mathf.Abs(back.transform.localScale.x - backLengthIfSegments1m);
                float diff125cm = Mathf.Abs(back.transform.localScale.x - backLengthIfSegments125cm);

                // Determine which segment length looks like it is used with this planogram
                // so that that segment length can be used for properly tiling the shelf back material
                segmentLength = diff1m < diff125cm ? 100 : 125;
            }

            // Find how many copies of the backing texture are needed to make 4-foot sections across the planogram
            float tilingX = Mathf.Ceil(back.transform.localScale.x / segmentLength);
            tilingX = tilingX > 0 ? tilingX : 1;

            // Find the matching tiling for the y dimension
            float tilingY = (ourMaterial.mainTexture.width * back.transform.localScale.y) / (ourMaterial.mainTexture.height * segmentLength);
            tilingY = tilingY > 0 ? tilingY : 1;

            // Make a vector with the desired scaling for each dimension
            Vector3 tiling = new Vector3(tilingX, tilingY);
            // Update the material's main texture scale with the new tiling
            ourMaterial.SetTextureScale("_MainTex", tiling);

            // Get the color of the back
            Color backColor = new Color(planogram.Color.R / 255.0f,
                                        planogram.Color.G / 255.0f,
                                        planogram.Color.B / 255.0f);

            // Apply the color of the back to the renderer
            ourMaterial.SetColor("_Color", backColor);

            if (OnShelfBackObjectCreated != null) {
                OnShelfBackObjectCreated(back, planogram);
            }
        }
    }

    private static void SpawnPlanogramBase(GameObject shelfAccentPrefab, Material baseMaterial, 
                                           PSA.Planogram planogram, GameObject parentObject, string materialsFolderParentPath = "")
    {
        string drawBaseString = planogram.Fields["Draw base"];
        bool drawShelfBase = drawBaseString.Equals("1") || drawBaseString.Equals("true");
        // Only spawn the shelf base if the planogram says it should be shown
        if (drawShelfBase)
        {
            GameObject planogramBase = GameObject.Instantiate(shelfAccentPrefab);

            planogramBase.transform.SetParent(parentObject.transform);
            planogramBase.tag = "ShelfBase";
            // Get the planogram base width (default to the width of the planogram)
            float baseWidth = planogram.Width;
            try
            {
                string baseWidthStr = planogram.Fields["Base width"];
                baseWidth = float.Parse(baseWidthStr);
            }
            catch
            {
                Debug.Log("Unable to parse base width.");
            }

            // Get the planogram base height (default to some arbitrary number)
            float baseHeight = 0.1f;
            try
            {
                string baseHeightStr = planogram.Fields["Base height"];
                baseHeight = float.Parse(baseHeightStr);
            }
            catch
            {
                Debug.Log("Unable to parse base height.");
            }

            // Get the planogram base depth (default to the depth of the planogram)
            float baseDepth = planogram.Depth;
            try
            {
                string baseDepthStr = planogram.Fields["Base depth"];
                baseDepth = float.Parse(baseDepthStr);
            }
            catch
            {
                Debug.Log("Unable to parse base depth.");
            }
            
            planogramBase.transform.localScale = new Vector3(baseWidth, baseHeight, baseDepth);
            planogramBase.transform.localPosition = new Vector3(planogram.Width / 2, baseHeight / 2, planogram.Depth - baseDepth / 2);

            // Get the color of the base
            // This way of getting the base color is copied from Jda's way of getting the planogram color
            Color baseColor = Color.white;
            long c = long.Parse(planogram.Fields["Base color"]);
            PfaColor basePfaColor = new PfaColor(c);
            baseColor = new Color(basePfaColor.R / 255.0f,
                                    basePfaColor.G / 255.0f,
                                    basePfaColor.B / 255.0f);

            // Apply the texture of the base to the renderer
            Renderer baseRenderer = planogramBase.GetComponentInChildren<Renderer>();
            Material ourMaterial;
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
            // Find the shelf base material for this strategy if it already exists, or create a new one if it doesn't
            string baseMaterialPath = materialsFolderParentPath + "/" + ApplyEditorMaterialsToProducts.MaterialsFolderName + "/" + PsaManager.Instance.LoadedPSA.TheProject.Name + "_" + planogram.Name + "_Base.mat";
            baseRenderer.sharedMaterial = ApplyEditorMaterialsToProducts.FindOrCreateMaterial(baseMaterial, baseMaterialPath);
            ourMaterial = baseRenderer.sharedMaterial;

            // Find the shelf base texture if it exists
            Texture2D baseTexture = Resources.Load("T_Base") as Texture2D;
            if (baseTexture != null)
            {
                // Apply the shelf base texture to the shelf material
                ourMaterial.mainTexture = baseTexture;
            }
#else
            baseRenderer.material = baseMaterial;
            ourMaterial = baseRenderer.material;
#endif
            ourMaterial.SetColor("_Color", baseColor);

            if (OnShelfBaseObjectCreated != null) {
                OnShelfBaseObjectCreated(planogramBase, planogram);
            }
        }
    }
    public static void UpdateProgressBar()
    {
        if (OnProgressChange != null)
        {
            OnProgressChange(CurrentProgress, StatusMessage, CurrentShelfBuildIndex, TotalShelfCount);
        }
    }
    public static void IncrementProgressBar(float increment, string message = null)
    {
        CurrentProgress += increment;

        if (message != null)
        {
            StatusMessage = message;
        }

        UpdateProgressBar();
    }

    public static bool BuildComplete()
    {
        return !BuildInProgress;
    }
}
