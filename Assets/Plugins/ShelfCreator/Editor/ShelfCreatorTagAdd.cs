using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class ShelfCreatorTagAdd
{

    static ShelfCreatorTagAdd()
    {
        //this opens up the tag manager
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

        SerializedProperty tagsProp = tagManager.FindProperty("tags");
        //adding a tag
        string[] tags = { "Bar", "Shelf", "Pegboard" };
        foreach (string tag in tags)
        {
            //first check if it's not already present
            bool found = false;
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
                if (t.stringValue.Equals(tag)) { found = true; break; }
            }
            //if not found then add it
            if (!found)
            {
                tagsProp.InsertArrayElementAtIndex(0);
                SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
                n.stringValue = tag;
            }
        }

        SerializedProperty layerProp = tagManager.FindProperty("layers");
        //adding a layer
        string[] layers = { "Shelf" };
        int[] layerIndices = { 11 };
        for (int i = 0; i < layers.Length; i++)
        {
            //first check if it's not already present
            bool found = false;
            for (int j = 0; j < layerProp.arraySize; j++)
            {
                SerializedProperty l = layerProp.GetArrayElementAtIndex(j);
                if (l.stringValue.Equals(layers[i]))
                {
                    if (j != layerIndices[i])
                    {
                        layerProp.MoveArrayElement(j, layerIndices[i]);
                    }
                    found = true;
                    break;
                }
            }
            //if not found then add it
            if (!found)
            {
                layerProp.InsertArrayElementAtIndex(layerIndices[i]);
                SerializedProperty n = layerProp.GetArrayElementAtIndex(layerIndices[i]);
                n.stringValue = layers[i];
            }
        }

        //save the changes
        tagManager.ApplyModifiedProperties();
    }

}
