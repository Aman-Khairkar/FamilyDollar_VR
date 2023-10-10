using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookmarkDropZone : MonoBehaviour
{
    public Bookmark thisBookmark;

    // Start is called before the first frame update
    void Start()
    {
        // Note that this script is attached to some things that are not bookmarks, so thisBookmark may still be null after this
        if (thisBookmark == null)
        {
            thisBookmark = GetComponent<Bookmark>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Handle when the user drops something onto this bookmark.
    /// </summary>
    public void OnDrop()
    {
        Bookmark draggedBookmark = null;
        foreach (Bookmark bookmark in BookmarkManager.Instance.Bookmarks)
        {
            if (bookmark.Dragging)
            {
                draggedBookmark = bookmark;
                break;
            }
        }

        if (draggedBookmark != null)
        {
            // Default to placing the bookmark at the end of the list
            int newIndex = BookmarkManager.Instance.Bookmarks.Count;

            // If this drop zone also corresponds to a bookmark
            if (thisBookmark != null)
            {
                // We'll place the bookmark in the list directly before this one
                newIndex = BookmarkManager.Instance.Bookmarks.IndexOf(thisBookmark);
            }

            // If the new index is somewhere in the list
            if (newIndex > -1)
            {
                // Move bookmark to its new position
                BookmarkManager.Instance.MoveBookmark(draggedBookmark, newIndex);
            }

            // Mark that we're no longer dragging
            draggedBookmark.Dragging = false;
        }
    }
}
