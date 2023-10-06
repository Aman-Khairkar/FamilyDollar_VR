using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JDA;
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
using UnityEditor;
#endif

public static class PsaProductPostionBuilder
{

    public static GameObject SpawnPositionGameObject(PSA.Planogram planogram, PSA.Product psaProduct, PSA.Position psaPosition,
                                                     int planoId, string productKey, int positionIndex,
                                                     GameObject planoObject, GameObject productPositionPrefab)
    {
        GameObject spawnedGameObject = GameObject.Instantiate(productPositionPrefab, planoObject.transform);
        spawnedGameObject.name = planoId + "_" + productKey + "_" + positionIndex;
        /*
        * Note that PSA files keep an object's bottom-left corner as its position 
        * while Unity keeps the object's center as its position, but we don't have 
        * to account for that because it's already taken care of in the prefab.
        */
        spawnedGameObject.transform.localPosition = new Vector3(psaPosition.X,
                                                                psaPosition.Y,
                                                                planogram.Depth - psaPosition.Z);
        spawnedGameObject.transform.rotation = Quaternion.identity;
        spawnedGameObject.transform.localScale = Vector3.one;

        //Here we change the tag of the mesh for this object so it doesn't get confused with the mesh for the individual products.
        spawnedGameObject.transform.GetChild(0).tag = "PositionRenderer";

        //Setup facings data
        var productInformation = spawnedGameObject.AddComponent<ProductInformation>();
        productInformation.DFacings = psaPosition.DFacings;
        productInformation.HFacings = psaPosition.HFacings;
        productInformation.VFacings = psaPosition.VFacings;

        return spawnedGameObject;
    }

    public static void BuildInSinglePosition(PSA.Product psaProduct, PSA.Position psaPosition, GameObject productPositionObject)
    {
        // Make the game object the size of the position
        productPositionObject.transform.localScale = new Vector3(psaPosition.Width, psaPosition.Height, psaPosition.Depth);
        PrepareTextureUvs(productPositionObject, psaProduct, psaPosition, true);
    }

