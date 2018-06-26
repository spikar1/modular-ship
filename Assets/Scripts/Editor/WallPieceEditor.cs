using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(WallPiece))]
public class WallPieceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var wallPiece = (WallPiece) target;
        if (GUILayout.Button("Find Meshes"))
        {
            if (wallPiece.wallBlenderFile == null)
            {
                Debug.LogError("Need to have Blender File Attached");
                return;
            }

            GameObject wallBlenderFile = wallPiece.wallBlenderFile;
            MeshFilter[] mesheFilters = wallBlenderFile.GetComponentsInChildren<MeshFilter>();

            var meshLists = new[]
            {
                wallPiece.corner, wallPiece.corner90, wallPiece.corner180, wallPiece.corner270,
                wallPiece.cornerCol, wallPiece.corner90Col, wallPiece.corner180Col, wallPiece.corner270Col, 
                wallPiece.diagonal, wallPiece.diagonal90, wallPiece.diagonal180, wallPiece.diagonal270,
                wallPiece.diagonalCol, wallPiece.diagonal90Col, wallPiece.diagonal180Col, wallPiece.diagonal270Col,
                wallPiece.straight, wallPiece.straight90, wallPiece.straight180, wallPiece.straight270,
                wallPiece.straightCol, wallPiece.straight90Col, wallPiece.straight180Col, wallPiece.straight270Col
            };
            foreach (var meshList in meshLists)
            {
                meshList.Clear();
            }
            //Delete old generated meshes
            var assetPath = AssetDatabase.GetAssetPath(target);
            var allObjectsAtPath = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            foreach (var o in allObjectsAtPath)
            {
                if (o is Mesh)
                    DestroyImmediate(o, true);
            }

            for (int i = 0; i < mesheFilters.Length; i++)
            {
                Mesh mesh = mesheFilters[i].sharedMesh;

                if (mesh.name.StartsWith("corner"))
                {
                    if (mesh.name.EndsWith("col"))
                        AddMeshVariants(mesh, wallPiece.cornerCol, wallPiece.corner90Col, wallPiece.corner180Col, wallPiece.corner270Col);
                    else
                        AddMeshVariants(mesh, wallPiece.corner, wallPiece.corner90, wallPiece.corner180, wallPiece.corner270);
                }

                if (mesh.name.StartsWith("straight"))
                {
                    if (mesh.name.EndsWith("col"))
                        AddMeshVariants(mesh, wallPiece.straightCol, wallPiece.straight90Col, wallPiece.straight180Col, wallPiece.straight270Col);
                    else
                        AddMeshVariants(mesh, wallPiece.straight, wallPiece.straight90, wallPiece.straight180, wallPiece.straight270);
                }

                if (mesh.name.StartsWith("diagonal"))
                {
                    if (mesh.name.EndsWith("col"))
                        AddMeshVariants(mesh, wallPiece.diagonalCol, wallPiece.diagonal90Col, wallPiece.diagonal180Col, wallPiece.diagonal270Col);
                    else
                        AddMeshVariants(mesh, wallPiece.diagonal, wallPiece.diagonal90, wallPiece.diagonal180, wallPiece.diagonal270);
                }
            }

            EditorUtility.SetDirty(wallPiece);
            AssetDatabase.ImportAsset(assetPath);
        }
    }

    private void AddMeshVariants(Mesh mesh, List<Mesh> meshes, List<Mesh> meshes90, List<Mesh> meshes180, List<Mesh> meshes270)
    {
        var meshRotated0 = mesh.Copy(mesh.name + "_00");
        var meshRotated90 = mesh.Copy(mesh.name + "_90");
        var meshRotated180 = mesh.Copy(mesh.name + "_180");
        var meshRotated270 = mesh.Copy(mesh.name + "_270");

        meshRotated90.RotateVertices(Quaternion.Euler(0f, 0f, 90f));
        meshRotated180.RotateVertices(Quaternion.Euler(0f, 0f, 180f));
        meshRotated270.RotateVertices(Quaternion.Euler(0f, 0f, 270f));

        meshes.Add(meshRotated0);
        meshes90.Add(meshRotated90);
        meshes180.Add(meshRotated180);
        meshes270.Add(meshRotated270);

        var assetPath = AssetDatabase.GetAssetPath(target);

        AssetDatabase.AddObjectToAsset(meshRotated0, assetPath);
        AssetDatabase.AddObjectToAsset(meshRotated90, assetPath);
        AssetDatabase.AddObjectToAsset(meshRotated180, assetPath);
        AssetDatabase.AddObjectToAsset(meshRotated270, assetPath);
    }
}