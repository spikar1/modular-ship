using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RotateMesh))]
public class RotateMeshEditor : Editor
{
    private MeshFilter meshFilter;
    private Mesh orgMesh;
    private void OnEnable()
    {
        var rotateMesh = (RotateMesh) target;
        meshFilter = rotateMesh.GetComponent<MeshFilter>();
        if (rotateMesh.orgMesh == null)
        {
            rotateMesh.orgMesh = orgMesh = meshFilter.sharedMesh;
        }
        else
        {
            orgMesh = rotateMesh.orgMesh;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("90f"))
        {
            var copy = orgMesh.Copy();
            copy.RotateVertices(Quaternion.Euler(0f, 0f, 90f));
            meshFilter.sharedMesh = copy;
        }

        if (GUILayout.Button("180f"))
        {
            var copy = orgMesh.Copy();
            copy.RotateVertices(Quaternion.Euler(0f, 0f, 180f));
            meshFilter.sharedMesh = copy;
        }

        if (GUILayout.Button("270f"))
        {
            var copy = orgMesh.Copy();
            copy.RotateVertices(Quaternion.Euler(0f, 0f, 270f));
            meshFilter.sharedMesh = copy;
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("restore"))
        {
            meshFilter.sharedMesh = orgMesh;
        }
    }
}