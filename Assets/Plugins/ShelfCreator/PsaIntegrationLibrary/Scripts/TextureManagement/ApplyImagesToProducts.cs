using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class ApplyImagesToProducts : MonoBehaviour
{
    private static ApplyImagesToProducts instance;
    public static ApplyImagesToProducts Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject instanceObject = PsaManager.Instance.gameObject;
                instance = instanceObject.AddComponent<ApplyImagesToProducts>();
            }
            return instance;
        }
    }

    Texture2D addedProductTexture;

    public List<string> MissingTextures = new List<string>();

    //reference to the textures we're importing
    public List<string> PublicTextureFileLocations = new List<string>();
    public List<Texture2D> CloudTextures = new List<Texture2D>();

    // This will hold three versions of the same list of ProductImageName objects, sorted based on different versions of the name
    // The index of the list in productImageNameLists matches the index of the ImageName that should be requested from ProductImageName
    private List<List<ProductImageName>> productImageNameLists = new List<List<ProductImageName>>
    {
        new List<ProductImageName>(),   // holds all of the ProductImageName objects sorted by their full name
        new List<ProductImageName>(),   // holds all of the ProductImageName objects sorted by their name with the first digit stripped
        new List<ProductImageName>()    // holds all of the ProductImageName objects sorted by their name with the first two digits stripped
    };

    static string pathPreFix = @"file:\\";

    // The numbers ending the texture names corresponding to the front, left, top, back, right, and bottom (in order)
    static public int[] FaceTextureNumbers = { 1, 2, 3, 7, 8, 9 };
    // The indices of the materials to which to apply each of the textures (listed in the same order as faceTextureNumbers)
    static public int[] FaceMaterialIndices = { 0, 5, 1, 2, 4, 3 };

    private class ProductImageName
    {
        // full path to the image file
        public string FileLocation;

        public Texture2D CloudImportedImage;
        // holds the name of the texture, with i digits stripped from the front (where i is an element's index in ImageName[])
        public string[] ImageName;

        public ProductImageName(string fileLocation, string imageName)
        {
            FileLocation = fileLocation;
            ImageName = new string[]
            {
                imageName,                                          // full name
                imageName.Length > 1 ? imageName.Substring(1) : "", // name with the first digit stripped
                imageName.Length > 2 ? imageName.Substring(2) : ""  // name with the first two digits stripped
            };
        }
    }
    
    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
            DestroyImmediate(this);
#else
            Destroy(this);
