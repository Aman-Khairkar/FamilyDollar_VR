using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public static class FlatControllerCheckForExistingScript
{
    /// <summary>
    /// Returns true if the searched class name exists in the project.
    /// </summary>
    /// <param name="className"></param>
    /// <returns></returns>
    public static bool IsValidClass(string className)
    {
        bool hasScripts = false;

        //This tests to see if there is a script named "className" inside of the project. If it is there then we add it to the product, if not then we ignore it and inform the user.
        //following code from this source.
        //https://forum.unity.com/threads/addcomponent-based-on-string-or-alternative-solved.462984/
        var o = (from assembly in System.AppDomain.CurrentDomain.GetAssemblies()
                 from type in assembly.GetTypes()
                 where type.Name == className
                 select type).FirstOrDefault();

        if (o != null)
        {
            hasScripts = true;
        }
        else
        {
            hasScripts = false;
        }

        return hasScripts;
    }
}
