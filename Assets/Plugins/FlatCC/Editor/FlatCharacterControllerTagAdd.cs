using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class FlatCharacterControllerTagAdd
{
    static FlatCharacterControllerTagAdd()
    {
        //this opens up the tag manager
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");
        //this needs to happen for Unity5
        SerializedProperty layersProp = tagManager.FindProperty("layers");
        //adding a tag
        string s = "CharacterController";
        //first check if it's not already present
        bool found = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(s)) { found = true; break; }
        }
        //if not found then add it
        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(0);
            SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
            n.stringValue = s;
        }
        //save the changes
        tagManager.ApplyModifiedProperties();
    }
}
