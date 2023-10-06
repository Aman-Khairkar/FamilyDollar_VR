using JDA;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
[ExecuteInEditMode]
#endif
public class PsaManager : MonoBehaviour {
    /// <summary>
    /// Called event for when we successfully load in a PSA
    /// </summary>
    /// <param name="loadedPSA">The PSA that was just loaded in</param>
    public delegate void PsaLoadedEvent(PSA loadedPSA);
    /// <summary>
    /// Attach here to be alerted when a new PSA is loaded into the PsaManager
    /// </summary>
    public PsaLoadedEvent OnPsaLoaded;
    public bool IsCloud;
    public bool PerformanceMode;

    [SerializeField]
    private GameObject productPositionPrefab;
    [SerializeField]
    private GameObject fixturePrefab;
    [SerializeField]
    private GameObject posmPrefab;
    [SerializeField]
    private GameObject segmentPrefab;
    [SerializeField]
    private GameObject shelfAccentsPrefab;
    [SerializeField]
    private Material shelfMaterial;
    [SerializeField]
    private Material backMaterial;
    [SerializeField]
    private Material baseMaterial;
    [SerializeField]
    private Mesh simplifiedBoxMesh;

    private PSA loadedPSA;
    /// <summary>
    /// The currently loaded PSA from the last call to ImportPsa
    /// Will be used when building a Shelf GameObject in BuildPsa
    /// </summary>
    public PSA LoadedPSA {
        get {
            return loadedPSA;
        }

        set
        {
            loadedPSA = value;
        }
    }

    private string loadedPsaPath = "";
    /// <summary>
    /// The file path to the last loaded PSA file that is being stored in LoadedPSA
    /// Populated after calling ImportPsa
    /// </summary>
    public string LoadedPsaPath {
        get {
            return loadedPsaPath;
        }
    }

    public static GameObject TempTopLevelPsaGameObject;
    private GameObject topLevelPsaGameObject = null;
    public GameObject TopLevelGameObject {
        get {
            return topLevelPsaGameObject;
        }
    }

    private List<int> planogramsSelectedForInteraction = new List<int>();
    /// <summary>
    /// Curated list of planogram ids that has been selected to interact with
    /// </summary>
    public List<int> PlanogramsSelectedForInteraction {
        get {
            return planogramsSelectedForInteraction;
        }
        set {
            planogramsSelectedForInteraction = value;
        }
    }

    private string materialsFolderParentPath = "";
    /// <summary>
    /// Path to the folder where materials should be stored (used in editor mode only)
    /// </summary>
    public string MaterialsFolderPath
    {
        get
        {
            return materialsFolderParentPath;
        }
        set
        {
            materialsFolderParentPath = value;
        }
    }

