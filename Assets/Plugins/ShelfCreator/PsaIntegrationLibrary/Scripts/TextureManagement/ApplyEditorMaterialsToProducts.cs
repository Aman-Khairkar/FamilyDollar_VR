using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class ApplyEditorMaterialsToProducts : MonoBehaviour
{
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
    private static ApplyEditorMaterialsToProducts instance;
    public static ApplyEditorMaterialsToProducts Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ApplyEditorMaterialsToProducts>();
            }
            return instance;
        }
    }

    static string materialsFolderName = "Materials";
    public static string MaterialsFolderName
    {
        get
        {
            return materialsFolderName;
        }
    }

    public void ApplyProductMaterials(Renderer renderer, Material[] newMaterials)
    {
        renderer.materials = newMaterials;
    }

    public void ApplyPosmMaterials(Renderer renderer, Material[] newMaterials)
    {
        renderer.materials = newMaterials;
    }

    public static Material FindOrCreateMaterial(Material templateMaterial, string materialPath)
    {
        // First check whether this material is already there
        Material material = AssetDatabase.LoadAssetAtPath(materialPath, Type.GetType("Material")) as Material;

        // Otherwise, if the material didn't already exist
        if (material == null)
        {
            // Create the material based on the template
            material = new Material(templateMaterial);

            // Get the name of the folder we want to put the material in
            string materialFolder = "";
            // Find the last occurrence of the '/' character
            for (int i = materialPath.Length - 1; i >= 0; i--)
            {
                if (materialPath[i] == '/')
                {
                    // Keep everything before the '/'
                    materialFolder = materialPath.Substring(0, i);
                    break;
                }
            }

            // Make sure the desired folder exists
            PSAToText.CreateFoldersForPath(materialFolder);

            // Store the material in the assets folder
            AssetDatabase.CreateAsset(material, materialPath);

            // Refresh the changes to the asset database
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        return material;
    }

    public static Material[] CreateProductMaterials(string productUPC, string productID, string shelfSaveFolder, Renderer renderer)
    {
        Material[] sixSidesMaterials = new Material[renderer.sharedMaterials.Length];

        // Now we want to apply the texture to the correct product face.
        // For each face of the cube:
        for (int f = 0; f < 6; f++)
        {
            Material material = renderer.sharedMaterials[ApplyImagesToProducts.FaceMaterialIndices[f]];

            string dotNumber = "." + ApplyImagesToProducts.FaceTextureNumbers[f];

            //Debug.Log("Checking for material \"" + productUPC + dotNumber + ".mat\"");
            Material existingMat = AssetDatabase.LoadAssetAtPath(shelfSaveFolder + "/" + MaterialsFolderName + "/" + productUPC + dotNumber + ".mat", material.GetType()) as Material;
            if (existingMat != null)
            {
                //Debug.Log("Material already exists!");
                material = existingMat;
            }
            else
            {
                //Debug.Log("Trying \"" + productUPC + dotNumber + "\"");
                // Load the image into a texture (try looking for the UPC first
                Texture2D loadedTexture = Resources.Load(productUPC + dotNumber) as Texture2D;

                // If there wasn't an image corresponding to the UPC
                if (loadedTexture == null)
                {
                    //Debug.Log("Didn't work, trying \"" + productID + dotNumber + "\" instead");
                    // Try the ID instead
                    loadedTexture = Resources.Load(productID + dotNumber) as Texture2D;
                }

                // If there wasn't an image corresponding to the UPC or the ID
                if (loadedTexture == null)
                {
                    // Try UPC/UPC instead
                    loadedTexture = Resources.Load(productUPC + "/" + productUPC + dotNumber) as Texture2D;
                }

                if (loadedTexture != null                                               // If this current texture exists
                    && ApplyImagesToProducts.FaceMaterialIndices[f] < renderer.sharedMaterials.Length)              // and if there is a material corresponding to this face
                {
                    material = CreateMaterial(productUPC + dotNumber, shelfSaveFolder, loadedTexture, renderer.sharedMaterials[ApplyImagesToProducts.FaceMaterialIndices[f]]);

                    material.color = Color.white;
                }

            }

            sixSidesMaterials[ApplyImagesToProducts.FaceMaterialIndices[f]] = material;
        }

        return sixSidesMaterials;
    }

    public static Material[] CreatePosmMaterials(string imageName, string shelfSaveFolder, Renderer renderer)
    {
        Material[] sixSidesMaterials = new Material[renderer.sharedMaterials.Length];

        // Now we want to apply the texture to all faces.
        // For each material that we have:
        for (int i = 0; i < sixSidesMaterials.Length; i++)
        {
            Material material = renderer.sharedMaterials[i];

            //Debug.Log("Checking for material \"" + imageName + ".mat\"");
            Material existingMat = AssetDatabase.LoadAssetAtPath(shelfSaveFolder + "/" + MaterialsFolderName + "/" + imageName + ".mat", material.GetType()) as Material;
            if (existingMat != null)
            {
                //Debug.Log("Material already exists!");
                material = existingMat;
            }
            else
            {
                //Debug.Log("Trying \"" + imageName + "\"");
                // Load the image into a texture (try looking for the UPC first
                Texture2D loadedTexture = Resources.Load(imageName) as Texture2D;

                if (loadedTexture != null)                                               // If this current texture exists
                {
                    material = CreateMaterial(imageName, shelfSaveFolder, loadedTexture, renderer.sharedMaterials[i]);
                }
            }

            sixSidesMaterials[i] = material;
        }

        return sixSidesMaterials;
    }

    static Material CreateMaterial(string newMaterialName, string shelfSaveFolder, Texture2D texture, Material materialTemplate)
    {
        // Create a new material based on the template
        Material material = new Material(materialTemplate);

        // Make sure the directory we want to save in exists
        string saveFolder = shelfSaveFolder + "/" + MaterialsFolderName;
        PSAToText.CreateFoldersForPath(saveFolder);

        // Store the material in the assets folder
        AssetDatabase.CreateAsset(material, saveFolder + "/" + newMaterialName + ".mat");

        // Assign the material's texture
        material.mainTexture = texture;

        return material;
    }

    public static GameObject Load3DModel(string productUPC)
    {
        // Check for a file named with the UPC
        GameObject modelPrefab = Resources.Load(productUPC, typeof(GameObject)) as GameObject;

        // If that didn't work, then try looking to see if it's in a folder named after its UPC
        if (modelPrefab == null)
        {
            modelPrefab = Resources.Load(productUPC + "/" + productUPC, typeof(GameObject)) as GameObject;
        }

        return modelPrefab;
    }

    private static bool IsDotNumber(string extension)
    {
        return extension.Length == 2 && (extension[0] == '.' && Char.IsNumber(extension[1]));
    }
#endif
}
