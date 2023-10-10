using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public class FlatCharacterControllerBookmarksPostBuild
{
    [PostProcessBuildAttribute()]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltExe)
    {
        // If there is bookmark data in the project
        if (Directory.Exists(Bookmark.BookmarkDataFolderPath))
        {
            // Figure out where to place the bookmark data in the build
            string bookmarkDataFolderDestination = Path.GetFullPath(Path.Combine(pathToBuiltExe, @"..\" + Application.productName + "_Data", Bookmark.BookmarkDataFolderName));

            // If the bookmark data folder is already there
            if (Directory.Exists(bookmarkDataFolderDestination))
            {
                // Delete it
                FileUtil.DeleteFileOrDirectory(bookmarkDataFolderDestination);
            }

            // Copy the bookmark data folder and its contents to the build's data folder so the build can access it
            FileUtil.CopyFileOrDirectory(Bookmark.BookmarkDataFolderPath, bookmarkDataFolderDestination);
        }
    }
}