    public static GameObject BuildIndividualProductsPerPosition(PSA.Planogram psaPlanogram, PSA.Product psaProduct, PSA.Position psaPosition,
                                                                int planoId, int positionIndex, string productKey,
                                                                GameObject productPositionObject, GameObject planoObject, GameObject productPositionPrefab,
                                                                bool combineMeshes, Mesh simplifiedBoxMesh = null, string materialsFolderParentPath = "")
    {
        ResizeGeometryDisableRenderer(productPositionObject, psaPosition, psaProduct);

        GameObject original = SpawnSharedMaterialObject(psaProduct, productPositionObject, productPositionPrefab);

        PrepareTextureUvs(original, psaProduct, psaPosition, false);

        Material[] sixSidesMaterials;
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
        sixSidesMaterials = ApplyEditorMaterialsToProducts.CreateProductMaterials(psaProduct.UPC, psaProduct.ID, materialsFolderParentPath, original.GetComponentInChildren<Renderer>());
#else
        sixSidesMaterials = original.GetComponentInChildren<Renderer>().materials;
#endif



        int orientation;
        try
        {
            orientation = int.Parse(psaPosition.Fields["Orientation"]);
        }
        catch (Exception)
        {
            orientation = 0;
        }

        // Get the global dimensions of a product based on its orientation
        Vector3 productScale = new Vector3(GetIndividualRealWidth(psaProduct, psaPosition.Orientation),
                                        GetIndividualRealHeight(psaProduct, psaPosition.Orientation),
                                        GetIndividualRealDepth(psaProduct, psaPosition.Orientation));

        Vector3 ourProductRotations = GetTransformValueByOrientation(orientation).ObjectRotation;

        bool is3D = false;
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
        // Load the 3d model for this product if it has one
        GameObject product3dModel = ApplyEditorMaterialsToProducts.Load3DModel(psaProduct.UPC);
        if (product3dModel != null)
        {
            is3D = true;

            SwapOutMeshForModel(original, product3dModel, ourProductRotations, orientation);
        }

        // If this isn't a 3D product, and we have a simplified box mesh we could use
        if (!is3D && simplifiedBoxMesh != null)
        {
            // Figure out whether we want to use the simplified mesh
            bool useSimplifiedMesh = true;
            // Check each of the materials
            for (int m = 0; m < sixSidesMaterials.Length; m++)
            {
                // If there is a material besides the front face material that has a texture attached to it
                if (m != ApplyImagesToProducts.FaceMaterialIndices[0]
                    && sixSidesMaterials[m] != null
                    && sixSidesMaterials[m].mainTexture != null)
                {
                    // Then we shouldn't use the simplified mesh
                    useSimplifiedMesh = false;
                }
            }

            // If we determined that we should use the simplified mesh for this product
            if (useSimplifiedMesh)
            {
                // Swap out the normal mesh for the simplified one
                SwapMesh(original, simplifiedBoxMesh);

                // Reduce the number of materials in our array to match the number of submeshes in the mesh
                sixSidesMaterials = new Material[] { sixSidesMaterials[0], sixSidesMaterials[1] };
            }
        }
#endif

        // Find the gap between products on the x axis so we can give them the proper amount of space
        float gapBetweenProductsX = CalculateXGapBetweenProducts(psaProduct, psaPosition);

        // Find how far in each direction caps will push the normal products
        Vector3 capOffset = new Vector3(0, 0, 0);
        {
            float totalNormalProductsDepth = productScale.z * psaPosition.DFacings;

            // Figure out how far in the x direction the x caps will push the normal products
            if (psaPosition.XCapReversed == 1)
            {
                capOffset.x = (GetIndividualRealWidth(psaProduct, psaPosition.XCapOrientation) + gapBetweenProductsX) * psaPosition.XCapNum;
            }

            // Account for the fact that the x caps may end up further back than the normal products
            if (psaPosition.XCapNum > 0)
            {
                float xCapDepth = GetIndividualRealDepth(psaProduct, psaPosition.XCapOrientation);
                float totalXCapsDepth = xCapDepth * NumberOfCapsToFillSpace(xCapDepth, productScale.z, psaPosition.DFacings);
                capOffset.z = Math.Min(capOffset.z,
                                    totalNormalProductsDepth - totalXCapsDepth);
            }

            // Figure out how far in the y direction the y caps will push the normal products
            if (psaPosition.YCapReversed == 1)
            {
                capOffset.y = GetIndividualRealHeight(psaProduct, psaPosition.YCapOrientation) * psaPosition.YCapNum;
            }

            // Account for the fact that the y caps may end up further back than the normal products
            if (psaPosition.YCapNum > 0)
            {
                float yCapDepth = GetIndividualRealDepth(psaProduct, psaPosition.YCapOrientation);
                float totalYCapsDepth = yCapDepth * NumberOfCapsToFillSpace(yCapDepth, productScale.z, psaPosition.DFacings);
                capOffset.z = Math.Min(capOffset.z,
                                    totalNormalProductsDepth - totalYCapsDepth);
            }

            // Figure out how far in the z direction the z caps will push the normal products
            if (psaPosition.ZCapReversed == 1)
            {
                capOffset.z -= GetIndividualRealDepth(psaProduct, psaPosition.ZCapOrientation) * psaPosition.ZCapNum;
            }
        }

        /*
         * Spawn the normal facings
         */

        // Create one depth-row so that we can easily duplicate it
        GameObject originalDepthRow = SpawnOneProductDepthRow(psaProduct, psaPosition, original, productPositionObject,
                                                                productScale, ourProductRotations, sixSidesMaterials,
                                                                combineMeshes, is3D);

        // For each facing this position is tall
        for (int v = 0; v < psaPosition.VFacings; v++)
        {
            // For each facing this position is wide
            for (int h = 0; h < psaPosition.HFacings; h++)
            {
                // Create a new duplicate of the original depth-row
                GameObject newDepthRow = GameObject.Instantiate(originalDepthRow, Vector3.zero, Quaternion.identity, productPositionObject.transform);

                // Calculate where this depth-row should be placed at
                Vector3 depthRowPosition = new Vector3(capOffset.x + (productScale.x + gapBetweenProductsX) * h,
                                                       capOffset.y + productScale.y * v,
                                                       capOffset.z);

                // Place the depth-row in the correct spot
                newDepthRow.transform.localPosition = depthRowPosition;
            }
        }

        // Destroy the template depthRow since we don't need it anymore
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
        GameObject.DestroyImmediate(originalDepthRow);
#else
        GameObject.Destroy(originalDepthRow);
#endif

        /*
         * Spawn capped products
         */

        float productFrontZ = -productScale.z * psaPosition.DFacings + capOffset.z;

        // Spawn x caps
        if (psaPosition.XCapNum > 0)
        {
            // Position for reversed X Caps (left of normal products)
            Vector3 xCapBasePosition = new Vector3(0, capOffset.y, productFrontZ);
            // Position change for non-reversed X Caps (right of normal products)
            Vector3 xCapNonReversedPositionModifier = new Vector3(capOffset.x + (productScale.x + gapBetweenProductsX) * psaPosition.HFacings, 0, 0);
            bool xCapsReversed = (psaPosition.XCapReversed == 1);

            // Spawn the set of X caps
            SpawnCapSet(psaProduct, psaPosition, original, productPositionObject,
                        xCapBasePosition, xCapNonReversedPositionModifier, sixSidesMaterials,
                        gapBetweenProductsX, psaPosition.XCapOrientation,
                        (int)psaPosition.XCapNum, -1, -1, xCapsReversed, is3D);
        }

        // Spawn y caps
        if (psaPosition.YCapNum > 0)
        {
            // Position for reversed Y Caps (under normal products)
            Vector3 yCapBasePosition = new Vector3(0, 0, productFrontZ);
            // Position change for non-reversed Y Caps (on top of normal products)
            Vector3 yCapNonReversedPositionModifier = new Vector3(0, capOffset.y + (productScale.y * psaPosition.VFacings), 0);
            bool yCapsReversed = (psaPosition.YCapReversed == 1);

            // Spawn the set of Y caps
            SpawnCapSet(psaProduct, psaPosition, original, productPositionObject,
                        yCapBasePosition, yCapNonReversedPositionModifier, sixSidesMaterials,
                        gapBetweenProductsX, psaPosition.YCapOrientation,
                        -1, (int)psaPosition.YCapNum, -1, yCapsReversed, is3D);
        }

        // Spawn z caps
        if (psaPosition.ZCapNum > 0)
        {
            // Position for reversed Z Caps (behind normal products)
            Vector3 zCapBasePosition = new Vector3(0, capOffset.y, -GetIndividualRealDepth(psaProduct, psaPosition.ZCapOrientation) * psaPosition.ZCapNum);
            // Position change for non-reversed Z Caps (in front of normal products)
            Vector3 zCapNonReversedPositionModifier = new Vector3(0, 0, productFrontZ);
            bool zCapsReversed = (psaPosition.ZCapReversed == 1);

            // Spawn the set of Z caps
            SpawnCapSet(psaProduct, psaPosition, original, productPositionObject,
                        zCapBasePosition, zCapNonReversedPositionModifier, sixSidesMaterials,
                        gapBetweenProductsX, psaPosition.ZCapOrientation,
                        -1, -1, (int)psaPosition.ZCapNum, zCapsReversed, is3D);
        }

        // Deactivating the original, we only keep it because all of the product copies reference the same materials inside the original object
        original.SetActive(false);

        return productPositionObject;
    }

