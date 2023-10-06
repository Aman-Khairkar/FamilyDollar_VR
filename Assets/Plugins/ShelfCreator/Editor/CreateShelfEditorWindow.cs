using UnityEditor;
using UnityEngine;

public class CreateShelfEditorWindow : EditorWindow
{
    public static float LeftPanelWidth = 0.0f;

    public GameObject psaConverterGameObjPrefab;
    public PSAToText psaToTextScript;
    public PsaIntegrationLibraryEventHandlers psaEventHandlers;
    public PriceTagEventHandlers priceTagsEventHandlers;

    float progressBarFill = 0f;
    string currentStatusMessage = "Rendering...";
    Rect progressBarRect = new Rect(8f, 100f, 250f, 20f);

    bool setUpGuiEventHandlers = false;
    bool organizeProductsBySegmentAndLayer = true;
    bool createdInUnityVersionOlderThan2018_3 = false;

    public const string pluginName = "Shelf Creator";
    public const string versionNumber = "2.18";

    [MenuItem("TVS Unity Tools/" + pluginName + " - " + versionNumber, false, 400)]
    static void LoadWindow()
    {
        EditorWindow.GetWindow<CreateShelfEditorWindow>("Create Shelf");
    }

    void OnGUI()
    {
#if ALLOW_SHELF_BUILD_IN_EDITOR
        if (PsaShelfBuilder.OnProgressChange == null)
        {
            PsaShelfBuilder.OnProgressChange += UpdateProgressBarFill;
            setUpGuiEventHandlers = true;
        }

        /* * * * * * * * * * * * * * * * * * * * * * * 
         *  Get remembered settings from PlayerPrefs *
         * * * * * * * * * * * * * * * * * * * * * * */

        //if our hierarchy structure value is available in the PlayerPrefs
        if (PlayerPrefs.HasKey(PSAToText.ORGANIZE_PRODUCTS_BY_SEGMENT_AND_LAYER))
        {
            organizeProductsBySegmentAndLayer = PlayerPrefs.GetInt(PSAToText.ORGANIZE_PRODUCTS_BY_SEGMENT_AND_LAYER) != 0;
        }
        else
        {
            // Get the current value from PSAToText
            organizeProductsBySegmentAndLayer = PSAToText.OrganizeProductsBySegmentAndLayer;
        }

        // if our original unity version value is available in the PlayerPrefs
        if (PlayerPrefs.HasKey(PSAToText.CREATED_IN_UNITY_VERSION_OLDER_THAN_2018_3))
        {
            createdInUnityVersionOlderThan2018_3 = PlayerPrefs.GetInt(PSAToText.CREATED_IN_UNITY_VERSION_OLDER_THAN_2018_3) != 0;
        }
        else
        {
            // False by default
            createdInUnityVersionOlderThan2018_3 = false;
        }

        /* * * * * * * * * * * * * * * * * * * * * * * 
         *                  Draw GUI                 *
         * * * * * * * * * * * * * * * * * * * * * * */

        EditorGUILayout.BeginVertical("box", GUILayout.Width(LeftPanelWidth));
        
        GUILayout.Label("Create Shelf", EditorStyles.boldLabel);

        GUILayout.Space(5);

        organizeProductsBySegmentAndLayer = GUILayout.Toggle(organizeProductsBySegmentAndLayer, new GUIContent("Organize products by segment and layer"));

#if UNITY_2018_3_OR_NEWER
        createdInUnityVersionOlderThan2018_3 = GUILayout.Toggle(createdInUnityVersionOlderThan2018_3, new GUIContent("This project was originally created in a Unity version older than 2018.3.14f1"));
#endif
        GUILayout.Space(10);

        // Button to select a PSA file to build a new shelf
        if (GUILayout.Button("Select PSA"))
        {
            PreShelfCreationSetup();

            psaToTextScript.ImportNewPSA();

            //PostShelfCreationCleanup();
        }

        // Show which object is currently selected
        GameObject selectedGO = Selection.activeGameObject;
        EditorGUILayout.ObjectField("Currently selected shelf", selectedGO, typeof(GameObject), true);

        // Check whether the selected object is in fact a shelf
        bool validSelection = (selectedGO != null) && (selectedGO.GetComponent<ScenarioIdentifier>() != null);

        // Disable the rebuild button if the object wasn't a shelf
        EditorGUI.BeginDisabledGroup(!validSelection);
        // Button to rebuild an existing shelf
        if (GUILayout.Button("Rebuild selected shelf"))
        {
            PreShelfCreationSetup();
            
            // Rebuild the shelf
            psaToTextScript.ReimportExistingPSA(selectedGO);

            //PostShelfCreationCleanup();
        }

        if (psaToTextScript != null
            && !PsaShelfBuilder.BuildInProgress)
        {
            PostShelfCreationCleanup();
        }
        EditorGUI.EndDisabledGroup();

        GUILayout.EndVertical();

        // Update value in PSAToText
        PSAToText.SetOrganizeProductsBySegmentAndLayerOption(organizeProductsBySegmentAndLayer);
        PSAToText.SetUpgradedFromOldUnityVersion(createdInUnityVersionOlderThan2018_3);

        GUILayout.Space(5);

        DrawStatusPanel();
#else
        GUILayout.Label("Loading scripting define symbols. This may take a minute.");
#endif
    }

