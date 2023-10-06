using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class PsaIntegrationLibraryTagAdd
{
    static PsaIntegrationLibraryTagAdd()
    {
        //this opens up the tag manager
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

        SerializedProperty tagsProp = tagManager.FindProperty("tags");
        //adding a tag
        string[] tags = { "PositionRenderer", "ShelfBase" };
        foreach (string tag in tags)
        {
            //first check if it's not already present
            bool found = false;
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
                if (t.stringValue.Equals(tag)) { found = true; break; }
            }
            //if not found then add it to the end of the array
            if (!found)
            {
                int index = tagsProp.arraySize;
                tagsProp.InsertArrayElementAtIndex(index);
                SerializedProperty n = tagsProp.GetArrayElementAtIndex(index);
                n.stringValue = tag;
            }
        }

        SerializedProperty layerProp = tagManager.FindProperty("layers");
        //adding a layer
        string[] layers = { "Products" };
        foreach (string layer in layers)
        {
            //first check if it's not already present
            bool found = false;
            for (int i = 0; i < layerProp.arraySize; i++)
            {
                SerializedProperty l = layerProp.GetArrayElementAtIndex(i);
                if (l.stringValue.Equals(layer)) { found = true; break; }
            }
            //if not found then add it
            if (!found)
            {
                int index = 8;
                layerProp.InsertArrayElementAtIndex(index);
                SerializedProperty n = layerProp.GetArrayElementAtIndex(index);
                n.stringValue = layer;
            }
        }

        //save the changes
        tagManager.ApplyModifiedProperties();
    }

}

