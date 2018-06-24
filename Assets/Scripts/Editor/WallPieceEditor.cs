using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(WallPiece))]
public class WallPieceEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        WallPiece wallPiece = (WallPiece)target;
        if (GUILayout.Button("Find Meshes")) {
            if (wallPiece.wallBlenderFile == null)
                Debug.LogError("Need to have Blender File Attached");

            GameObject wallBlenderFile = wallPiece.wallBlenderFile;
            MeshFilter[] mesheFilters = wallBlenderFile.GetComponentsInChildren<MeshFilter>();

            wallPiece.corner.Clear();
            wallPiece.straight.Clear();
            wallPiece.diagonal.Clear();

            for (int i = 0; i < mesheFilters.Length; i++) {
                Mesh mesh = mesheFilters[i].sharedMesh;
                Debug.Log("found " + mesheFilters[i].name);

                if (mesh.name.StartsWith("corner")) {
                    if (mesh.name.EndsWith("col"))
                        wallPiece.cornerCol.Add(mesh);
                    else
                        wallPiece.corner.Add(mesh);
                }
                if (mesh.name.StartsWith("straight")) {
                    if (mesh.name.EndsWith("col"))
                        wallPiece.straightCol.Add(mesh);
                    else
                        wallPiece.straight.Add(mesh);
                }
                if (mesh.name.StartsWith("diagonal")) {
                    if (mesh.name.EndsWith("col"))
                        wallPiece.diagonalCol.Add(mesh);
                    else
                        wallPiece.diagonal.Add(mesh);
                }
            }

        }
        if (GUI.changed)
            EditorUtility.SetDirty(wallPiece);
    }

}
