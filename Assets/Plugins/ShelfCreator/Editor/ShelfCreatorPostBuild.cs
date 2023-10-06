using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Threading;
using System.Collections.Generic;

public class ShelfCreatorPostBuild
{
    [PostProcessBuildAttribute(3)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltExe)
    {
        // Path to the text file we're writing the plugin version numbers to
        string pathToVersionsFile = Path.GetFullPath(Path.Combine(pathToBuiltExe, @"..\PluginVersions.txt"));

        // If the version file doesn't exist yet
        if (!File.Exists(pathToVersionsFile))
        {
            // Create it
            File.Create(pathToVersionsFile);

            // Wait for a bit to give the file time to become available
            for (int i = 0; i < 1000; i++)
            {
                Thread.Sleep(5);
            }
        }

        // Wait until we're allowed read access to the file
        using (StreamReader reader = WaitForFileRead(pathToVersionsFile))
        {
            if (reader != null)
            {
                // Read all of the existing lines of the file and add them to our List
                List<string> allLines = new List<string>();
                while (!reader.EndOfStream)
                {
                    allLines.Add(reader.ReadLine());
                }
                reader.Close();

                // Wait until we're allowed write access to the file
                using (StreamWriter writer = WaitForFileWrite(pathToVersionsFile))
                {
                    if (writer != null)
                    {
                        // Add our version number to the file (or update it if it's already listed there)
                        WriteOrUpdateVersion(allLines, writer, CreateShelfEditorWindow.pluginName, CreateShelfEditorWindow.versionNumber);
                        writer.Close();
                    }
                    else
                    {
                        Debug.Log("Unable to write " + CreateShelfEditorWindow.pluginName + " version number to file.");
                    }
                }
            }
            else
            {
                Debug.Log("Unable to write " + CreateShelfEditorWindow.pluginName + " version number to file.");
            }
        }
    }

    static void WriteOrUpdateVersion(List<string> lines, StreamWriter writer, string pluginName, string version)
    {
        // What our plugin's version entry would start with
        string versionLineStart = pluginName + " - ";

        bool lineAlreadyExisted = false;

        // Check each of the lines that were already in the file
        foreach (string line in lines)
        {
            string currentLine = line;

            // If this line lists the version of our plugin
            if (currentLine.StartsWith(versionLineStart))
            {
                // Update the existing line, and then note that we did this
                currentLine = versionLineStart + version;
                lineAlreadyExisted = true;
            }

            // Write this line out to file
            writer.WriteLine(currentLine);
        }

        // If we didn't already find our plugin listed in the file
        if (!lineAlreadyExisted)
        {
            // Add a new line for it
            writer.WriteLine(versionLineStart + version);
        }
    }

    /// <summary>
    /// Waits until the file is available to read from (or until timeout if the file never becomes available),
    ///  opens a StreamReader for the file, and returns it.
    /// Based on code from StackOverflow user "mafu":
    ///   https://stackoverflow.com/questions/50744/wait-until-file-is-unlocked-in-net
    /// </summary>
    /// <param name="fullPath">The path to the file we want to read from.</param>
    /// <returns>An open StreamReader for the file at <param name="fullPath">.</returns>
    static StreamReader WaitForFileRead(string fullPath)
    {
        for (int numTries = 0; numTries < 10000; numTries++)
        {
            StreamReader fs = null;
            try
            {
                fs = new StreamReader(fullPath);
                return fs;
            }
            catch (IOException)
            {
                if (fs != null)
                {
                    fs.Dispose();
                }
                Thread.Sleep(10);
            }
        }

        return null;
    }

    /// <summary>
    /// Waits until the file is available to write to (or until timeout if the file never becomes available),
    ///   opens a StreamWriter for the file, and returns it.
    /// Based on code from StackOverflow user "mafu":
    ///   https://stackoverflow.com/questions/50744/wait-until-file-is-unlocked-in-net
    /// </summary>
    /// <param name="fullPath">The path to the file we want to write to.</param>
    /// <returns>An open StreamWriter for the file at <param name="fullPath">.</returns>
    static StreamWriter WaitForFileWrite(string fullPath)
    {
        for (int numTries = 0; numTries < 10000; numTries++)
        {
            StreamWriter fs = null;
            try
            {
                fs = new StreamWriter(fullPath, false);
                return fs;
            }
            catch (IOException)
            {
                if (fs != null)
                {
                    fs.Dispose();
                }
                Thread.Sleep(10);
            }
        }

        return null;
    }
}