    private static GameObject SwapOutMeshForModel(GameObject original, GameObject product3dModel, Vector3 productRotation, int orientation)
    {
        // Spawn the 3d model
        GameObject model;
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
        model = PrefabUtility.InstantiatePrefab(product3dModel) as GameObject;
#else
        model = GameObject.Instantiate(product3dModel);
#endif
        // get mesh position based on product orientation.
        Vector3 meshPosition = GetTransformValueByOrientation(orientation).MeshPosition;

        model.transform.SetParent(original.transform);
        model.transform.localEulerAngles = productRotation + new Vector3(0, 180f, 0); // Effectively "un-rotate" the product, because the model is likely forward-facing
        // Change the local position of mesh based on product orientation
        model.transform.localPosition = meshPosition;
        model.name = "Mesh";

        // Models may not be the right size, so we need to correct for this
        Vector3 modelBoundsScale = Vector3.one;
        Vector3 boxBoundsScale = Vector3.one;

        MeshFilter modelMeshFilter = model.GetComponent<MeshFilter>();
        if (modelMeshFilter != null)
        {
            // Get the size of the model
            modelBoundsScale = modelMeshFilter.sharedMesh.bounds.size;
        }

        Transform boxMeshTransform = original.transform.Find("Mesh");
        MeshFilter boxMeshFilter = boxMeshTransform.GetComponent<MeshFilter>();
        if (boxMeshFilter != null)
        {
            // Get the size of the model
            boxBoundsScale = boxMeshFilter.sharedMesh.bounds.size;
        }

        // If we divide by its current scale and multiply by the scale we want it to be,
        //   then the product should turn out to be the right size.
        model.transform.localScale = new Vector3(boxBoundsScale.x / modelBoundsScale.x,
                                                 boxBoundsScale.y / modelBoundsScale.y,
                                                 boxBoundsScale.z / modelBoundsScale.z);

        /* Hold on to this in case we change our mind and need it (Commented out to enforce uniform model creation 
         * (only +y facing bottom left front pivot models)) 
        //Temporary Solution to 3D model assets facing the negative x direction rather than the positive y direction in 3ds Max
        //and having their pivot point at bottom center instead of corner
        BoxCollider modelBox = model.AddComponent<BoxCollider>();

        //used to determine the pivot point of the model (center does not necessarily represent the pivot point but can reveal it)
        Vector3 colliderCenter = modelBox.center;

        //if the center (in terms of x and z) is approximately 0
        if(colliderCenter.x < .0001f && colliderCenter.x > -.0001f && colliderCenter.z < .0001f && colliderCenter.z > -.0001f)
        {
            //that means the pivot is located at the center and we should change the positioning of the Mesh and its rotation
            model.transform.localEulerAngles = new Vector3(0, -90f, 0); 
            model.transform.localPosition = new Vector3(0f, -0.5f, 0f);
        }

        //Destroy the model Box Collider now that we've made sure any corrections needed for the model position and rotation are made
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
        GameObject.DestroyImmediate(modelBox);
#else
        GameObject.Destroy(modelBox);
#endif
        //End of Temporary Solution
        */

        // Destroy the box mesh
        if (boxMeshTransform != null)
        {
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
            GameObject.DestroyImmediate(boxMeshTransform.gameObject);
#else
            GameObject.Destroy(boxMeshTransform.gameObject);
#endif
        }

        return model;
    }

    private static void SwapMesh(GameObject original, Mesh newMesh)
    {
        Transform boxMeshTransform = original.transform.Find("Mesh");
        MeshFilter boxMeshFilter = boxMeshTransform.GetComponent<MeshFilter>();
        if (boxMeshFilter != null)
        {
            boxMeshFilter.mesh = newMesh;
        }
    }

    private static GameObject SpawnOneProductDepthRow(PSA.Product psaProduct, PSA.Position psaPosition,
                                                GameObject original, GameObject productPositionObject,
                                                Vector3 productDimensions, Vector3 productRotations,
                                                Material[] sixSidesMaterials, bool combineMeshes, bool is3D)
    {
        // Spawn the products in the depth-row
        List<GameObject> spawnedProductList = new List<GameObject>();
        for (int j = 0; j < psaPosition.DFacings; j++)
        {
            SpawnPrepIndividualProduct(original, productPositionObject, sixSidesMaterials, is3D, spawnedProductList);
        }

        // Correctly position our first depth-row of products
        Vector3 lowerLeft = Vector3.zero;
        for (int j = 0; j < spawnedProductList.Count; j++)
        {
            // Rotate this product to match its orientation
            spawnedProductList[j].transform.Rotate(productRotations);

            // Place the product so the lower-left corner is at (0,0,0) of the rotation parent's local space
            if (j == 0)
            {
                lowerLeft = FindLowerLeft(spawnedProductList[j]);
            }
            GameObject rotationParent = AttachRotationParent(productPositionObject, spawnedProductList[j], lowerLeft);

            if (is3D)
            {
                spawnedProductList[j].transform.localScale = productDimensions;
                spawnedProductList[j].transform.localPosition = new Vector3(-productDimensions.x / 2, productDimensions.y / 2, productDimensions.z / 2);
            }

            // Place the product in the correct place in the position           
            rotationParent.transform.localPosition = new Vector3(0, 0, (-productDimensions.z) * j);
        }

        // Combine the meshes of the products in each depth-row
        GameObject productRowGameObj;

        if (combineMeshes)
        {
            productRowGameObj = CombineMeshes(psaProduct, spawnedProductList, productPositionObject, sixSidesMaterials);

            // Give this object the correct tag and layer
            productRowGameObj.tag = "ProductRenderer";
            productRowGameObj.layer = LayerMask.NameToLayer("Products");

            // Clean up the old objects
            for (int i = 0; i < spawnedProductList.Count; i++)
            {
                // Destroy from the parent so that we don't have a bunch of "RotateParent" objects hanging around
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
                GameObject.DestroyImmediate(spawnedProductList[i].transform.parent.gameObject);
#else
                GameObject.Destroy(spawnedProductList[i].transform.parent.gameObject);
#endif
            }
        }
        else
        {
            productRowGameObj = new GameObject("ProductRow");
            productRowGameObj.transform.parent = productPositionObject.transform;
            productRowGameObj.transform.localScale = Vector3.one;

            // Give this object the correct layer
            productRowGameObj.layer = LayerMask.NameToLayer("Products");

            // Adjust the individual objects
            for (int i = 0; i < spawnedProductList.Count; i++)
            {
                Transform positionObject = spawnedProductList[i].transform;
                Transform rotateParent = positionObject.parent;

                // Give this object the correct layer
                positionObject.gameObject.layer = LayerMask.NameToLayer("Products");

                // Attach the RotateParent to the ProductRow object
                rotateParent.localPosition += productRowGameObj.transform.localPosition;
                rotateParent.parent = productRowGameObj.transform;
                rotateParent.localScale = Vector3.one;

                if (is3D)
                {
                    // Un-rotate the mesh
                    positionObject.localRotation = Quaternion.identity;
                }
            }
        }

        return productRowGameObj;
    }

