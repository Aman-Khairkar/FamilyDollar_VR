using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class ShelfCreatorSymbolAdd
{
    static ShelfCreatorSymbolAdd()
    {
        // The symbols we want to add
        string[] symbolsToAdd = { "ALLOW_SHELF_BUILD_IN_EDITOR" };
        // The symbols that are already defined in this project
        string currentScriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);

        // Start by including all of the symbols that already exist
        string newScriptingDefineSymbols = currentScriptingDefineSymbols;
        // Go through each of the symbols we want in our project
        foreach (string symbol in symbolsToAdd)
        {
            // If this symbol isn't onthe project already
            if (!currentScriptingDefineSymbols.Contains(symbol))
            {
                // If there are other symbols already
                if (newScriptingDefineSymbols.Length > 0)
                {
                    // We need to separate them with a semicolon
                    newScriptingDefineSymbols += ";";
                }
                // Add the symbol
                newScriptingDefineSymbols += symbol;
            }
        }

        // Update the project with the new symbols
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, newScriptingDefineSymbols);
    }
}
