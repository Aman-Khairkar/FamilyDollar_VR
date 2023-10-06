using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVManager : MonoBehaviour{
    /// <summary>
    /// Set the UVs for each side of a perpendicular POSM so that each side's image has the correct orientation.
    /// </summary>
    public static void SetPerpendicularPosmUVs(GameObject posmObject)
    {
        // Get the mesh so that we can set its uv coordinates
        Mesh posmCubeMesh = null;
        {
            MeshFilter meshFilter = posmObject.transform.GetComponentInChildren<MeshFilter>();
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
            posmCubeMesh = meshFilter.sharedMesh;
#else
            posmCubeMesh = meshFilter.mesh;
#endif
        }

        // If a mesh was not found, then don't continue
        if (posmCubeMesh == null)
        {
            return;
        }

        // Make an array which we'll put the new UV values into
        Vector2[] uvValues = new Vector2[posmCubeMesh.uv.Length];
        uvValues = posmCubeMesh.uv;

        // Front
        uvValues[0] = new Vector2(0.0f, 1.0f);
        uvValues[1] = new Vector2(0.5f, 1.0f);
        uvValues[2] = new Vector2(0.5f, 0.0f);
        uvValues[3] = new Vector2(0.0f, 0.0f);

        // Back
        uvValues[4] = new Vector2(1.0f, 1.0f);
        uvValues[5] = new Vector2(1.0f, 0.0f);
        uvValues[6] = new Vector2(0.5f, 0.0f);
        uvValues[7] = new Vector2(0.5f, 1.0f);

        // Left
        uvValues[8] = new Vector2(0.0f, 1.0f);
        uvValues[9] = new Vector2(0.0f, 0.0f);
        uvValues[10] = new Vector2(0.0f, 0.0f);
        uvValues[11] = new Vector2(0.0f, 1.0f);

        // Bottom
        uvValues[12] = new Vector2(0.0f, 0.0f);
        uvValues[13] = new Vector2(0.5f, 0.0f);
        uvValues[14] = new Vector2(0.5f, 0.0f);
        uvValues[15] = new Vector2(0.0f, 0.0f);

        // Right
        uvValues[16] = new Vector2(0.5f, 0.0f);
        uvValues[17] = new Vector2(0.5f, 1.0f);
        uvValues[18] = new Vector2(0.5f, 1.0f);
        uvValues[19] = new Vector2(0.5f, 0.0f);

        // Top        
        uvValues[20] = new Vector2(0.0f, 1.0f);
        uvValues[21] = new Vector2(0.5f, 1.0f);
        uvValues[22] = new Vector2(0.5f, 1.0f);
        uvValues[23] = new Vector2(0.0f, 1.0f);

        // Give our UV values back to the mesh
        posmCubeMesh.uv = uvValues;
    }

    public void SetPerpendicularPosmUVsOnAdd(GameObject posmObject)
    {
        SetPerpendicularPosmUVs(posmObject);
    }

    /// <summary>
    /// Set the UVs for each side of a header POSM so that each side's image has the correct orientation.
    /// </summary>
    public static void SetHeaderPosmUVs(GameObject posmObject)
    {
        // Get the mesh so that we can set its uv coordinates
        Mesh posmCubeMesh = null;
        {
            MeshFilter meshFilter = posmObject.transform.GetComponentInChildren<MeshFilter>();
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
            posmCubeMesh = meshFilter.sharedMesh;
#else
            posmCubeMesh = meshFilter.mesh;
#endif
        }

        // If a mesh was not found, then don't continue
        if (posmCubeMesh == null)
        {
            return;
        }

        // Make an array which we'll put the new UV values into
        Vector2[] uvValues = new Vector2[posmCubeMesh.uv.Length];
        uvValues = posmCubeMesh.uv;

        // Front
        uvValues[0] = new Vector2(0.0f, 0.5f);
        uvValues[1] = new Vector2(1f, 0.5f);
        uvValues[2] = new Vector2(1f, 0.0f);
        uvValues[3] = new Vector2(0.0f, 0.0f);

        // Back
        uvValues[4] = new Vector2(1.0f, 1.0f);
        uvValues[5] = new Vector2(1.0f, 0.5f);
        uvValues[6] = new Vector2(0.0f, 0.5f);
        uvValues[7] = new Vector2(0.0f, 1.0f);

        // Left
        uvValues[8] = new Vector2(0.0f, 0.5f);
        uvValues[9] = new Vector2(0.0f, 0.0f);
        uvValues[10] = new Vector2(0.0f, 0.0f);
        uvValues[11] = new Vector2(0.0f, 0.5f);

        // Bottom
        uvValues[12] = new Vector2(0.0f, 0.0f);
        uvValues[13] = new Vector2(1.0f, 0.0f);
        uvValues[14] = new Vector2(1.0f, 0.0f);
        uvValues[15] = new Vector2(0.0f, 0.0f);

        // Right
        uvValues[16] = new Vector2(1.0f, 0.0f);
        uvValues[17] = new Vector2(1.0f, 0.5f);
        uvValues[18] = new Vector2(1.0f, 0.5f);
        uvValues[19] = new Vector2(1.0f, 0.0f);

        // Top        
        uvValues[20] = new Vector2(0.0f, 0.5f);
        uvValues[21] = new Vector2(1.0f, 0.5f);
        uvValues[22] = new Vector2(1.0f, 0.5f);
        uvValues[23] = new Vector2(0.0f, 0.5f);

        // Give our UV values back to the mesh
        posmCubeMesh.uv = uvValues;
    }

    /// <summary>
    /// Set the UVs for each side of a custom POSM so that each side's image has the correct orientation.
    /// </summary>
    public static void SetCustomPosmUVs(GameObject posmObject)
    {
        // Get the mesh so that we can set its uv coordinates
        Mesh posmCubeMesh = null;
        {
            MeshFilter meshFilter = posmObject.transform.GetComponentInChildren<MeshFilter>();
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
            posmCubeMesh = meshFilter.sharedMesh;
#else
            posmCubeMesh = meshFilter.mesh;
#endif
        }

        // If a mesh was not found, then don't continue
        if (posmCubeMesh == null)
        {
            return;
        }

        // Make an array which we'll put the new UV values into
        Vector2[] uvValues = new Vector2[posmCubeMesh.uv.Length];
        uvValues = posmCubeMesh.uv;

        // Front
        uvValues[0] = new Vector2(0.0f, 1f);
        uvValues[1] = new Vector2(1f, 1f);
        uvValues[2] = new Vector2(1f, 0.0f);
        uvValues[3] = new Vector2(0.0f, 0.0f);

        // Give our UV values back to the mesh
        posmCubeMesh.uv = uvValues;
    }

    public void SetCustomPosmUVsOnAdd(GameObject posmObject)
    {
        SetCustomPosmUVs(posmObject);
    }
}