    /// <summary>
    /// Combines the meshes of the GameObjects in the productsToCombine List, deletes those GameObjects, and attaches the new mesh to a new GameObject. 
    /// Returns the new GameObject with the combined Mesh attached to it.
    /// </summary>
    /// <param name="psaProduct"></param>
    /// <param name="productsToCombine"></param>
    /// <param name="productPositionObject"></param>
    /// <param name="sixSidesMaterials"></param>
    private static GameObject CombineMeshes(PSA.Product psaProduct, List<GameObject> productsToCombine, GameObject productPositionObject, Material[] sixSidesMaterials)
    {
        // Spawn a new empty mesh. This will hold all of the products we're about to combine as submeshes.
        Mesh combinedMesh = new Mesh();

        // Create an array that will contain one submesh per side of the product cube, so that we can apply a single material to each one
        CombineInstance[] subMeshes = new CombineInstance[sixSidesMaterials.Length];

        // So, for each side of the cube
        for (int f = 0; f < sixSidesMaterials.Length; f++)
        {
            // Spawn a new empty mesh. This will hold all instances of face f, which we're about to merge into a single submesh.
            Mesh singleFaceCombinedMesh = new Mesh();

            // Create an array that will contain the meshes of the face we're going to combine in this particular depth-row.
            CombineInstance[] singleFaceSubMeshes = new CombineInstance[productsToCombine.Count];

            // Grab face f from each product in this depth-row and add it to the singleFaceSubMeshes array
            for (int i = 0; i < productsToCombine.Count; i++)
            {
                // Get the mesh filter of this product
                MeshFilter meshFilter = productsToCombine[i].GetComponentInChildren<MeshFilter>();

                // Create a CombineInstance struct that we can use to combine its face f submesh with the others
                CombineInstance combineElement = new CombineInstance
                {
                    mesh = meshFilter.sharedMesh,
                    // Multiply appropriate matrices to transform the meshes into the product parent object's space
                    transform = productPositionObject.transform.worldToLocalMatrix * meshFilter.transform.localToWorldMatrix,
                    // Get the f-th face of the cube to be combined with the others
                    subMeshIndex = f
                };

                // Save the CombineInstance struct in our array
                singleFaceSubMeshes[i] = combineElement;
            }

            // Combine this face from each product into one mesh with a single submesh
            singleFaceCombinedMesh.CombineMeshes(singleFaceSubMeshes, true);

            // Create a CombineInstance struct that we can use to combine this mesh with the others
            CombineInstance singleFaceCombineElement = new CombineInstance
            {
                mesh = singleFaceCombinedMesh,
                transform = Matrix4x4.identity
            };

            // Save this submesh so we can combine it later
            subMeshes[f] = singleFaceCombineElement;
        }

        // Combine the products into one mesh (keeping the face submeshes separate) and give it a name
        combinedMesh.CombineMeshes(subMeshes, false);
        combinedMesh.name = "CombinedMesh" + psaProduct.UPC;

        // Create a new GameObject to hold the mesh
        GameObject newGameObj = GameObject.Instantiate(new GameObject(), productPositionObject.transform);
        newGameObj.name = "ProductRow";
        MeshFilter addedMeshFilter = newGameObj.AddComponent(typeof(MeshFilter)) as MeshFilter;
        MeshRenderer meshRenderer = newGameObj.AddComponent(typeof(MeshRenderer)) as MeshRenderer;

        // Attach our combined mesh to the GameObject
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
        addedMeshFilter.sharedMesh = combinedMesh;
#else
        addedMeshFilter.mesh = combinedMesh;
#endif

        // Grab the materials we need
        meshRenderer.materials = sixSidesMaterials;

        return newGameObj;
    }

