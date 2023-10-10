using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatControllerShelfDependencies {

    private static FlatControllerShelfDependencies instance;
    public static FlatControllerShelfDependencies Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FlatControllerShelfDependencies();
            }
            return instance;
        }
    }

    // If this is still false after the constructor is executed, then we shouldn't try to use any ShelfCreator-related stuff
    bool scenarioIdentifierExists = false;
    public bool ShelfCreatorInProject
    {
        get
        {
            return scenarioIdentifierExists;
        }
    }

    public class TypeNames
    {
        public const string ScenarioIdentifier = "ScenarioIdentifier";
    }
    Dictionary<string, Type> Types = new Dictionary<string, Type>();

    // Use this for initialization
    private FlatControllerShelfDependencies()
    {
        // If we can use ScenarioIdentifier
        scenarioIdentifierExists = FlatControllerExternalDependencies.IsValidType(TypeNames.ScenarioIdentifier);
        if (scenarioIdentifierExists)
        {
            FillTypesDictionary();
        }
    }

    private void FillTypesDictionary()
    {
        Types.Clear();

        // Get the Type for ScenarioIdentifier and add it to the dictionary
        Type scenarioIdentifierType = FlatControllerExternalDependencies.GetType(TypeNames.ScenarioIdentifier);
        Types.Add(TypeNames.ScenarioIdentifier, scenarioIdentifierType);
    }

    /// <summary>
    /// Finds the GameObject of the shelf containing Transform transform and returns it if it exists, or null if it does not exist.
    /// </summary>
    /// <param name="transform">The transform for whom we would like to find the shelf parent's GameObject</param>
    /// <returns>Returns the GameObject that has a ScenarioIdentifier script attached to it and is a parent of transform, if it exists, otherwise returns null</returns>
    public GameObject GetShelfParentIfItExists(Transform transform)
    {
        // If the ShelfCreator resources aren't in the project, just return null
        if (!ShelfCreatorInProject)
        {
            return null;
        }

        GameObject result = null;

        try
        {
            // Check if we can find a ScenarioIdentifier in the parents of this transform
            object scenarioIdentifierScript = transform.GetComponentInParent(Types[TypeNames.ScenarioIdentifier]);
            if (scenarioIdentifierScript != null)
            {
                result = (scenarioIdentifierScript as MonoBehaviour).gameObject;
            }
        }
        catch (Exception)
        {
            Debug.Log("Error with looking for the shelf parent.");
            result = null;
        }

        return result;
    }
}
