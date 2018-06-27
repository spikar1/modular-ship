using System.Collections.Generic;
using UnityEngine;

public static class MeshExtension
{
    public static Mesh Copy(this Mesh mesh, string newName = null)
    {
        return new Mesh
        {
            name = newName ?? mesh.name,
            vertices = mesh.vertices,
            triangles = mesh.triangles,
            bindposes = mesh.bindposes,
            boneWeights = mesh.boneWeights,
            colors = mesh.colors,
            normals = mesh.normals,
            tangents = mesh.tangents,
            uv = mesh.uv,
            uv2 = mesh.uv2,
            uv3 = mesh.uv3,
            uv4 = mesh.uv4
        };
    }

    public static void OverwriteWith(this Mesh mesh, Mesh otherMesh)
    {
        mesh.vertices = otherMesh.vertices;
        mesh.triangles = otherMesh.triangles;
        mesh.bindposes = otherMesh.bindposes;
        mesh.boneWeights = otherMesh.boneWeights;
        mesh.colors = otherMesh.colors;
        mesh.normals = otherMesh.normals;
        mesh.tangents = otherMesh.tangents;
        mesh.uv = otherMesh.uv;
        mesh.uv2 = otherMesh.uv2;
        mesh.uv3 = otherMesh.uv3;
        mesh.uv4 = otherMesh.uv;
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
            if (!string.IsNullOrEmpty(UnityEditor.AssetDatabase.GetAssetPath(mesh)))
            {
                UnityEditor.EditorUtility.SetDirty(mesh);
            }
        }
#endif
    }

    public static void RotateVertices(this Mesh mesh, Quaternion rotation)
    {
        var vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
            vertices[i] = rotation * vertices[i];
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}

public static class Physics2DExtension 
{ 
    public static bool Raycast(this Collider2D collider, Vector2 origin, Vector2 direction, out RaycastHit2D hitInfo, float maxDistance) 
    { 
        var oriLayer = collider.gameObject.layer;
        const int tempLayer = 31;
        const int tempMask = 1 << tempLayer; 
        collider.gameObject.layer = tempLayer; 
        hitInfo = Physics2D.Raycast(origin, direction, maxDistance, tempMask); 
        collider.gameObject.layer = oriLayer; 
        if (hitInfo.collider && hitInfo.collider != collider) 
        { 
            Debug.LogError(
                $"Collider2D.Raycast() need a unique temp layer to work! Make sure Layer #{tempLayer} is unused!"); 
            return false; 
        } 
        return hitInfo.collider != null; 
    } 
}

public static class Physics2DHelper
{
    private static readonly Collider2D[] buffer = new Collider2D[100];
    public static void GetAllNear<T>(Vector2 fromPoint, float radius, int layermask, List<T> results)
    {
        results.Clear();
        var numFound = Physics2D.OverlapCircleNonAlloc(fromPoint, radius, buffer, layermask);
        for (int i = 0; i < numFound; i++)
        {
            var foundObj = buffer[i].GetComponentInParent<T>();
            if (foundObj == null)
                continue;
            results.Add(foundObj);
        }
    }
}