    /// <summary>
    /// Spawns one set of caps (either X, Y, or Z caps) for a position.
    /// </summary>
    /// <param name="psaProduct">The information about this product in the PSA, used to find proper scale of caps.</param>
    /// <param name="original">The prefab to base newly-spawned products off of.</param>
    /// <param name="productPositionObject">The GameObject serving as the parent for the caps we're spawning.</param>
    /// <param name="capBasePosition">The point that the front-bottom-left corner of the front-bottom left cap of this type should line up with, assuming caps are reversed.</param>
    /// <param name="nonReversedCapPositionModifier">If the position is not reversed, this will be added to capBasePosition to find the local position of the bottom-left-front cap.</param>
    /// <param name="sixSidesMaterials">The materials to be applied to the spawned products.</param>
    /// <param name="gapBetweenProductsX">The gap between products on the x axis.</param>
    /// <param name="capOrientation">The number representing the orientation of this type of cap.</param>
    /// <param name="xRows">How many products wide this group of caps should be, or -1 to fill the width of the position.</param>
    /// <param name="yRows">How many products tall this group of caps should be, or -1 to fill the height of the position.</param>
    /// <param name="zRows">How many products deep this group of caps should be, or -1 to fill the depth of the position.</param>
    /// <param name="capReversed">Whether this type of cap is reversed.</param>
    private static void SpawnCapSet(PSA.Product psaProduct, PSA.Position psaPosition, GameObject original, GameObject productPositionObject,
                                      Vector3 capBasePosition, Vector3 nonReversedCapPositionModifier,
                                      Material[] sixSidesMaterials, float gapBetweenProductsX, int capOrientation,
                                      int xRows, int yRows, int zRows,
                                      bool capReversed, bool is3D)
    {
        Vector3 normalProductScale = new Vector3(GetIndividualRealWidth(psaProduct, psaPosition.Orientation),
                                                 GetIndividualRealHeight(psaProduct, psaPosition.Orientation),
                                                 GetIndividualRealDepth(psaProduct, psaPosition.Orientation));
        Vector3 capScale = new Vector3(GetIndividualRealWidth(psaProduct, capOrientation),
                                       GetIndividualRealHeight(psaProduct, capOrientation),
                                       GetIndividualRealDepth(psaProduct, capOrientation));
        Vector3 capRotation = GetTransformValueByOrientation(capOrientation).ObjectRotation;

        float gapBetweenCapsX = gapBetweenProductsX;

        // If this cap type is defaulting to fill the width of the position
        if (xRows == -1)
        {
            // If the caps are oriented in a way that they're the same width as the normal products
            if (capScale.x == normalProductScale.x)
            {
                // Then there should be the same number of x rows as there are HFacings
                xRows = psaPosition.HFacings;
            }
            else
            {
                // Calculate how many caps we can fit in this space, and set the cap gap to 0
                xRows = NumberOfCapsToFillSpace(capScale.x, normalProductScale.x, psaPosition.HFacings, gapBetweenProductsX);
                gapBetweenCapsX = 0;
            }
        }

        // If this cap type is defaulting to fill the height of the position
        if (yRows == -1)
        {
            // Calculate how many caps we can fit in this space
            yRows = NumberOfCapsToFillSpace(capScale.y, normalProductScale.y, psaPosition.VFacings);
        }

        // If this cap type is defaulting to fill the depth of the position
        if (zRows == -1)
        {
            // Calculate how many caps we can fit in this space
            zRows = NumberOfCapsToFillSpace(capScale.z, normalProductScale.z, psaPosition.DFacings);
        }

        // Spawn the caps
        for (int xIndex = 0; xIndex < xRows; xIndex++)
        {
            for (int yIndex = 0; yIndex < yRows; yIndex++)
            {
                for (int zIndex = 0; zIndex < zRows; zIndex++)
                {
                    // Spawn an object for this cap and configure it with the correct orientation and place in the hierarchy
                    GameObject spawnedCap = SpawnPrepIndividualProduct(original, productPositionObject, sixSidesMaterials, is3D);
                    spawnedCap.transform.Rotate(capRotation);
                    AttachRotationParent(productPositionObject, spawnedCap, FindLowerLeft(spawnedCap));

                    // Find the correct position for this cap
                    Vector3 capLocalPosition = capBasePosition + new Vector3((capScale.x + gapBetweenCapsX) * xIndex,
                                                                            capScale.y * yIndex,
                                                                            capScale.z * (zIndex + 1));

                    // If the caps aren't reversed
                    if (!capReversed)
                    {
                        // Then modify their position to place them on the other side of the normal products
                        capLocalPosition += nonReversedCapPositionModifier;
                    }

                    // Apply the position
                    spawnedCap.transform.parent.transform.localPosition = capLocalPosition;
                }
            }
        }
    }

    private static int NumberOfCapsToFillSpace(float capDimension, float productDimension, int numProducts, float gapBetweenNormalProducts = 0)
    {
        int numCaps;

        // Calculate how many caps we can fit in this space
        float normalProductsTotalScale = productDimension * numProducts + gapBetweenNormalProducts * (numProducts - 1);
        numCaps = (int)(normalProductsTotalScale / capDimension);

        // Make sure that we allow at least one cap
        numCaps = Math.Max(numCaps, 1);

        return numCaps;
    }

    private static void PrepareTextureUvs(GameObject rendererObject, PSA.Product psaProduct, PSA.Position psaPosition, bool setTiling)
    {
        // Get a reference to the renderer
        Renderer renderer = rendererObject.GetComponentInChildren<Renderer>();

        // Find the product's textures and apply them to the product
        ApplyImagesToProducts.Instance.ApplyProductTextures(renderer, psaPosition.UPC, psaPosition.ID, psaProduct.Fields[PSA.Product.FieldNames.Name]);

        if (setTiling)
        {
            // Apply UV scaling to each face to reflect the number of facings
            ProductTiling.SetTilingForAllFacings(renderer, psaPosition.HFacings, psaPosition.VFacings, psaPosition.DFacings);
        }
    }