    void DrawStatusPanel()
    {
        Rect panelRect = EditorGUILayout.BeginVertical("box");

        EditorStyles.label.wordWrap = true;

        GUILayout.Label("Shelf Build Status", EditorStyles.boldLabel);

        GUILayout.Space(5);

        if (PsaShelfBuilder.BuildInProgress)
        {
            // Configure the progress bar rect to sit nicely in the panel
            progressBarRect.x = panelRect.x + 5;
            progressBarRect.y = panelRect.y + 25;
            progressBarRect.width = Mathf.Min(250f, panelRect.size.x);

            // Draw the load bar
            EditorGUI.ProgressBar(progressBarRect, progressBarFill, currentStatusMessage);
            GUILayout.Space(progressBarRect.height);

            GUILayout.Space(5);

            EditorGUILayout.LabelField("Building shelf...");
        }
        else
        {
            EditorGUILayout.LabelField("No shelf is currently being built.");
        }
        
        EditorGUILayout.EndVertical();
    }
    
    void PreShelfCreationSetup()
    {
        progressBarFill = 0;

        // Get PSAToText script
        if (psaToTextScript == null)
        {
            psaToTextScript = FindObjectOfType<PSAToText>();
            if (psaToTextScript == null)
            {
                GameObject psaConverterObject = PrefabUtility.InstantiatePrefab(psaConverterGameObjPrefab) as GameObject;
                psaToTextScript = psaConverterObject.GetComponent<PSAToText>();
            }
        }

        // Get PsaIntegrationLibraryEventHandlers script
        if (psaEventHandlers == null)
        {
            psaEventHandlers = psaToTextScript.GetComponent<PsaIntegrationLibraryEventHandlers>();
        }

        // Add the necessary event handlers for PsaIntegrationLibrary
        if (PsaShelfBuilder.OnPlanogramObjectCreated == null)
        {
            if (psaEventHandlers != null)
            {
                psaEventHandlers.AddEventHandlers();
            }
        }

        // Get PriceTagEventHandlers script
        if (priceTagsEventHandlers == null)
        {
            priceTagsEventHandlers = psaToTextScript.GetComponent<PriceTagEventHandlers>();
        }

        // Add the necessary event handlers for PriceTags
        if (PriceTags.PriceTag.FindShelf == null)
        {
            if (priceTagsEventHandlers != null)
            {
                priceTagsEventHandlers.AddEventHandlers();
            }
        }

        // Spawn the necessary objects for creating price tags
        if (psaEventHandlers.priceTagsObject == null)
        {
            psaEventHandlers.SetUpPriceTags();
        }
    }

    void PostShelfCreationCleanup()
    {
        // Delete the extra game objects since we don't need them anymore
        DestroyImmediate(psaToTextScript.gameObject);
        DestroyImmediate(PsaManager.Instance.gameObject);
    }

    public void UpdateProgressBarFill(float progress, string statusMessage, int currentShelfIndex, int totalShelfCount)
    {
        //Debug.Log("Progress bar updated. \nStatus: \"" + statusMessage + "\" \nProgress: " + (progress * 100) + "%"
        //    + "\nTime: " + System.DateTime.Now.Minute + ":" + System.DateTime.Now.Second + "." + System.DateTime.Now.Millisecond);

        // Update how much to fill the progress bar
        progressBarFill = progress;
        string shelfIndexTrackerMessage = "";
        if (currentShelfIndex > -1)
        {
            shelfIndexTrackerMessage = "[" + (currentShelfIndex + 1) + "/" + totalShelfCount + "] ";
        }
        currentStatusMessage = shelfIndexTrackerMessage + statusMessage + " (" + (int)(progress * 100) + "%)";

        // Redraw the window so the user sees the update
        Repaint();
    }

    void OnSelectionChange()
    {
        Repaint();
    }
}
