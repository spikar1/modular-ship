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

    public static void RotateVertices(this Mesh mesh, Quaternion rotation)
    {
        var vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
            vertices[i] = rotation * vertices[i];
        mesh.vertices = vertices;
    }
}