    private static Vector3 FindLowerLeft(GameObject productObject)
    {
        MeshFilter mf = productObject.transform.Find("Mesh").GetComponent<MeshFilter>();
        Mesh mesh = null;
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
        if (mf != null)
            mesh = mf.sharedMesh;
#else
        mesh = mf.mesh;
#endif
        Matrix4x4 localToWorld = productObject.transform.localToWorldMatrix;

        List<Vector3> verticeList = new List<Vector3>();

        //should this be good for every single one?
        for (int k = 0; k < mesh.vertices.Length; ++k)
        {
            Vector3 world_v = localToWorld.MultiplyPoint3x4(mesh.vertices[k]);
            verticeList.Add(world_v);
        }

        float chosenX = 999;
        float chosenY = 999;
        float chosenZ = 999;

        for (int k = 0; k < verticeList.Count; ++k)
        {
            if (k == 0)
            {
                chosenX = verticeList[k].x;
                chosenY = verticeList[k].y;
                chosenZ = verticeList[k].z;
            }

            if (verticeList[k].x < chosenX)
            {
                chosenX = verticeList[k].x;
            }

            if (verticeList[k].y < chosenY)
            {
                chosenY = verticeList[k].y;
            }

            if (verticeList[k].z > chosenZ)
            {
                chosenZ = verticeList[k].z;
            }

        }

        return new Vector3(chosenX, chosenY, chosenZ);
    }

    /// <summary>
    /// Creates a new game object from the product position object to be used as a copy source when generating individual product objects
    /// It will also be a shared source for their materials, so all the individual product objects reference this one object for material textures cutting down on memory
    /// This will also zero out the collider's center and renderer positions
    /// The vanilla product position prefab accounts for position origins needing to be in lower left corner, but for rotations, we need it in the center
    /// These individual products will then have a rotation parent added on later
    /// </summary>
    /// <param name="psaProduct">For knowing the product dimensions</param>
    /// <param name="productPositionObject">To set as object's parent</param>
    /// <param name="productPositionPrefab">To generate our new object</param>
    /// <returns>Source gameobject to allowing material sharing and making copy objects</returns>
    private static GameObject SpawnSharedMaterialObject(PSA.Product psaProduct, GameObject productPositionObject, GameObject productPositionPrefab)
    {
        GameObject original = GameObject.Instantiate(productPositionPrefab, productPositionObject.transform);
        original.GetComponent<BoxCollider>().center = Vector3.zero;
        original.GetComponentInChildren<Renderer>().gameObject.transform.localPosition = Vector3.zero;
        original.transform.localScale = new Vector3(psaProduct.Width, psaProduct.Height, psaProduct.Depth);

        return original;
    }
    private static GameObject SpawnPrepIndividualProduct(GameObject original, GameObject parent, Material[] materials, bool is3D, List<GameObject> trackerList = null)
    {
        GameObject spawnedProduct = GameObject.Instantiate(original, parent.transform);
        if (!is3D)
        {
            Renderer spawnedRenderer = spawnedProduct.GetComponentInChildren<Renderer>();
            spawnedRenderer.materials = materials;
        }

        Rigidbody rb = spawnedProduct.GetComponent<Rigidbody>();
        Collider cl = spawnedProduct.GetComponent<Collider>();
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
        GameObject.DestroyImmediate(rb);
        GameObject.DestroyImmediate(cl);
#else
        GameObject.Destroy(rb);
        GameObject.Destroy(cl);
#endif

        if (trackerList != null)
        {
            trackerList.Add(spawnedProduct);
        }

        return spawnedProduct;
    }

    private static GameObject AttachRotationParent(GameObject productPositionObject, GameObject productObject, Vector3 lowerLeft)
    {
        GameObject rotationParent = new GameObject();
        rotationParent.name = "RotateParent";
        rotationParent.transform.SetParent(productPositionObject.transform);
        rotationParent.transform.position = lowerLeft;
        rotationParent.transform.Rotate(0, 180f, 0);
        productObject.transform.SetParent(rotationParent.transform);

        return rotationParent;
    }

    private static void ResizeGeometryDisableRenderer(GameObject productPositionObject, PSA.Position psaPosition, PSA.Product psaProduct)
    {
        Vector3 positionSize = new Vector3(psaPosition.Width, psaPosition.Height, psaPosition.Depth);

        GameObject meshGO = productPositionObject.transform.Find("Mesh").gameObject;

        //JDA appends extra space for the width, this makes our mesh super long, so the extra space is removed
        if (psaPosition.HFacings > 0)
        {
            var xGapBetweenProducts = CalculateXGapBetweenProducts(psaProduct, psaPosition);

            positionSize.x = psaPosition.Width - xGapBetweenProducts;
        }

        //JDA appends extra space for the height on pegboard products, this makes our mesh too tall, so the extra space is removed
        if (psaPosition.VFacings > 0)
        {
            var yGapBetweenProducts = CalculateYGapBetweenProducts(psaProduct, psaPosition);

            positionSize.y = psaPosition.Height - yGapBetweenProducts;
        }

        meshGO.transform.localScale = positionSize;
        meshGO.transform.localPosition = new Vector3(positionSize.x / 2f, positionSize.y / 2f, positionSize.z / -2f);
        meshGO.GetComponent<Renderer>().enabled = false;

        BoxCollider meshCL = productPositionObject.GetComponent<BoxCollider>();
        meshCL.size = positionSize;
        meshCL.center = new Vector3(positionSize.x / 2f, positionSize.y / 2f, positionSize.z / -2f);
    }

