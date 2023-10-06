using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportedProductFileTypes : MonoBehaviour {
    public static string[] Images = { "png", "tga", "1", "2", "3", "7", "8", "9" };
    //  "tga" ,
    public static string BuildFileChooserFilterRegex(string[] supportedExtensions)
    {
        // Build a regex of all of the supported image file extensions
        string extensionRegex = "";
        for (int ext = 0; ext < supportedExtensions.Length; ext++)
        {
            // If this isn't the first extension we've looked at
            if (ext != 0)
            {
                // Append it onto the existing string with the "or" bar
                extensionRegex += "|";
            }
            // Append this extension to the string
            extensionRegex += supportedExtensions[ext];
        }
        return extensionRegex;
    }
}
