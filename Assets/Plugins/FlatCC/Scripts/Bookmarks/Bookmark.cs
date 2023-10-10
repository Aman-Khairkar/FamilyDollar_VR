using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Bookmark : MonoBehaviour
{
    private class BookmarkSaveData
    {
        public string Name;
        public string ThumbnailFilePath;
        public Vector3 Position;
        public Vector3 RotationEulerAngles;
        public string Scene;
        public bool LoadScene;
        public float Speed;
        public bool FlyMode;
        public float ViewerHeight;

        public BookmarkSaveData(string bookmarkName, string thumbnailFileRelativePath, Vector3 savedPosition, Vector3 savedRotationEulerAngles, string sceneName, bool loadScene, float speed, bool flyMode, float viewerHeight)
        {
            Name = bookmarkName;
            ThumbnailFilePath = thumbnailFileRelativePath;
            Position = savedPosition;
            RotationEulerAngles = savedRotationEulerAngles;
            Scene = sceneName;
            LoadScene = loadScene;
            Speed = speed;
            FlyMode = flyMode;
            ViewerHeight = viewerHeight;
        }
    }

    // Variables for storing the name and path of the directory where we'll keep our bookmark data
    public const string BookmarkDataFolderName = "BookmarkData";
    private static string bookmarkDataFolderPath = "";
    public static string BookmarkDataFolderPath
    {
        get
        {
            if (string.IsNullOrEmpty(bookmarkDataFolderPath))
            {
                string assetsPath = Application.dataPath;
                assetsPath = assetsPath.Replace("/", "\\");
                bookmarkDataFolderPath = Path.Combine(assetsPath, BookmarkDataFolderName);
            }
            return bookmarkDataFolderPath;
        }
    }

    // Variables for storing the name and path of the directory where we'll keep our bookmark thumbnails.
    // This is a subdirectory of the general bookmark data folder.
    private const string bookmarkThumbnailsFolderName = "Thumbnails";
    private static string bookmarkThumbnailsFolderPath = "";
    public static string BookmarkThumbnailsFolderPath
    {
        get
        {
            if (string.IsNullOrEmpty(bookmarkThumbnailsFolderPath))
            {
                bookmarkThumbnailsFolderPath = Path.Combine(BookmarkDataFolderPath, bookmarkThumbnailsFolderName);
            }
            return bookmarkThumbnailsFolderPath;
        }
    }

    private string bookmarkName = "Bookmark";
    public string BookmarkName
    {
        get
        {
            return bookmarkName;
        }
    }

    public static FlatPlayerController CharacterController;

    public Text NameLabel;
    public GameObject DeleteButton;
    public Image ThumbnailImage;
    public BookmarkIndicator ActiveIndicator;
    public RectTransform BookmarkBounds;
    public Image RaycastTargetImage;
    public BookmarkLoadSceneIndicator LoadSceneIndicator;

    /// <summary>
    /// Amount by which to adjust the position of the bookmark in relation to the mouse when dragging bookmarks.
    /// Based on the dimensions of BookmarkBounds
    /// </summary>
    private Vector3 positionAdjust;

    public string ThumbnailFileName;
    public string SceneName;
    private bool _loadScene = true; // True by default
    public bool LoadScene {
        get
        {
            return _loadScene;
        }
    }
    public Vector3 SavedPosition;
    /// <summary>
    /// The euler angles of the rotation saved in this bookmark.
    /// When using, note that the x and z values come from / are applied to the CameraParent object (Character's direct child),
    /// while the y value comes from / is applied to the top-level Character object itself.
    /// </summary>
    public Vector3 SavedRotationEulerAngles;
    /// <summary>
    /// How quickly the player to move from their previous position to the bookmark position when it is activated, in meters/second.
    /// </summary>
    public float Speed;
    public bool FlyMode;
    public float ViewerHeight;

    public bool Dragging = false;

    public string ThumbnailFilePath
    {
        get
        {
            return Path.Combine(BookmarkThumbnailsFolderPath, ThumbnailFileName);
        }
    }

    private void Start()
    {
        // If the bookmark data folder doesn't exist, create it
        if (!Directory.Exists(BookmarkDataFolderPath))
        {
            Directory.CreateDirectory(BookmarkDataFolderPath);
        }
        // If the bookmark thumbnails folder doesn't exist, create it
        if (!Directory.Exists(BookmarkThumbnailsFolderPath))
        {
            Directory.CreateDirectory(BookmarkThumbnailsFolderPath);
        }

        if (CharacterController == null)
        {
            CharacterController = FindObjectOfType<FlatPlayerController>();
        }

        if (BookmarkBounds == null)
        {
            BookmarkBounds = GetComponent<RectTransform>();
        }
        positionAdjust = new Vector3(0, -BookmarkBounds.rect.height / 2, 0);

        LoadSceneIndicator.IndicateActive(LoadScene);

        UpdateForCurrentEditMode();
    }

    private void Update()
    {
        // If this bookmark item is currently being dragged
        if (Dragging)
        {
            // Move it along with the mouse
            transform.position = Input.mousePosition - positionAdjust;
        }
    }

    /// <summary>
    /// Set the bookmark's name and reflect this change in the UI.
    /// </summary>
    /// <param name="name">New name for the bookmark.</param>
    public void SetBookmarkName(string name)
    {
        bookmarkName = name;
        NameLabel.text = name;
    }

    /// <summary>
    /// Apply the data for the current camera view to this bookmark.
    /// </summary>
    public void SetBookmarkToCurrentCameraView()
    {
        if (CharacterController == null)
        {
            CharacterController = FindObjectOfType<FlatPlayerController>();
        }

        // Save the current player position
        SavedPosition = CharacterController.transform.position;
        // Remove the influence of the player height
        SavedPosition.y -= CharacterController.transform.localScale.y;

        // Save the euler angles of the current rotation
        SavedRotationEulerAngles = CharacterController.GetCombinedEulerAngles();

        // Set the scene name to the name of the current scene
        Scene scene = SceneManager.GetActiveScene();
        SceneName = scene.name;

        // Set default speed to 1.5 m/s
        Speed = 1.5f;

        // Save whether the player is currently in fly mode
        FlyMode = CharacterController.FlyMode;

        // Save the viewer height
        ViewerHeight = CharacterController.ViewerHeight;

        SetThumbnailToCurrentCameraView();
    }

    /// <summary>
    /// Create and apply a thumbnail for this bookmark using the current camera view.
    /// </summary>
    public void SetThumbnailToCurrentCameraView()
    {
        StartCoroutine(SetThumbnailToCurrentCameraViewCoroutine());
    }

    /// <summary>
    /// Sets the thumbnail image of the bookmark item to the new thumbnail.
    /// This method converts the Texture2D into a Sprite, then feeds it through the overload of this method that takes a Sprite.
    /// </summary>
    /// <param name="newThumbnailImage">A Texture2D of the new thumbnail image to display.</param>
    private void SetThumbnailImage(Texture2D newThumbnailImage)
    {
        Sprite sprite = Sprite.Create(newThumbnailImage,
                                        new Rect(0, 0, newThumbnailImage.width, newThumbnailImage.height), 
                                        new Vector2(0.5f, 0.5f));
        sprite.name = bookmarkName + " Thumbnail";
        SetThumbnailImage(sprite);
    }

    /// <summary>
    /// Sets the thumbnail image of the bookmark item to the new thumbnail.
    /// </summary>
    /// <param name="newThumbnailImage">A sprite of the new thumbnail image to display.</param>
    private void SetThumbnailImage(Sprite newThumbnailImage)
    {
        ThumbnailImage.sprite = newThumbnailImage;
    }

    /// <summary>
    /// Asynchronously capture a picture of the current camera angle and set this bookmark's thumbnail to match that picture.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetThumbnailToCurrentCameraViewCoroutine()
    {
        // Clean up the old thumbnail file if it exists
        DeleteThumbnailFile();

        // Set the new thumbnail path
        ThumbnailFileName = BookmarkName + ".png";

        // Make absolutely sure our thumbnail file path is unique
        int modifierNumber = 0;
        while (File.Exists(ThumbnailFilePath))
        {
            // Increment our modifier number
            modifierNumber++;

            // Set the new thumbnail path
            ThumbnailFileName = BookmarkName + "_" + modifierNumber + ".png";
        }

        Canvas[] canvasObjects = FindObjectsOfType<Canvas>();
        bool[] wasEnabled = new bool[canvasObjects.Length];

        // Hide all the UI
        for (int i = 0; i < canvasObjects.Length; i++)
        {
            wasEnabled[i] = canvasObjects[i].enabled;
            canvasObjects[i].enabled = false;
        }

        // Wait till the end of the frame
        yield return new WaitForEndOfFrame();

        // Capture the screenshot and save it in the project
        ScreenCapture.CaptureScreenshot(ThumbnailFilePath);

        // Re-enable the UI that was hidden
        for (int i = 0; i < canvasObjects.Length; i++)
        {
            canvasObjects[i].enabled = wasEnabled[i];
        }

        // Wait until the image save is complete
        yield return new WaitUntil(ThumbnailFileExists);

        // Read in the new thumbnail
        Texture2D texture = LoadPNG(ThumbnailFilePath);

        // Set the thumbnail image
        SetThumbnailImage(texture);
    }

    /// <summary>
    /// Deletes this bookmark's thumbnail file, if that thumbnail is not also used by another bookmark. 
    /// If <paramref name="forceDelete"/> is set to true, the thumbnail will be deleted even if another bookmark references it.
    /// </summary>
    /// <param name="forceDelete">If false, deletes the thumbnail image only if there are no other bookmarks referencing it. If true, deletes the thumbnail image regardless. 
    ///                           This is set to false by default.</param>
    public void DeleteThumbnailFile(bool forceDelete = false)
    {
        // If we have a thumbnail file
        if (ThumbnailFileExists())
        {
            // Keep track of whether we can delete this bookmark
            bool canDelete = true;

            // If we're not forcing the deletion
            if (!forceDelete)
            {
                // Check each bookmark in the manager's list
                foreach (Bookmark bookmark in BookmarkManager.Instance.Bookmarks)
                {
                    if (bookmark.ThumbnailFilePath == ThumbnailFilePath // If that bookmark has the same thumbnail file path as this one
                        && bookmark != this)                            // but it's not the same bookmark
                    {
                        // Then we shouldn't delete this thumbnail
                        canDelete = false;
                    }
                }
            }

            // If we determined that the thumbnail file should be deleted
            if (canDelete)
            {
                // Delete it
                File.Delete(ThumbnailFilePath);
            }
        }
    }

    /// <summary>
    /// Checks whether there is a file in the location where we expect to find the thumbnail image.
    /// </summary>
    /// <returns>True if a file exists in the location where we expect to find a thumbnail image, otherwise returns false.</returns>
    public bool ThumbnailFileExists()
    {
        return !string.IsNullOrEmpty(ThumbnailFilePath) && File.Exists(ThumbnailFilePath);
    }

    public static Texture2D LoadPNG(string filePath)
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
        tex.name = Path.GetFileNameWithoutExtension(filePath);

        return tex;
    }

    public void DeleteBookmark()
    {
        BookmarkManager.Instance.DeleteBookmark(this);
    }

    public void UpdateForCurrentEditMode()
    {
        // Show the delete button if edit mode is on, otherwise hide it
        DeleteButton.SetActive(BookmarkManager.Instance.EditMode);

        // Update the load scene indicator so that the user can only change the LoadScene property when in edit mode
        LoadSceneIndicator.UpdateForEditMode(BookmarkManager.Instance.EditMode);
    }

    public string SaveToString()
    {
        BookmarkSaveData saveData = new BookmarkSaveData(bookmarkName, ThumbnailFileName, SavedPosition, SavedRotationEulerAngles, SceneName, LoadScene, Speed, FlyMode, ViewerHeight);
        return JsonUtility.ToJson(saveData);
    }

    public void LoadFromString(string loadDataJson)
    {
        BookmarkSaveData loadData = JsonUtility.FromJson<BookmarkSaveData>(loadDataJson);

        // Apply bookmark name
        SetBookmarkName(loadData.Name);

        // Set the thumbnail image
        ThumbnailFileName = loadData.ThumbnailFilePath;
        if (File.Exists(ThumbnailFilePath))
        {
            Texture2D texture = LoadPNG(ThumbnailFilePath);
            SetThumbnailImage(texture);
        }

        // Set bookmark position and rotation
        SavedPosition = loadData.Position;
        SavedRotationEulerAngles = loadData.RotationEulerAngles;

        // Set the scene name
        SceneName = loadData.Scene;
        SetLoadSceneProperty(loadData.LoadScene);

        // Set bookmark speed
        Speed = loadData.Speed;

        // Set whether this bookmark is in fly mode
        FlyMode = loadData.FlyMode;

        // Set the viewer height of this bookmark
        ViewerHeight = loadData.ViewerHeight;
    }

    /// <summary>
    /// Handle when the user clicks on this bookmark in the UI
    /// </summary>
    public void OnClick()
    {
        // If we're not in edit mode
        if (!BookmarkManager.Instance.EditMode)
        {
            ActivateBookmark();
        }
    }

    /// <summary>
    /// Handle when the user starts dragging this bookmark.
    /// </summary>
    public void OnStartDrag()
    {
        if (BookmarkManager.Instance.EditMode && !Dragging)
        {
            // Turn off the raycast target
            RaycastTargetImage.raycastTarget = false;
            // Pull bookmark UI element out of the list
            transform.SetParent(BookmarkManager.Instance.CanvasTransform);
            // Mark that we're dragging
            Dragging = true;
        }
    }

    /// <summary>
    /// Handle when the user finishes dragging this bookmark, but doesn't drop it on top of another bookmark.
    /// </summary>
    public void OnEndDrag()
    {
        // Turn the raycast target back on
        RaycastTargetImage.raycastTarget = true;
        if (Dragging)
        {
            // Move bookmark to the end of the list
            BookmarkManager.Instance.MoveBookmark(this, BookmarkManager.Instance.Bookmarks.Count);

            // Mark that we're no longer dragging
            Dragging = false;
        }
    }

    /// <summary>
    /// Move the player to this bookmark and show in the UI that it is active.
    /// </summary>
    public void ActivateBookmark(bool stopAnim=false)
    {
        // Show all other bookmarks as inactive
        foreach(Bookmark bookmark in BookmarkManager.Instance.Bookmarks)
        {
            if (bookmark != null && bookmark != this)
            {
                bookmark.IndicateActiveBookmark(false);
            }
        }

        // Show this bookmark as active
        IndicateActiveBookmark(true);

        // Update the current bookmark in the bookmark manager
        BookmarkManager.Instance.ActiveBookmark = this;
        
        if (LoadScene                                           // If this bookmark specifies that we should load its scene
            && SceneManager.GetActiveScene().name != SceneName  // and we're not currently in that scene
            && SceneManager.GetSceneByName(SceneName) != null)  // and the scene exists
        {
            // Load the bookmark's scene
            SceneManager.LoadScene(SceneName);
        }

        // Set whether the character controller should be in fly mode, but don't update the colliders
        CharacterController.SetFlyMode(FlyMode, false);

        CharacterController.ActiveIndicator = ActiveIndicator;
        if (!stopAnim)
        {
            // Move the flat character controller to this bookmark.
            CharacterController.MoveToTarget(SavedPosition, SavedRotationEulerAngles, Speed, ViewerHeight);
        }
        else
        {
            //Set character controller to this bookmark.(No transition)
            CharacterController.MoveToTarget(SavedPosition, SavedRotationEulerAngles, 0, ViewerHeight);
        }
    }

    /// <summary>
    /// Toggle a visual indicator showing whether this is the current active bookmark or not.
    /// </summary>
    /// <param name="active">True if this bookmark should be shown as active, false otherwise.</param>
    public void IndicateActiveBookmark(bool active)
    {
        if (ActiveIndicator != null)
        {
            BookmarkManager.Instance.StartCoroutine(ActiveIndicator.IndicateActive(active));
        }
    }

    public void ToggleLoadSceneProperty()
    {
        SetLoadSceneProperty(!LoadScene);
    }

    public void SetLoadSceneProperty(bool loadScene)
    {
        _loadScene = loadScene;
        LoadSceneIndicator.IndicateActive(LoadScene);
    }
}