    public static float CalculateXGapBetweenProducts(PSA.Product productData, PSA.Position positionData)
    {
        float gapBetweenProductsX = 0;

        // Calculate how much X space individual products and individual caps take up based on their orientation
        float individualWidthMain = GetIndividualRealWidth(productData, positionData.Orientation);
        float individualWidthXCaps = GetIndividualRealWidth(productData, positionData.XCapOrientation);

        // Calculate the total amount of X space that is actually filled by a product
        float totalOccupiedWidth = (individualWidthMain * positionData.HFacings) + (individualWidthXCaps * positionData.XCapNum);

        // The gap between products is the total empty X space in the position divided evenly between the facings
        gapBetweenProductsX = (positionData.Width - totalOccupiedWidth) / (positionData.HFacings + positionData.XCapNum);

        if (gapBetweenProductsX < 0)
            gapBetweenProductsX = 0;

        return gapBetweenProductsX;
    }

    public static float CalculateYGapBetweenProducts(PSA.Product productData, PSA.Position positionData)
    {
        float gapBetweenProductsY = 0;

        // Calculate how much Y space individual products and individual caps take up based on their orientation
        float individualHeightMain = GetIndividualRealHeight(productData, positionData.Orientation);
        float individualHeightYCaps = GetIndividualRealHeight(productData, positionData.YCapOrientation);

        // Calculate the total amount of Y space that is actually filled by a product
        float totalOccupiedHeight = (individualHeightMain * positionData.VFacings) + (individualHeightYCaps * positionData.YCapNum);

        // The gap between products is the total empty Y space in the position divided evenly between the facings
        gapBetweenProductsY = (positionData.Height - totalOccupiedHeight) / (positionData.VFacings + positionData.YCapNum);

        if (gapBetweenProductsY < 0)
            gapBetweenProductsY = 0;

        return gapBetweenProductsY;
    }

    /// <summary>
    /// Figure out how much X space one individual product takes up based on its orientation.
    /// </summary>
    /// <param name="productData">The product whose dimensions we want to use</param>
    /// <param name="orientation">The orientation to use for the calculation</param>
    /// <returns></returns>
    public static float GetIndividualRealWidth(PSA.Product productData, int orientation)
    {
        float absoluteWidth = 0;

        int side = GetSide(orientation) % 3; //0=Front/Back, 1=Side/Right, 2=Top/Base
        bool onSide = GetAngle(orientation) % 180 != 0; // Whether the product is turned on its side

        if (side == 0 && !onSide        // Product's front face dimensions (normal)
            || side == 2 && !onSide)    // Product's top face dimensions (normal)
        {
            absoluteWidth = productData.Width;
        }
        else if (side == 0 && onSide        // Product's front face dimensions (rotated)
                 || side == 1 && onSide)    // Product's side dimensions (rotated)
        {
            absoluteWidth = productData.Height;
        }
        else // Product's top dimensions (rotated), or side dimensions (normal)
        {
            absoluteWidth = productData.Depth;
        }

        return absoluteWidth;
    }

    /// <summary>
    /// Figure out how much Y space one individual product takes up based on its orientation.
    /// </summary>
    /// <param name="productData">The product whose dimensions we want to use</param>
    /// <param name="orientation">The orientation to use for the calculation</param>
    /// <returns></returns>
    public static float GetIndividualRealHeight(PSA.Product productData, int orientation)
    {
        float absoluteHeight = 0;

        int side = GetSide(orientation) % 3; //0=Front/Back, 1=Side/Right, 2=Top/Base
        bool onSide = GetAngle(orientation) % 180 != 0; // Whether the product is turned on its side

        if (side == 0 && !onSide        // Product's front face dimensions (normal)
            || side == 1 && !onSide)    // Product's side face dimensions (normal)
        {
            absoluteHeight = productData.Height;
        }
        else if (side == 0 && onSide        // Product's front face dimensions (rotated)
                 || side == 2 && onSide)    // Product's top dimensions (rotated)
        {
            absoluteHeight = productData.Width;
        }
        else // Product's top dimensions (normal), or side dimensions (rotated)
        {
            absoluteHeight = productData.Depth;
        }

        return absoluteHeight;
    }

    /// <summary>
    /// Figure out how much Z space one individual product takes up based on its orientation.
    /// </summary>
    /// <param name="productData">The product whose dimensions we want to use</param>
    /// <param name="orientation">The orientation to use for the calculation</param>
    /// <returns></returns>
    public static float GetIndividualRealDepth(PSA.Product productData, int orientation)
    {
        float absoluteDepth = 0;
        int side = GetSide(orientation) % 3; //0=Front/Back, 1=Side/Right, 2=Top/Base

        if (side == 0)    // Product's front face dimensions
        {
            absoluteDepth = productData.Depth;
        }
        else if (side == 1)    // Product's side dimensions
        {
            absoluteDepth = productData.Width;
        }
        else // Product's top dimensions
        {
            absoluteDepth = productData.Height;
        }

        return absoluteDepth;
    }

    private static int GetAngle(int productOrientation)
    {
        int angle = 0;
        if (productOrientation / 12 == 0)
        {
            if (productOrientation % 2 == 0)
            {
                angle = 0;
            }
            else
            {
                angle = 90;
            }
        }
        else
        {
            if (productOrientation % 2 == 0)
            {
                angle = 180;
            }
            else
            {
                angle = 270;
            }
        }

        return angle;
    }

    /// <summary>
    /// Figures out which side is facing the front based on the orientation number
    /// </summary>
    /// <param name="productOrientation"></param>
    /// <returns>0=Front, 1=Side, 2=Top, 3=Back, 4=Right, 5=Base</returns>
    private static int GetSide(int productOrientation)
    {
        int side = productOrientation % 12;
        side = side / 2;

        return side;
    }

