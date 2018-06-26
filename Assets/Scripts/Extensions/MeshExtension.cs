﻿using UnityEngine;

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