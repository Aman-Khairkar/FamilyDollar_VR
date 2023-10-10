using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class CharacterControllerEditorWindow : EditorWindow
{
    public GameObject CharacterControllerPrefab;

    public static float LeftPanelWidth = 0.0f;
    public FlatPlayerController fpsController;

    public float speed = 0.0f;
    public float height = 0.0f;

    public float minSpeed = 1f;
    public float maxSpeed = 20f;

    public float minHeight = 0.5f;
    public float maxHeight = 1.0f;    

    public const string pluginName = "2D Character Controller";
    public const string versionNumber = "2.13";

    [MenuItem("TVS Unity Tools/" + pluginName + " - " + versionNumber, false, 400)]
    static void LoadWindow()
    {
        EditorWindow.GetWindow<CharacterControllerEditorWindow>("Character Controller");
    }

    void OnGUI()
    {
        if (fpsController == null)
        {
            ResetValuesOnSceneChange();
            fpsController = CurrentSceneFPSController();
        }
        
        if (EditorApplication.isPlaying || EditorApplication.isPaused)
        {
            DrawPlayModePanel();
        }
        else
        {
            DrawLeftPanel();
        }
    }

    void DrawLeftPanel()
    {
        //get the current fps controller and apply its settings to the GUI sliders
        if (fpsController == null)
        {
            GUILayout.Label("There is no Character Controller in this scene.", EditorStyles.boldLabel);

            if (GUILayout.Button("Add Character Controller to Scene", GUILayout.Width(300)))
            {
                PrefabUtility.InstantiatePrefab(CharacterControllerPrefab);

                //Unity's manager for tags 
                SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
                //tags property 
                SerializedProperty tagsProperty = tagManager.FindProperty("tags");

                for (int i = 0; i < tagsProperty.arraySize; i++)
                {
                    SerializedProperty aProjTag = tagsProperty.GetArrayElementAtIndex(i);

                    //Check if VRCharacterController is a viable tag in the project
                    if (aProjTag.stringValue.Equals("VRCharacterController"))
                    {
                        GameObject VRController = GameObject.FindGameObjectWithTag("VRCharacterController");

                        //Check if the VR Controller is in the scene
                        if (VRController != null)
                        {
                            Transform FlatControllerTransform = GameObject.FindGameObjectWithTag("CharacterController").transform;
                            Transform VRControllerTransform = VRController.transform;
                            
                            //if the VR Controller is found, match its position and rotation
                            FlatControllerTransform.position = new Vector3(VRControllerTransform.position.x, FlatControllerTransform.position.y, VRControllerTransform.position.z);
                            FlatControllerTransform.rotation = VRControllerTransform.rotation;
                        
                        }
                        break;
                    }
                }
            }

            return;
        }
        
        //if our height value is available in the PlayerPrefs
        if (PlayerPrefs.HasKey(FlatPlayerController.VIEWER_HEIGHT_PLAYERPREF_KEY))
        {
            height = PlayerPrefs.GetFloat(FlatPlayerController.VIEWER_HEIGHT_PLAYERPREF_KEY);
        }
        else
        {
            // Pull the current height from the character controller
            Transform fpsTransform = fpsController.transform;
            height = fpsTransform.localScale.y;
        }

        //if our speed value is available in the PlayerPrefs
        if (PlayerPrefs.HasKey(FlatPlayerController.WALK_SPEED_PLAYERPREF_KEY))
        {
            speed = PlayerPrefs.GetFloat(FlatPlayerController.WALK_SPEED_PLAYERPREF_KEY);
        }
        else
        {
            // Pull the current speed from the character controller
            speed = fpsController.WalkSpeed;
        }

        EditorGUILayout.BeginVertical("box", GUILayout.Width(LeftPanelWidth));

        GUILayout.Label("Settings", EditorStyles.boldLabel);

        GUIContent heightLabel = new GUIContent("Height");
        
        height = EditorGUILayout.Slider(heightLabel, height, minHeight, maxHeight);

        //apply the height number
        fpsController.SetViewerHeight(height);

        GUILayout.Space(20);

        GUIContent speedLabel = new GUIContent("Speed");

        EditorGUI.BeginChangeCheck();
        speed = EditorGUILayout.Slider(speedLabel, speed, minSpeed, maxSpeed);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(fpsController, "Change Flat CC Speed");
        }        
        EditorGUILayout.EndVertical();

        //apply the speed number
        fpsController.SetWalkSpeed(speed);
    }

    void DrawPlayModePanel()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.Width(LeftPanelWidth));

        GUILayout.Label("Settings", EditorStyles.boldLabel);

        GUILayout.Label("Exit play mode to change Flat Character Controller settings in the editor.");
        
        EditorGUILayout.EndVertical();
    }

    private FlatPlayerController CurrentSceneFPSController()
    {
        //find the current manager if there is one
        FlatPlayerController currentFPSController = FindObjectOfType<FlatPlayerController>();

        //if there wasn't one in this scene
        if (currentFPSController == null)
        {
            //we don't want to actually automatically add one
            return null;
        }

        return currentFPSController;
    }

    public void ResetValuesOnSceneChange()
    {
        speed = 0.0f;
        height = 0.0f;
    }    
}
