using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductTiling : MonoBehaviour {
    // The indices of the materials to which to apply each of the textures (listed in the same order as faceTextureNumbers)
    public static int[] FaceMaterialIndices = { 0, 2, 5, 1, 4, 3 };

    public static void SetTilingForHFacings(Renderer renderer, int numFacings)
    {
        // Get the indices of the relevant sides for which we need to change the tiling to match the new number of facings
        int[] relevantSideIndices = {
            FaceMaterialIndices [0],    // Front
            FaceMaterialIndices [2],     // Top
            FaceMaterialIndices [3],     // Back
            FaceMaterialIndices [5]        // Bottom
        };
        // The new number of facings is the horizontal axis for all affected sides
        Vector2 tiling = new Vector2(numFacings, 1);
        // Loop through all relevant sides to change their texture scaling
        for (int s = 0; s < relevantSideIndices.Length; s++)
        {
            // Get the material of the face we're looking at
            Material currentFace;
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
            currentFace = renderer.sharedMaterials[relevantSideIndices[s]];
#else
            currentFace = renderer.materials[relevantSideIndices[s]];
#endif
            // Grab the y value of the current scale
            tiling.y = currentFace.GetTextureScale("_MainTex").y;
            // Update the material's main texture scale with the new tiling
            currentFace.SetTextureScale("_MainTex", tiling);
        }
    }

    public static void SetTilingForVFacings(Renderer renderer, int numFacings)
    {
        // Get the indices of the relevant sides for which we need to change the tiling to match the new number of facings
        int[] relevantSideIndices = {
            FaceMaterialIndices [0],    // Front
            FaceMaterialIndices [1],     // Left
            FaceMaterialIndices [3],     // Back
            FaceMaterialIndices [4]        // Right
        };
        // The new number of facings is the vertical axis for all affected sides
        Vector2 tiling = new Vector2(1, numFacings);
        // Loop through all relevant sides to change their texture scaling
        for (int s = 0; s < relevantSideIndices.Length; s++)
        {
            // Get the material of the face we're looking at
            Material currentFace;
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
            currentFace = renderer.sharedMaterials[relevantSideIndices[s]];
#else
            currentFace = renderer.materials[relevantSideIndices[s]];
#endif
            // Grab the x value of the current scale
            tiling.x = currentFace.GetTextureScale("_MainTex").x;
            // Update the material's main texture scale with the new tiling
            currentFace.SetTextureScale("_MainTex", tiling);
        }
    }

    public static void SetTilingForAllFacings(Renderer renderer, int numHFacings, int numVFacings, int numDFacings)
    {
        Vector2[] faceTilings = 
            {
                new Vector2(numHFacings, numVFacings),  // Front
                new Vector2(numDFacings, numVFacings),  // Left
                new Vector2(numHFacings, numDFacings),  // Top
                new Vector2(numHFacings, numVFacings),  // Back
                new Vector2(numDFacings, numVFacings),  // Right
                new Vector2(numHFacings, numDFacings)   // Bottom
            };

        // Apply UV scaling to each face to reflect the number of facings
        for (int i = 0; i < FaceMaterialIndices.Length; i++)
        {
            Material currentMaterial;
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
            currentMaterial = renderer.sharedMaterials[ApplyImagesToProducts.FaceMaterialIndices[i]];
#else
            currentMaterial = renderer.materials[ApplyImagesToProducts.FaceMaterialIndices[i]];
#endif
            currentMaterial.SetTextureScale("_MainTex", faceTilings[i]);
        }
    }
}
