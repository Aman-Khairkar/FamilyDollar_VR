using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BookmarkManager : MonoBehaviour
{
    private static BookmarkManager instance = null;
    public static BookmarkManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BookmarkManager>();
            }

            return instance;
        }
    }

    private static string bookmarkFileName = "Bookmarks.json";
    public static string BookmarkFilePath
    {
        get
        {
            return Path.Combine(Bookmark.BookmarkDataFolderPath, bookmarkFileName);
        }
    }

    private bool editMode = false;
    public bool EditMode
    {
        get
        {
            return editMode;
        }
    }

    public List<Bookmark> Bookmarks = new List<Bookmark>();
    static int NextBookmarkNumber = 1;

    public GameObject BookmarkItemPrefab;
    public Transform BookmarkItemsParent;
    public Transform CanvasTransform;
    public Toggle BookmarkEditModeToggle;
    public GameObject AddBookmarkButton;

    public Bookmark ActiveBookmark;

    private List<string> oldBookmarkData = new List<string>();

    //store current running coroutine to stop when needed.
    //Stopping globally is not stopping coroutine properly.
    public IEnumerator RunningCoroutine = null;

    private float Autoplay_bookmarkWaitTime = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        if (BookmarkEditModeToggle != null)
        {
            BookmarkEditModeToggle.isOn = editMode;
        }

        // Make sure the bookmarks can access the flat character controller
        // Needed in case the user uses keyboard shortcuts before opening the bookmark menu
        if (Bookmark.CharacterController == null)
        {
            Bookmark.CharacterController = FindObjectOfType<FlatPlayerController>();
        }
        if (Bookmarks.Count > 0)
        {
            //Invoking this afer a frame to make sure all the references and event subscriptions done.
            Invoke("SetCharacterToDefaultBookmark", Time.deltaTime);
        }
    }

    internal void SetCharacterToDefaultBookmark()
    {
        Bookmarks[0].ActivateBookmark(stopAnim: true);
    }

    // Update is called once per frame
    void Update()
    {
        // toggle bookmark edit mode using shortcut key.
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleBookmarkEditMode();
        }

        // Switch between bookmarks with Ctrl + Up/Down
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ActivatePreviousBookmark();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ActivateNextBookmark();
            }
        }
        //Run all bookmarks
        if (Input.GetKeyDown(KeyCode.R))
        {
            RunningCoroutine = ActivateSequenceofBookmarks();
            StartCoroutine(RunningCoroutine);
        }
        //Stop current bookmark transition
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (RunningCoroutine != null)
            {
                StopCoroutine(RunningCoroutine);
            }

            StopCurrentTransition();
        }
    }

    private void OnEnable()
    {
        ReadBookmarksFromFile();
    }

    private void OnDestroy()
    {
        WriteBookmarksToFile();
    }

    public void CreateNewBookmark()
    {
        // Create the new bookmark and place it in the menu
        Bookmark bookmarkScript = InstantiateBookmark();

        // Find a unique name for the bookmark
        string bookmarkName;
        do
        {
            bookmarkName = "Bookmark " + (NextBookmarkNumber++);
        } while (Bookmarks.Where(x => x.BookmarkName == bookmarkName).ToArray().Length > 0);

        // Set the new bookmark's name and thumbnail image
        bookmarkScript.SetBookmarkName(bookmarkName);
        bookmarkScript.SetBookmarkToCurrentCameraView();
    }

    public void LoadBookmark(string jsonData)
    {
        // Create an entry for the loaded bookmark and place it in the menu
        Bookmark bookmarkScript = InstantiateBookmark();

        // Load in the bookmark's data
        bookmarkScript.LoadFromString(jsonData);
    }

    private Bookmark InstantiateBookmark()
    {
        int newChildIndex = Mathf.Max(0, BookmarkItemsParent.childCount - 1);
        // Create the new bookmark and place it in the menu
        GameObject newBookmarkItem = GameObject.Instantiate(BookmarkItemPrefab, BookmarkItemsParent);
        // Put it at the end of the list, but before the + button
        newBookmarkItem.transform.SetSiblingIndex(newChildIndex);

        // Add the bookmark to the master list
        Bookmark bookmarkScript = newBookmarkItem.GetComponent<Bookmark>();
        Bookmarks.Add(bookmarkScript);

        return bookmarkScript;
    }

    public void DeleteBookmark(Bookmark bookmarkToDelete)
    {
        // Remove this bookmark from our tracking list
        Bookmarks.Remove(bookmarkToDelete);
        // Clean up the thumbnail file
        bookmarkToDelete.DeleteThumbnailFile();
        // Destroy the bookmark entry in the legend UI
        Destroy(bookmarkToDelete.gameObject);
    }

    /// <summary>
    /// Toggle bookmark edit mode
    /// </summary>
    public void ToggleBookmarkEditMode()
    {
        SetBookmarkEditMode(!editMode);
    }

    /// <summary>
    /// Set whether we are in edit mode based on whether the UI toggle is on or off.
    /// Called from the UI toggle's On Value Changed event.
    /// </summary>
    public void SetBookmarkEditModeToMatchToggle()
    {
        if (BookmarkEditModeToggle != null)
        {
            SetBookmarkEditMode(BookmarkEditModeToggle.isOn);
        }
    }

    /// <summary>
    /// Turn edit mode on/off as specified.
    /// </summary>
    /// <param name="isOn">True if edit mode should be on, false otherwise.</param>
    public void SetBookmarkEditMode(bool isOn)
    {
        bool oldEditMode = editMode;
        editMode = isOn;

        // If the edit mode actually changed
        if (editMode != oldEditMode)
        {
            // If edit mode was turned on
            if (editMode)
            {
                // Reload the JSON data
                ReadBookmarksFromFile(true);
            }
            else // If edit mode was turned off
            {
                // Save the current data to JSON
                WriteBookmarksToFile();
            }
        }

        // If the toggle exists and isn't already the correct value
        // (Note that if the toggle is already the correct value and we set it anyway, this method 
        //  will get called again infinitely and trigger a StackOverflowException.)
        if (BookmarkEditModeToggle != null
            && BookmarkEditModeToggle.isOn != editMode)
        {
            // Set the toggle value to reflect whether we are in edit mode
            BookmarkEditModeToggle.isOn = editMode;
        }

        // Show the add button if edit mode is on, otherwise hide it
        AddBookmarkButton.SetActive(EditMode);

        // Show/hide the delete buttons according to whether we're in edit mode
        foreach (Bookmark bookmark in Bookmarks)
        {
            bookmark.UpdateForCurrentEditMode();
        }
    }

    /// <summary>
    /// Save the runtime bookmarks to JSON.
    /// </summary>
    public void WriteBookmarksToFile()
    {
        // Build a list containing the bookmark data
        oldBookmarkData.Clear();
        for (int b = 0; b < Bookmarks.Count; b++)
        {
            oldBookmarkData.Add(Bookmarks[b].SaveToString());
        }

        // Write the bookmark data to a JSON file
        using (StreamWriter writer = new StreamWriter(BookmarkFilePath))
        {
            foreach (string line in oldBookmarkData)
            {
                writer.WriteLine(line);
            }
        }
    }

    /// <summary>
    /// Load saved bookmarks into the game.
    /// </summary>
    public void ReadBookmarksFromFile(bool reload = false)
    {
        // If there's a JSON file containing bookmark save data
        if (File.Exists(BookmarkFilePath))
        {
            // Read the file data into a list of strings
            List<string> newBookmarkData = new List<string>();
            using (StreamReader reader = new StreamReader(BookmarkFilePath))
            {
                while (!reader.EndOfStream)
                {
                    newBookmarkData.Add(reader.ReadLine());
                }
            }

            // If the new bookmark data is different from what we had before
            // (prevents reload if the JSON data is all the same)
            if (HasBookmarkDataChanged(newBookmarkData))
            {
                // Clear out the old bookmarks
                if (reload)
                {
                    ClearAllBookmarks();
                }

                // Load in each of the saved bookmarks
                foreach (string line in newBookmarkData)
                {
                    LoadBookmark(line);
                }

                // Update our bookmarkData list
                oldBookmarkData = newBookmarkData;
            }

        }
    }

    public void ActivatePreviousBookmark()
    {
        // Get the index of the current active bookmark
        int activeIndex = Bookmarks.IndexOf(ActiveBookmark);
        Bookmark newActiveBookmark = null;

        // If this is a bookmark still in the list
        if (activeIndex != -1)
        {
            // Go to the previous bookmark, and wrap around to the other side of the list if the index goes out of bounds
            activeIndex = (activeIndex - 1 + Bookmarks.Count) % Bookmarks.Count;
            newActiveBookmark = Bookmarks[activeIndex];
        }
        else if (Bookmarks.Count > 0) // Or if the bookmark wasn't in the list, but there are bookmarks
        {
            // Activate the last bookmark in the list
            newActiveBookmark = Bookmarks[Bookmarks.Count - 1];
        }

        if (newActiveBookmark != null)
        {
            newActiveBookmark.ActivateBookmark();
        }
    }

    public void ActivateNextBookmark()
    {
        // Get the index of the current active bookmark
        int activeIndex = Bookmarks.IndexOf(ActiveBookmark);
        Bookmark newActiveBookmark = null;

        // If this is a bookmark still in the list
        if (activeIndex != -1)
        {
            // Go to the next bookmark, and wrap around to the other side of the list if the index goes out of bounds
            activeIndex = (activeIndex + 1) % Bookmarks.Count;
            newActiveBookmark = Bookmarks[activeIndex];
        }
        else if (Bookmarks.Count > 0) // Or if the bookmark wasn't in the list, but there are bookmarks
        {
            // Activate the last bookmark in the list
            newActiveBookmark = Bookmarks[0];
        }

        if (newActiveBookmark != null)
        {
            newActiveBookmark.ActivateBookmark();
        }
    }

    /// <summary>
    /// Change the position of the specified bookmark in the bookmark master list.
    /// Will also update the bookmark's position in the UI list, unless otherwise specified.
    /// </summary>
    /// <param name="bookmark">The bookmark to move</param>
    /// <param name="newIndex">The new index where the bookmark should be inserted</param>
    /// <param name="updateUIPosition">True if the bookmark should be </param>
    public void MoveBookmark(Bookmark bookmark, int newIndex, bool updateUIPosition = true)
    {
        // Find where the bookmark used to be in the list
        int oldIndex = Bookmarks.IndexOf(bookmark);
        int adjustedNewIndex = newIndex;

        // If the bookmark was found in the list
        if (oldIndex > -1)
        {
            // Remove the bookmark from the list
            Bookmarks.RemoveAt(oldIndex);

            // If we're moving the item to somewhere later in the list
            if (newIndex > oldIndex)
            {
                // Account for having removed the bookmark from earlier in the list
                adjustedNewIndex--;
            }
        }

        // If the new index is too high to be valid
        if (newIndex > Bookmarks.Count)
        {
            // Just drop this bookmark at the end of the list
            Bookmarks.Add(bookmark);
        }
        else
        {
            // Insert the bookmark back into the list at the proper place
            Bookmarks.Insert(newIndex, bookmark);
        }

        // If we want to update the UI position as well
        if (updateUIPosition)
        {
            ReturnBookmarkToMenu(bookmark);
        }
    }

    /// <summary>
    /// Put the bookmark back into the list in the correct spot after dragging it.
    /// </summary>
    /// <param name="bookmark">Bookmark to insert back into the UI list</param>
    public void ReturnBookmarkToMenu(Bookmark bookmark)
    {
        // Place the bookmark in the menu
        bookmark.transform.SetParent(BookmarkItemsParent);

        // Find the proper index of the bookmark
        int index = Bookmarks.IndexOf(bookmark);

        // If it wasn't in the list
        if (index < 0)
        {
            // Then add it to the list at the end
            Bookmarks.Add(bookmark);
            index = Bookmarks.Count - 1;
        }

        // Put it at the end of the list, but before the + button
        bookmark.transform.SetSiblingIndex(index);
    }

    public void ClearAllBookmarks()
    {
        // Clear any existing bookmarks
        while (Bookmarks.Count > 0)
        {
            Destroy(Bookmarks[0].gameObject);
            Bookmarks.RemoveAt(0);
        }

        // Clear out the reference to the active bookmark
        ActiveBookmark = null;
    }

    private bool HasBookmarkDataChanged(List<string> newBookmarkData)
    {
        bool hasChanged = false;
        // If the new data doesn't have the same number of lines as the old data
        if (newBookmarkData.Count != oldBookmarkData.Count)
        {
            // then this data is definitely different
            hasChanged = true;
        }
        else
        {
            // Check each string in the new list
            for (int i = 0; i < newBookmarkData.Count; i++)
            {
                // If this line doesn't match the corresponding line in the old data
                if (newBookmarkData[i] != oldBookmarkData[i])
                {
                    // Then it's different
                    hasChanged = true;
                    break;
                }
            }
        }
        return hasChanged;
    }


    internal IEnumerator ActivateSequenceofBookmarks()
    {
        // Get the index of the current active bookmark
        int activeIndex = Bookmarks.IndexOf(ActiveBookmark);
        Bookmark newActiveBookmark = null;

        //If the active bookmark transition is not complted,start with that.
        if ((ActiveBookmark != null) && !ActiveBookmark.ActiveIndicator.IsTransitionComplete)
        {
            newActiveBookmark = ActiveBookmark;
        }
        // Go to the next bookmark
        else if (activeIndex != -1 && (activeIndex < Bookmarks.Count - 1))
        {
            activeIndex = (activeIndex + 1);
            newActiveBookmark = Bookmarks[activeIndex];
        }
        else if (Bookmarks.Count > 0) // Or if the bookmark wasn't in the list, but there are bookmarks
        {
            // Activate the last bookmark in the list
            newActiveBookmark = Bookmarks[activeIndex];
        }

        if (newActiveBookmark != null)
        {
            newActiveBookmark.ActivateBookmark();
        }

        //activate bookmarks untill it reaches to last bookmark in the list.
        if (activeIndex < Bookmarks.Count - 1)
        {
            yield return new WaitUntil(() => newActiveBookmark.ActiveIndicator.IsTransitionComplete);
            yield return new WaitForSeconds(Autoplay_bookmarkWaitTime);
            RunningCoroutine = ActivateSequenceofBookmarks();

            StartCoroutine(RunningCoroutine);
        }
    }
    internal void StopCurrentTransition()
    {
        FlatPlayerController.interruptCurrentAnimation = true;
    }
}
