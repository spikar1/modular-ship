using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

[CanEditMultipleObjects]
[CustomEditor(typeof(RoomNode))]
public class RoomNodeEditor : Editor
{
    private RoomNode roomNode;
    void OnEnable()
    {
        roomNode = (RoomNode) target;
    }

    public override void OnInspectorGUI()
    {
        bool diagonalLastValue = roomNode.isDiagonal;
        DrawDefaultInspector();

        //Check if object has MeshFilter and make collision.
        if (roomNode.GetComponent<MeshFilter>())
            if (GUILayout.Button("UpdateCollision"))
                UpdateCollisionMesh();

        bool showMeshPopup = true;
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            var mf = Selection.gameObjects[i].GetComponent<MeshFilter>();
            if (!mf)
            {
                showMeshPopup = false;
                break;
            }

            if (i != 0)
            {
                MeshFilter lastmf = Selection.gameObjects[i - 1].GetComponent<MeshFilter>();
                if (mf.sharedMesh.name.Split('_')[0] != lastmf.sharedMesh.name.Split('_')[0])
                    showMeshPopup = false;
            }
        }

        if (showMeshPopup)
        {
            Mesh mesh = roomNode.GetComponent<MeshFilter>().sharedMesh;
            string[] meshListStrings = new string[0];
            if (mesh.name.StartsWith("corner"))
                meshListStrings = GetMeshStringArray(roomNode.wallPiece.corner);

            if (mesh.name.StartsWith("straight"))
                meshListStrings = GetMeshStringArray(roomNode.wallPiece.straight);

            if (mesh.name.StartsWith("diagonal"))
                meshListStrings = GetMeshStringArray(roomNode.wallPiece.diagonal);

            int mi = EditorGUILayout.Popup(roomNode.meshIndex, meshListStrings);
            foreach (RoomNode rn in targets)
            {
                var mf = rn.GetComponent<MeshFilter>();
                rn.meshIndex = mi;
                SetMesh(mf, roomNode.wallOrientation, roomNode.isDiagonal, roomNode.meshIndex);
            }
        }

        if (diagonalLastValue != roomNode.isDiagonal && roomNode.GetComponent<MeshFilter>())
        {
            foreach (RoomNode rn in targets)
            {
                rn.meshIndex = 0;
                SetMesh(rn.GetComponent<MeshFilter>(), rn.wallOrientation, rn.isDiagonal);
            }
        }

        Undo.RecordObjects(targets, "Orientation buttons");
        OrientationButtons();