    private static PsaManager instance;
    /// <summary>
    /// Current singleton instance of the PsaManager
    /// </summary>
    public static PsaManager Instance {
        get
        {
            if (instance == null)
            {
                // Check if there's already an instance of this object we don't know about (likely if we're working in the editor)
                GameObject psaManager = GameObject.Find("_PsaManager");
                if (psaManager != null)
                {
                    instance = psaManager.GetComponent<PsaManager>();
                }

                // If there wasn't one already, we'll make one
                if (instance == null)
                {
                    GameObject instanceObject = new GameObject("_PsaManager");
                    instance = instanceObject.AddComponent<PsaManager>();
                }
            }

            return instance;
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

    /// <summary>
    /// Reads in and keeps a reference to the PSA object
    /// </summary>
    /// <param name="psaFilePath"></param>
    /// <returns>Whether the PSA import was successful</returns>
    public bool ImportPsa(string psaFilePath) {
        if (File.Exists(psaFilePath)) {
            string errorLog;
            //if this is a csv file use the csv import else use the default PSA import process.
            if (Path.GetExtension(psaFilePath) == ".csv")
            {
                loadedPSA = CSVImport.Read(psaFilePath, out errorLog);
            }
            else
            {
                loadedPSA = PSA.Read(psaFilePath, out errorLog);
            }

            if (loadedPSA != null && loadedPSA.TheProject != null && loadedPSA.TheProject.Planograms != null) {
                loadedPsaPath = psaFilePath;
                if (OnPsaLoaded != null) {
                    OnPsaLoaded(loadedPSA);
                }
                return true;
            } else {
                Debug.LogError("PSA Import failed with errors:\n" + errorLog);
                return false;
            }
        } else {
            Debug.LogError("PSA Import failed, no file at specified location " + psaFilePath);
            return false;
        }
    }

    /// <summary>
    /// Reads in and keeps a reference to the PSA object
    /// </summary>
    /// <param name="psaRawData"></param>
    /// <returns>Whether the PSA import was successful</returns>
    public bool ImportPsaRawData(string[] psaRawData)
    {
        if (psaRawData != null)
        {
            string errorLog;
            //Debug.Log("Accessed Read");
            loadedPSA = PSA.Read(psaRawData, out errorLog);

            if (loadedPSA != null && loadedPSA.TheProject != null && loadedPSA.TheProject.Planograms != null)
            {
                if (OnPsaLoaded != null)
                {
                    OnPsaLoaded(loadedPSA);
                }
                return true;
            }
            else
            {
                Debug.LogError("PSA Import failed with errors:\n" + errorLog);
                return false;
            }
        }
        else
        {
            Debug.LogError("PSA Import failed, no data was found.");
            return false;
        }
    }

    public void LoadProductTextures(string textureFolder) {
        List<string> files = new List<string>();
        foreach (string extension in SupportedProductFileTypes.Images)
        {
            //load all of the files from that folder of this extension
            files.AddRange(Directory.GetFiles(textureFolder, "*." + extension));
        }

        ApplyImagesToProducts.Instance.PublicTextureFileLocations = files;
    }

    /// <summary>
    /// Runs a build process from the LoadedPsa object
    /// Make sure to call ImportPsa before this call
    /// This version of the method will create single mesh for each product position 
    /// and tile the texture to display multiple products
    /// </summary>
    /// <param name="shelfObjectName">Name for your new game object</param>
    public void BuildPsaWithSingleMeshPositions(string shelfObjectName) {
        BuildPsa(shelfObjectName, PsaShelfBuilder.PositionBuildOption.SimplePositionCube);
    }

    /// <summary>
    /// Runs a build process from the LoadedPsa object
    /// Make sure to call ImportPsa before this call
    /// This version of the method creates new game objects for each instance of a product within a position,
    /// but then combines the meshes of each depth-row of products for improved performance.
    /// </summary>
    /// <param name="shelfObjectName">Name for your new game object</param>
    public void BuildPsaWithIndividualProductsInPositions(string shelfObjectName)
    {
        BuildPsa(shelfObjectName, PsaShelfBuilder.PositionBuildOption.CombinedProductObjects);
    }

    /// <summary>
    /// Runs a build process from the LoadedPsa object
    /// Make sure to call ImportPsa before this call
    /// This version of the method creates new game objects for each instance of a product within a position
    /// and leaves the meshes separate to allow for easier manipulation of individual products.
    /// </summary>
    /// <param name="shelfObjectName">Name for your new game object</param>
    public void BuildPsaWithSeparateIndividualProductsInPositions(string shelfObjectName)
    {
        BuildPsa(shelfObjectName, PsaShelfBuilder.PositionBuildOption.SeparateProductObjects);
    }

    private void BuildPsa(string shelfObjectName, PsaShelfBuilder.PositionBuildOption buildPositionInSingleMesh)
    {
        topLevelPsaGameObject = null;
        TempTopLevelPsaGameObject = null;

        StartCoroutine(PsaShelfBuilder.BuildPsaShelf(LoadedPSA, planogramsSelectedForInteraction, shelfObjectName, buildPositionInSingleMesh,
                                      productPositionPrefab, fixturePrefab, posmPrefab, segmentPrefab, shelfAccentsPrefab,
                                      shelfMaterial, backMaterial, baseMaterial, PerformanceMode, simplifiedBoxMesh, materialsFolderParentPath));
        StartCoroutine(CompleteBuild());
    }

    private IEnumerator CompleteBuild()
    {
        yield return new WaitUntil(BuildComplete);

        topLevelPsaGameObject = TempTopLevelPsaGameObject;

        StopAllCoroutines();
    }

    private bool BuildComplete()
    {
        return !PsaShelfBuilder.BuildInProgress;
    }
}