    /// <summary>
    /// Get position and rotation of Product/Mesh by product orientation. 
    /// </summary>
    /// <param name="productOrientation"></param>
    /// <returns></returns>
    private static TransformValues GetTransformValueByOrientation(int productOrientation)
    {
        TransformValues transformValue = new TransformValues();

        switch (productOrientation)
        {
            case (0):// Front
                transformValue.MeshPosition = new Vector3(0.5f, -0.5f, 0.5f);
                transformValue.ObjectRotation = Vector3.zero;
                break;
            case (1):// Front 90
                transformValue.MeshPosition = new Vector3(0.5f, 0.5f, 0.5f);
                transformValue.ObjectRotation = new Vector3(0f, 0f, -90f);
                break;
            case (2):// Side
                transformValue.MeshPosition = new Vector3(-0.5f, -0.5f, 0.5f);
                transformValue.ObjectRotation = new Vector3(0f, -90f, 0f);
                break;
            case (3):// Side 90
                transformValue.MeshPosition = new Vector3(0.5f, -0.5f, 0.5f);
                transformValue.ObjectRotation = new Vector3(-90f, -90f, 0f);
                break;
            case (4):// Top
                transformValue.MeshPosition = new Vector3(0.5f, -0.5f, -0.5f);
                transformValue.ObjectRotation = new Vector3(-90f, 0f, 0f);
                break;
            case (5):// Top 90
                transformValue.MeshPosition = new Vector3(0.5f, 0.5f, -0.5f);
                transformValue.ObjectRotation = new Vector3(0f, 90f, -90f);
                break;
            case (6):// Back
                transformValue.MeshPosition = new Vector3(-0.5f, -0.5f, -0.5f);
                transformValue.ObjectRotation = new Vector3(0f, 180f, 0f);
                break;
            case (7):// Back 90
                transformValue.MeshPosition = new Vector3(0.5f, -0.5f, -0.5f);
                transformValue.ObjectRotation = new Vector3(0f, 180f, 90f);
                break;
            case (8):// Right
                transformValue.MeshPosition = new Vector3(0.5f, -0.5f, -0.5f);
                transformValue.ObjectRotation = new Vector3(0f, 90f, 0f);
                break;
            case (9):// Right 90
                transformValue.MeshPosition = new Vector3(0.5f, 0.5f, -0.5f);
                transformValue.ObjectRotation = new Vector3(90f, 90f, 0f);
                break;
            case (10):// Base
                transformValue.MeshPosition = new Vector3(0.5f, 0.5f, 0.5f);
                transformValue.ObjectRotation = new Vector3(90f, 0f, 0f);
                break;
            case (11):// Base 90
                transformValue.MeshPosition = new Vector3(-0.5f, 0.5f, 0.5f);
                transformValue.ObjectRotation = new Vector3(0f, -90f, -90f);
                break;
            case (12):// Front 180
                transformValue.MeshPosition = new Vector3(-0.5f, 0.5f, 0.5f);
                transformValue.ObjectRotation = new Vector3(0f, 0f, 180f);
                break;
            case (13):// Front 270
                transformValue.MeshPosition = new Vector3(-0.5f, -0.5f, 0.5f);
                transformValue.ObjectRotation = new Vector3(0f, 0f, 90f);
                break;
            case (14):// Side 180
                transformValue.MeshPosition = new Vector3(0.5f, 0.5f, 0.5f);
                transformValue.ObjectRotation = new Vector3(0f, 90f, 180f);
                break;
            case (15):// Side 270
                transformValue.MeshPosition = new Vector3(-0.5f, 0.5f, 0.5f);
                transformValue.ObjectRotation = new Vector3(90f, 90f, 180f);
                break;
            case (16):// Top 180
                transformValue.MeshPosition = new Vector3(-0.5f, 0.5f, -0.5f);
                transformValue.ObjectRotation = new Vector3(90f, 0f, 180f);
                break;
            case (17):// Top 270
                transformValue.MeshPosition = new Vector3(-0.5f, -0.5f, -0.5f);
                transformValue.ObjectRotation = new Vector3(180f, 90f, 270f);
                break;
            case (18):// Back 180
                transformValue.MeshPosition = new Vector3(0.5f, 0.5f, -0.5f);
                transformValue.ObjectRotation = new Vector3(180f, 0f, 0f);
                break;
            case (19):// Back 270
                transformValue.MeshPosition = new Vector3(-0.5f, 0.5f, -0.5f);
                transformValue.ObjectRotation = new Vector3(180f, 0f, 90f);
                break;
            case (20):// Right 180
                transformValue.MeshPosition = new Vector3(-0.5f, 0.5f, -0.5f);
                transformValue.ObjectRotation = new Vector3(180f, 90f, 0f);
                break;
            case (21):// Right 270
                transformValue.MeshPosition = new Vector3(-0.5f, -0.5f, -0.5f);
                transformValue.ObjectRotation = new Vector3(270f, 90f, 0f);
                break;
            case (22):// Base 180
                transformValue.MeshPosition = new Vector3(-0.5f, -0.5f, 0.5f);
                transformValue.ObjectRotation = new Vector3(270f, 180f, 0f);
                break;
            case (23):// Base 270
                transformValue.MeshPosition = new Vector3(0.5f, -0.5f, 0.5f);
                transformValue.ObjectRotation = new Vector3(180f, 270f, 270f);
                break;
            default:
                transformValue.MeshPosition = new Vector3(0.5f, -0.5f, 0.5f);
                transformValue.ObjectRotation = Vector3.zero;
                break;
        }

        return transformValue;
    }
    private class TransformValues
    {
        public Vector3 MeshPosition;
        public Vector3 ObjectRotation;
    }
}