        if (GUI.changed)
        {
            foreach (RoomNode rn in targets)
            {
                EditorUtility.SetDirty(rn);
                EditorSceneManager.MarkSceneDirty(rn.gameObject.scene);
            }
        }
    }

    string[] GetMeshStringArray(List<Mesh> meshList)
    {
        string[] meshListStrings;
        meshListStrings = new string[meshList.Count];
        for (int i = 0; i < meshList.Count; i++)
        {
            meshListStrings[i] = meshList[i].name;
        }

        return meshListStrings;
    }

    void OrientationButtons()
    {
        float buttonWidth = (Screen.width / 3) - 20;
        GUILayoutOption GUIbuttonWidth = GUILayout.Width(buttonWidth);

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Top Left", GUIbuttonWidth) || Event.current.keyCode == KeyCode.Keypad7)
            {
                ChangeWallOrientation(WallOrientation.TopLeft);
            }

            if (GUILayout.Button("Top", GUIbuttonWidth) || Event.current.keyCode == KeyCode.Keypad8)
            {
                ChangeWallOrientation(WallOrientation.Top);
            }

            if (GUILayout.Button("Top Right", GUIbuttonWidth) || Event.current.keyCode == KeyCode.Keypad9)
            {
                ChangeWallOrientation(WallOrientation.TopRight);
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Left", GUIbuttonWidth) || Event.current.keyCode == KeyCode.Keypad4)
            {
                ChangeWallOrientation(WallOrientation.Left);
            }

            if (GUILayout.Button("Empty", GUIbuttonWidth) || Event.current.keyCode == KeyCode.Keypad5)
            {
                for (int i = 0; i < Selection.gameObjects.Length; i++)
                {
                    GameObject go = Selection.gameObjects[i];
                    if (go.GetComponent<MeshFilter>())
                        DestroyImmediate(go.GetComponent<MeshFilter>());
                    if (go.GetComponent<MeshRenderer>())
                        DestroyImmediate(go.GetComponent<MeshRenderer>());
                    if (go.GetComponent<ColliderCreator>())
                        DestroyImmediate(go.GetComponent<ColliderCreator>());
                    if (go.GetComponent<PolygonCollider2D>())
                        DestroyImmediate(go.GetComponent<PolygonCollider2D>());
                    if (go.GetComponent<Wall>())
                        DestroyImmediate(go.GetComponent<Wall>());
                }
            }

            if (GUILayout.Button("Right", GUIbuttonWidth) || Event.current.keyCode == KeyCode.Keypad6)
            {
                ChangeWallOrientation(WallOrientation.Right);
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Bottom Left", GUIbuttonWidth) || Event.current.keyCode == KeyCode.Keypad1)
            {
                ChangeWallOrientation(WallOrientation.BottomLeft);
            }

            if (GUILayout.Button("Bottom", GUIbuttonWidth) || Event.current.keyCode == KeyCode.Keypad2)
            {
                ChangeWallOrientation(WallOrientation.Bottom);
            }

            if (GUILayout.Button("Bottom Right", GUIbuttonWidth) || Event.current.keyCode == KeyCode.Keypad3)
            {
                ChangeWallOrientation(WallOrientation.BottomRight);
            }
        }
        GUILayout.EndHorizontal();
    }

    void ChangeWallOrientation(WallOrientation wo)
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            GameObject go = Selection.gameObjects[i];
            MeshFilter mf;
            RoomNode rn = go.GetComponent<RoomNode>();
            rn.wallOrientation = wo;
            rn.meshIndex = 0;
            if (mf = go.GetComponent<MeshFilter>())
            {
                SetMesh(mf, wo,
                    roomNode.isDiagonal); //mf.mesh = roomNode.wallPiece.GetMeshFromOrientation(wo, roomNode.isDiagonal);
            }
            else
                go.AddComponent<MeshFilter>().mesh = roomNode.wallPiece.GetMeshFromOrientation(wo, roomNode.isDiagonal);

            if (!go.GetComponent<MeshRenderer>())
                go.AddComponent<MeshRenderer>();

            go.GetComponent<MeshRenderer>().material = roomNode.wallPiece.material;

            go.transform.rotation = wo.ToRotation();
        }
    }

    void SetMesh(MeshFilter _mf, WallOrientation wo, bool _isDiagonal, int index = -1)
    {
        _mf.mesh = roomNode.wallPiece.GetMeshFromOrientation(wo, _isDiagonal, index);
        UpdateWall(_mf.gameObject);
    }

    void UpdateWall(GameObject go)
    {
        if (!go.GetComponent<Wall>())
            go.AddComponent<Wall>();
        Wall w = go.GetComponent<Wall>();
        RoomNode rn = go.GetComponent<RoomNode>();
        if (!EditorApplication.isPlaying)
        {
            w.integrity = rn.wallPiece.integrity;
        }
    }

    private void UpdateCollisionMesh()
    {
        foreach (RoomNode rn in targets)
        {
            var cc = rn.GetComponent<ColliderCreator>();
            if (cc == null)
                cc = rn.gameObject.AddComponent<ColliderCreator>();

            cc.collisionMesh = rn.wallPiece.GetCollisionMesh(rn.GetComponent<MeshFilter>().sharedMesh);
            if (!Application.isPlaying)
                cc.Start();
        }
    }
}