#endif
        }
    }

    public void ApplyProductTextures(Renderer renderer, string productUPC, string productID, string productName)
    {
        int listIndex = 0;
        int textureIndex = -1;
        string fileNameStart = "";
        bool fileHasDotNumber = true;

        // If this is a product added from Speed Shelf and it has a name
        if (productUPC.StartsWith("SS") && !string.IsNullOrEmpty(productName))
        {
            // get the index of the texture we want to use
            textureIndex = GetTextureIndexByName(productName);
            fileNameStart = productName;

            // If we found a valid texture with the name
            if (textureIndex != -1)
            {
                // Then the image we found isn't going to have a dot number
                fileHasDotNumber = false;
            }
        }

        // If we still haven't found a valid texture
        if (textureIndex == -1)
        {
            // Set up the list of possible keys in the correct priority order
            List<string> possibleKeys = new List<string> { productUPC, productID };
            
            // Also allow the UPC or ID to be stripped of some 0's from the front
            for( int k = 0; k < 2; k++)
            {
                string strippedKey = (k == 0) ? productUPC : productID;

                // While the key still has leading 0's
                while (strippedKey.Length > 0 && strippedKey[0] == '0')
                {
                    // Take a '0' off the front of the key
                    strippedKey = strippedKey.Substring(1);

                    // Add it to our list of possible keys
                    possibleKeys.Add(strippedKey);
                }
            }

            // For each possible key
            for (int k = 0; k < possibleKeys.Count; k++)
            {
                string productKey = possibleKeys[k];

                // Check each image naming format
                for (int i = 0; i < productImageNameLists.Count; i++)
                {
                    // Don't allow for stripping any front digits from UPC/ID numbers that have already had 0's removed from the front
                    if (k >= 2 && i > 0)
                    {
                        continue;
                    }

                    // get the index of the texture we want to use
                    textureIndex = GetTextureIndexByProductUPC(productKey, i);
                    fileNameStart = productKey;
                    
                    // If we found the texture
                    if (textureIndex != -1)
                    {
                        // Note that we found it in the format with a .#
                        fileHasDotNumber = true;

                        // Remember which list we found it in
                        listIndex = i;
                        
                        // Break out of the list
                        break;
                    }
                }
                if (textureIndex != -1)
                {
                    break;
                }
            }
        }

        // If we still didn't find it after all that
        if (textureIndex == -1)
        {
            //Add to our list of missing textures
            if (!MissingTextures.Contains(productUPC))
                MissingTextures.Add(productUPC);
        }

        // Array to keep track of which faces we've already applied textures to
        bool[] appliedTextureToFace = new bool[6];

        while (textureIndex >= 0 && textureIndex < productImageNameLists[listIndex].Count                           // While the textureIndex is still within the bounds of the texture list
                && (fileHasDotNumber && GetProductImageName(textureIndex, listIndex).StartsWith(fileNameStart)  // And we're still looking at textures for this product
                || !fileHasDotNumber && GetProductImageName(textureIndex, listIndex).Equals(fileNameStart)))    // Or we're looking at the only texture for this product
        {
            // Whether we've already applied an image to this face of the product
            bool alreadyApplied = false;

            // If this is an image we expect to have a ".#" at the end of
            if (fileHasDotNumber)
            {
                // Get the image name
                string imageName = GetProductImageName(textureIndex, listIndex);

                // For each face it could be a texture for
                for (int f = 0; f < FaceTextureNumbers.Length; f++)
                {
                    // If it's a texture for this face
                    if (imageName.EndsWith("." + FaceTextureNumbers[f]))
                    {
                        // Remember whether we've already applied an image to this face
                        alreadyApplied = appliedTextureToFace[f];
                        // Either way, since this image matched, this face definitely will have had an image applied after this
                        appliedTextureToFace[f] = true;
                        // Break out of the for loop since we found the matching face
                        break;
                    }
                }
            }

            // Apply the image only if this side doesn't already have an image applied to it
            if (!alreadyApplied)
            {
                if (PsaManager.Instance.IsCloud)
                {
                    ApplyProductImageFromTexture(productImageNameLists[listIndex][textureIndex].CloudImportedImage, renderer, fileHasDotNumber);
                }
                else
                {
                    ApplyProductImage(productImageNameLists[listIndex][textureIndex].FileLocation, renderer, fileHasDotNumber);
                }
            }

            // Move on to look at the next texture.
            textureIndex++;
        }
    }

    /// <summary>
    /// Searches through the <code>publicTextures</code> list for the first instance of a texture whose name matches the format "productUPC.#"
    /// Returns the index in <code>publicTextures</code> of the matching texture if it exists, otherwise returns null.
    /// </summary>
    /// <param name="productUPC">The UPC of the product whose texture is to be found.</param>
    /// <returns>Returns the index in <code>publicTextures</code> of the matching texture if found, -1 otherwise.</returns>
    private int GetTextureIndexByProductUPC(string productUPC, int firstDigitsStripped = 0)
    {
        // Set to true for below debugs relating to matching textures
        bool detailedDebug = false;

        // Do a little prep so we can look at file names instead of full paths
        if ((!PsaManager.Instance.IsCloud && productImageNameLists[firstDigitsStripped].Count != PublicTextureFileLocations.Count) 
            || (PsaManager.Instance.IsCloud && productImageNameLists[firstDigitsStripped].Count != CloudTextures.Count))
        {
            ConvertFileLocationsToFileNames();
        }

        // Create a string that will match the product image name.
        string desiredTextureName = productUPC;

        // The earliest index that the correct texture could possibly be from what we know now.
        int startIndex = 0;
        // The index past the last index that the correct texture could possibly be from what we know now.
        int endIndex = productImageNameLists[firstDigitsStripped].Count;
        // Search through our textures for matching textures.
        while (startIndex < endIndex)
        {
            // Find the index of the next texture to check
            int i = startIndex + (endIndex - startIndex) / 2;
            if (detailedDebug)
            {
                Debug.Log("S:" + startIndex + " I:" + i + " E:" + endIndex);
            }
            // If this texture is one that we want, then grab the index of the first image with this name pattern and return it.
            string currentProductImageName = GetProductImageName(i, firstDigitsStripped);
            if (currentProductImageName.StartsWith(desiredTextureName)              // If this looks like the texture we want
                && currentProductImageName.Length <= desiredTextureName.Length + 3) // And there are only enough characters for up to one stripped end digit, a . and a #
            {
                if (detailedDebug)
                {
                    Debug.Log("We have a match!");
                }
                // Get the index of the first matching image
                int firstMatchingImgIndex = i;
                // While there is an image before image firstMatchingImgIndex and that image is also one we want
                while (firstMatchingImgIndex > 0 
                    && GetProductImageName(firstMatchingImgIndex - 1, firstDigitsStripped).StartsWith(desiredTextureName))
                {
                    // Move firstMatchingImgIndex backwards
                    firstMatchingImgIndex--;
                }
                // Return the index of the first image matching the product UPC
                return firstMatchingImgIndex;
            }
            // Otherwise, if the correct texture(s) would be in the first half of the array
            else if (String.Compare(desiredTextureName, GetProductImageName(i, firstDigitsStripped)) < 0)
            {
                // Cut out the second half of the array by moving endIndex
                endIndex = i;
                if (detailedDebug)
                {
                    Debug.Log("\"" + desiredTextureName + "\" < \"" + GetProductImageName(i, firstDigitsStripped) + "\"");
                }
            }
            // Otherwise, the correct texture(s) would be in the second half of the array
            else
            {
                // Cut out the first half of the array by moving startIndex
                startIndex = i + 1;
                if (detailedDebug)
                {
                    Debug.Log("\"" + desiredTextureName + "\" > \"" + GetProductImageName(i, firstDigitsStripped) + "\"");
                }
            }
        }
        if (detailedDebug)
        {
            Debug.Log("Matching texture not found.");
        }
        // If we didn't find a match, return -1.
        return -1;
    }
    
    private static void ApplyProductImage(string imageFilePath, Renderer renderer, bool hasDotNumber = true)
    {
        Texture2D loadedTexture = null;

        string fileType = Path.GetExtension(imageFilePath);
        Debug.Log("TEMP: " + fileType);
        if (fileType == ".tga")
        {
            // .TGA files need their own loader seperate from normal
            loadedTexture = TGALoader.LoadTGA(imageFilePath);
            Debug.Log(loadedTexture.name.ToString());

        }
        else
        {
            // load in normal image, assuming it is a .PNG
            loadedTexture = LoadPNG(imageFilePath);
        }

        ApplyProductImageFromTexture(loadedTexture, renderer, hasDotNumber);
    }

    private static void ApplyProductImageFromTexture(Texture2D loadedTexture, Renderer renderer, bool hasDotNumber = true)
    {
        // Now we want to apply the texture to the correct product face.
        // For each face of the cube:
        for (int f = 0; f < 6; f++)
        {
            if (loadedTexture != null                                               // If this current texture exists
                && (loadedTexture.name.EndsWith("." + FaceTextureNumbers[f])        // and if it corresponds to the f-th face in the array, 
                        || !hasDotNumber && f == 0)                                     // (or it doesn't have a dot number and this is the front)
                && FaceMaterialIndices[f] < renderer.materials.Length)              // and if there is a material corresponding to this face
            {
                // Assign this texture to the corresponding face of the product
                renderer.materials[FaceMaterialIndices[f]].mainTexture = loadedTexture;

                // Make this side visible
                renderer.materials[FaceMaterialIndices[f]].color = Color.white;
            }
        }
    }
    
    public void ApplyPosmTextures(Renderer renderer, string imageName)
    {
        // get the index of the texture we want to use
        int textureIndex = GetTextureIndexByName(imageName);

        // If the index we got back is a valid index in the PublicTextureFileLocations array
        if (textureIndex >= 0 && textureIndex < PublicTextureFileLocations.Count)
        {
            // Apply the image to all sides of the POSM
            ApplyPosmImage(PublicTextureFileLocations[textureIndex], renderer);
        }
    }

    /// <summary>
    /// Searches through the <code>publicTextures</code> list for the texture whose name matches <code>desiredTextureName</code> exactly.
    /// Returns the index in <code>publicTextures</code> of the matching texture if it exists, otherwise returns null.
    /// </summary>
    /// <param name="desiredTextureName">The name of the texture to be found.</param>
    /// <returns>Returns the index in <code>publicTextures</code> of the matching texture if found, -1 otherwise.</returns>
    private int GetTextureIndexByName(string desiredTextureName)
    {
        int fullNameListIndex = 0;

        // Do a little prep so we can look at file names instead of full paths
        if (productImageNameLists[fullNameListIndex].Count != PublicTextureFileLocations.Count)
        {
            ConvertFileLocationsToFileNames();
        }

        // The earliest index that the correct texture could possibly be from what we know now.
        int startIndex = 0;
        // The index past the last index that the correct texture could possibly be from what we know now.
        int endIndex = productImageNameLists[fullNameListIndex].Count;
        // Search through our textures for matching textures.
        while (startIndex < endIndex)
        {
            // Find the index of the next texture to check
            int i = startIndex + (endIndex - startIndex) / 2;

            //Debug.Log("S:" + startIndex + " I:" + i + " E:" + endIndex);

            // If this texture is the one that we want, then return its index.
            if (GetProductImageName(i, fullNameListIndex).Equals(desiredTextureName))
            {
                //Debug.Log("We have a match!");

                return i;
            }
            // Otherwise, if the correct texture(s) would be in the first half of the array
            else if (String.Compare(desiredTextureName, GetProductImageName(i, fullNameListIndex)) < 0)
            {
                // Cut out the second half of the array by moving endIndex
                endIndex = i;
                //Debug.Log("\"" + desiredTextureName + "\" < \"" + publicTextures[i].name + "\"");
            }
            // Otherwise, the correct texture(s) would be in the second half of the array
            else
            {
                // Cut out the first half of the array by moving startIndex
                startIndex = i + 1;
                //Debug.Log("\"" + desiredTextureName + "\" > \"" + publicTextures[i].name + "\"");
            }
        }

        //Debug.Log("Matching texture not found.");
        // If we didn't find a match, return -1.
        return -1;
    }

    public static void ApplyPosmImage(string imageFilePath, Renderer renderer)
    {
        // Load the image into a texture
        Texture2D loadedTexture = LoadPNG(imageFilePath);

        // Now we want to apply the texture to all faces of the POSM.
        ApplyPosmImage(loadedTexture, renderer);
    }

    public static void ApplyPosmImage(Texture2D loadedTexture, Renderer renderer)
    {
        // For each face of the cube:
        for (int f = 0; f < 6; f++)
        {
            if (loadedTexture != null                                               // If this current texture exists
                && FaceMaterialIndices[f] < renderer.materials.Length)              // and if there is a material corresponding to this face
            {
                // Assign this texture to the corresponding face
                renderer.materials[FaceMaterialIndices[f]].mainTexture = loadedTexture;
            }
        }
    }

    public void CreateAddedProductLoadedTexture(string filePath)
    {
        // Load the image into a texture
        addedProductTexture = LoadPNG(filePath);
    }

    public void CreateAndApplyAddedProductLoadedTexture(string[] filePaths, Renderer renderer)
    {
        // Array to keep track of which faces we've already applied textures to
        bool[] appliedTextureToFace = new bool[6];

        for (int i = 0; i < filePaths.Length; i++)  
        {
            // Whether we've already applied an image to this face of the product
            bool alreadyApplied = false;

            // Get the form of the image name that containts the Dot number
            string imageName;
            if (IsDotNumberExtension(Path.GetExtension(filePaths[i])))
            {
                imageName = Path.GetFileName(filePaths[i]);
            }
            else
            {
                imageName = Path.GetFileNameWithoutExtension(filePaths[i]);
            }

            // For each face it could be a texture for
            for (int f = 0; f < FaceTextureNumbers.Length; f++)
            {
                // If it's a texture for this face
                if (imageName.EndsWith("." + FaceTextureNumbers[f]))
                {
                    // Remember whether we've already applied an image to this face
                    alreadyApplied = appliedTextureToFace[f];
                    // Either way, since this image matched, this face definitely will have had an image applied after this
                    appliedTextureToFace[f] = true;
                    // Break out of the for loop since we found the matching face
                    break;
                }
            }

            // Apply the image only if this side doesn't already have an image applied to it
            if (!alreadyApplied)
            {
                ApplyProductImage(filePaths[i], renderer, HasDotNumber(filePaths[i]));
            }

            // Move on to look at the next texture.
        }
    }

    public void ApplyAddedProductImage(Renderer renderer)
    {
        if (addedProductTexture != null                                               // If this current texture exists
            && FaceMaterialIndices[0] < renderer.materials.Length)              // and if there is a material corresponding to the front face
        {
            // Assign this texture to the corresponding face
            renderer.materials[FaceMaterialIndices[0]].mainTexture = addedProductTexture;
        }
    }

    private void ConvertFileLocationsToFileNames()
    {      
        // Clear the name lists
        foreach (List<ProductImageName> nameList in productImageNameLists)
        {
            nameList.Clear();
        }
        if (PsaManager.Instance.IsCloud)
        {
            foreach (Texture2D texture in CloudTextures)
            {
                // Create an entry for this image
                ProductImageName imageName = new ProductImageName("", texture.name);
                imageName.CloudImportedImage = texture;

                // Add the image to each of the lists
                foreach (List<ProductImageName> nameList in productImageNameLists)
                {
                    nameList.Add(imageName);
                }
            }
        }
        else
        {
            //Sort the list again
            PublicTextureFileLocations = PublicTextureFileLocations.OrderBy(s => Path.GetFileNameWithoutExtension(s)).ToList();

            foreach (string fileLoc in PublicTextureFileLocations)
            {
                string fileName;

                // If it's a .# file, keep the extension
                if (IsDotNumberExtension(Path.GetExtension(fileLoc)))
                {
                    fileName = Path.GetFileName(fileLoc);
                }
                else
                {
                    fileName = Path.GetFileNameWithoutExtension(fileLoc);
                }

                // Create an entry for this image
                ProductImageName imageName = new ProductImageName(fileLoc, fileName);

                // Add the image to each of the lists
                foreach (List<ProductImageName> nameList in productImageNameLists)
                {
                    nameList.Add(imageName);
                }
            }
        }

        // Organize each list in alphabetical order based on its image name with i digits stripped off the front of the image name
        for (int i = 0; i < productImageNameLists.Count; i++)
        {
            productImageNameLists[i] = productImageNameLists[i].OrderBy(s => s.ImageName[i]).ToList();
        }

    }

    private static Texture2D LoadPNG(string filePath)
    {
        Texture2D tex = null;
        byte[] fileData;

        tex = new Texture2D(2, 2);

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);

            bool loaded = tex.LoadImage(fileData); //this will auto-resize the texture dimensions
            if (!loaded)
            {
                Debug.LogError("Was unable to load a PNG from the location \"" + filePath + "\".");
            }
        }
        else
        {
            Debug.LogError("Tried to load a PNG from the location \"" + filePath + "\" but the file did not exist.");
        }

        // Change the texture's name to match its image file name
        string textureName;
        // If it's a .# file, keep the extension
        if (IsDotNumberExtension(Path.GetExtension(filePath)))
        {
            textureName = Path.GetFileName(filePath);
        }
        else
        {
            textureName = Path.GetFileNameWithoutExtension(filePath);
        }
        tex.name = textureName;
        return tex;
    }

    public void AddImageAndSortList(string imageFilePath)
    {
        //Add new file to texture list
        PublicTextureFileLocations.Add(imageFilePath);

        //Sort the list again
        PublicTextureFileLocations = PublicTextureFileLocations.OrderBy(s => Path.GetFileNameWithoutExtension(s)).ToList();
    }

    public void AddImagesAndSortList(string[] imageFilePath)
    {
        //Add new file to texture list
        PublicTextureFileLocations.AddRange(imageFilePath);

        //Sort the list again
        PublicTextureFileLocations = PublicTextureFileLocations.OrderBy(s => Path.GetFileNameWithoutExtension(s)).ToList();
    }

    /// <summary>
    /// returns true if this is an image that has a dotnum extension OR ends in a dot num AND something else
    /// Example: 12345678.1.png == true
    /// Example2: 12345678.1 == true
    /// Example3: 12345678.1.tga == true
    /// Example4: 12345678.png == false
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static bool HasDotNumber(string fileName)
    {
        string fileNameWithoutPNGExtension = fileName;
        string extension = Path.GetExtension(fileName);
        if (!IsDotNumberExtension(extension))
        {
            fileNameWithoutPNGExtension = Path.GetFileNameWithoutExtension(fileName);
        }
        return fileNameWithoutPNGExtension.Length >= 2 && (fileNameWithoutPNGExtension[fileNameWithoutPNGExtension.Length - 2] == '.' && Char.IsNumber(fileNameWithoutPNGExtension[fileNameWithoutPNGExtension.Length - 1]));
    }

    /// <summary>
    /// returns true if file name ends in a dot number extension
    /// Example: 12345678.1 == true
    /// Example2: 12345678.1.png == false
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static bool HasDotNumberExtension(string fileName)
    {
        string extension = Path.GetExtension(fileName);
        return IsDotNumberExtension(extension);
    }

    /// <summary>
    /// returns true if extension consists only of dot and then a single number
    /// Example: 12345678.1 == false
    /// Example: .1 == true
    /// </summary>
    /// <param name="extension"></param>
    /// <returns></returns>
    public static bool IsDotNumberExtension(string extension)
    {
        return extension.Length == 2 && (extension[0] == '.' && Char.IsNumber(extension[1]));
    }

    //Check if texture is a MissingTexture
    public void RemoveFromMissingTextures(string texture)
    {
        //if texture exists in Missing Textures, remove it
        if (MissingTextures.Contains(texture))
            MissingTextures.Remove(texture);
    }

    /// <summary>
    /// Gets the name of the texture at the specified index in the specified list with the corresponding number of digits stripped from the front.
    /// </summary>
    /// <param name="textureIndex">The texture of the index in the proper list.</param>
    /// <param name="firstDigitsStripped">The number of digits that should be stripped from the front of the product image name. 
    /// Also corresponds to the index in productImageNameLists of the List we should check.</param>
    /// <returns></returns>
    private string GetProductImageName(int textureIndex, int firstDigitsStripped)
    {
        return productImageNameLists[firstDigitsStripped][textureIndex].ImageName[firstDigitsStripped];
    }
    //Clear list ProductImageNameLists 
    public void ProductImageNameListsClear()
    {
        foreach (List<ProductImageName> nameList in productImageNameLists)
        {
            nameList.Clear();
        }
    }
}
