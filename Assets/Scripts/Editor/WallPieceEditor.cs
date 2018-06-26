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
            var oldMeshes = new List<Mesh>();
            var stillUsed = new HashSet<Mesh>();
            foreach (var o in allObjectsAtPath)
            {
                var mesh = o as Mesh;
                if (mesh != null)
                    oldMeshes.Add(mesh);
            }

            for (int i = 0; i < mesheFilters.Length; i++)
            {
                Mesh mesh = mesheFilters[i].sharedMesh;

                if (mesh.name.StartsWith("corner"))
                {
                    if (mesh.name.EndsWith("col"))
                        AddMeshVariants(mesh, oldMeshes, stillUsed, wallPiece.cornerCol, wallPiece.corner90Col,
                            wallPiece.corner180Col, wallPiece.corner270Col);
                    else
                        AddMeshVariants(mesh, oldMeshes, stillUsed, wallPiece.corner, wallPiece.corner90,
                            wallPiece.corner180, wallPiece.corner270);
                }

                if (mesh.name.StartsWith("straight"))
                {
                    if (mesh.name.EndsWith("col"))
                        AddMeshVariants(mesh, oldMeshes, stillUsed, wallPiece.straightCol, wallPiece.straight90Col,
                            wallPiece.straight180Col, wallPiece.straight270Col);
                    else
                        AddMeshVariants(mesh, oldMeshes, stillUsed, wallPiece.straight, wallPiece.straight90,
                            wallPiece.straight180, wallPiece.straight270);
                }

                if (mesh.name.StartsWith("diagonal"))
                {
                    if (mesh.name.EndsWith("col"))
                        AddMeshVariants(mesh, oldMeshes, stillUsed, wallPiece.diagonalCol, wallPiece.diagonal90Col,
                            wallPiece.diagonal180Col, wallPiece.diagonal270Col);
                    else
                        AddMeshVariants(mesh, oldMeshes, stillUsed, wallPiece.diagonal, wallPiece.diagonal90,
                            wallPiece.diagonal180, wallPiece.diagonal270);
                }
            }

            foreach (var mesh in oldMeshes)
                if (!stillUsed.Contains(mesh))
                    DestroyImmediate(mesh, true);

            EditorUtility.SetDirty(wallPiece);
            AssetDatabase.ImportAsset(assetPath);
        }
    }

    private void AddMeshVariants(Mesh mesh, List<Mesh> oldMeshes, HashSet<Mesh> stillUsedOldMeshes, List<Mesh> meshes,
        List<Mesh> meshes90, List<Mesh> meshes180,
        List<Mesh> meshes270)
    {
        var meshRotated0 = mesh.Copy(mesh.name + "_00");
        var meshRotated90 = mesh.Copy(mesh.name + "_90");
        var meshRotated180 = mesh.Copy(mesh.name + "_180");
        var meshRotated270 = mesh.Copy(mesh.name + "_270");

        meshRotated90.RotateVertices(Quaternion.Euler(0f, 0f, 90f));
        meshRotated180.RotateVertices(Quaternion.Euler(0f, 0f, 180f));
        meshRotated270.RotateVertices(Quaternion.Euler(0f, 0f, 270f));

        meshRotated0 = AddToOrReplaceInAsset(meshRotated0, oldMeshes, stillUsedOldMeshes);
        meshRotated90 = AddToOrReplaceInAsset(meshRotated90, oldMeshes, stillUsedOldMeshes);
        meshRotated180 = AddToOrReplaceInAsset(meshRotated180, oldMeshes, stillUsedOldMeshes);
        meshRotated270 = AddToOrReplaceInAsset(meshRotated270, oldMeshes, stillUsedOldMeshes);

        meshes.Add(meshRotated0);
        meshes90.Add(meshRotated90);
        meshes180.Add(meshRotated180);
        meshes270.Add(meshRotated270);
    }

    private Mesh AddToOrReplaceInAsset(Mesh newMesh, List<Mesh> oldMeshes, HashSet<Mesh> stillUsedOldMeshes)
    {
        var oldMesh = oldMeshes.Find(m => m.name == newMesh.name);
        if (oldMesh)
        {
            // replace asset with same name
            oldMesh.OverwriteWith(newMesh);
            stillUsedOldMeshes.Add(oldMesh);
            EditorUtility.SetDirty(oldMesh);
            
            DestroyImmediate(newMesh);
            return oldMesh;
        }
        else
        {
            // add new asset
            var assetPath = AssetDatabase.GetAssetPath(target);
            AssetDatabase.AddObjectToAsset(newMesh, assetPath);
            return newMesh;
        }
    }
}