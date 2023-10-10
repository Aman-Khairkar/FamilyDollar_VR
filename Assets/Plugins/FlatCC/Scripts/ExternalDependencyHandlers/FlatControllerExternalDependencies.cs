using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatControllerExternalDependencies : MonoBehaviour
{
    public static bool IsValidType(string typeName)
    {
        return FlatControllerCheckForExistingScript.IsValidClass(typeName);
    }

    public static Type GetType(string typeName)
    {
        return Type.GetType(typeName);
    }

    public static System.Reflection.MethodInfo GetMethod(Type containerType, string methodName, Type[] argumentTypes)
    {
        return containerType.GetMethod(methodName, argumentTypes);
    }

    public static object CallMethod(System.Reflection.MethodInfo method, object[] arguments)
    {
        return method.Invoke(null, arguments);
    }

    public static Type GetEnum(Type containerType, string enumName)
    {
        return Type.GetType(containerType.Name + "+" + enumName);
    }

    public static object GetFieldValue(Type containerType, string fieldName)
    {
        // We'll store the result here
        object fieldValue = null;

        // Get all of the fields' information
        System.Reflection.FieldInfo[] fieldsArray = containerType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        // For each field
        foreach (System.Reflection.FieldInfo fInfo in fieldsArray)
        {
            // Find the name of this field
            string enumName = fInfo.Name.ToString(System.Globalization.CultureInfo.InvariantCulture);
            // If it matches the field we're looking for
            if (enumName == fieldName)
            {
                // Store its value in our variable
                fieldValue = fInfo.GetValue(null);
                // And stop looking for it
                break;
            }
        }

        return fieldValue;
    }
}
