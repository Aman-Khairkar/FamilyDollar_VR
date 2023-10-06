using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine.UI;
using UnityEditor;

public class LoadImagesFromFolder : MonoBehaviour {
    public static void OpenFileChooserToLoadTextures()
    {
        // Open the filechooser
        string imagesFilePath = EditorUtility.OpenFilePanel("Open Product Images", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "png");

        AssetDatabase.ImportAsset(imagesFilePath);
        AssetDatabase.Refresh();
    }
}
