using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.Linq;

[CanEditMultipleObjects]
[CustomEditor(typeof(RoomNode))]
public class RoomNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        bool roomsUpdated = false;
        RoomNode[] roomNodes = new RoomNode[targets.Length];
        for (int i = 0; i < targets.Length; i++)
            roomNodes[i] = (RoomNode) targets[i];

        bool[] diagonalLastValue = roomNodes.Select(rn => rn.isDiagonal).ToArray();
        DrawDefaultInspector();

        DrawSelectMeshPopup(roomNodes, ref roomsUpdated);

        for (var i = 0; i < roomNodes.Length; i++)
        {
            RoomNode rn = roomNodes[i];
            if (diagonalLastValue[i] != rn.isDiagonal && rn.GetComponent<MeshFilter>())
            {
                rn.meshIndex = 0;
                SetMesh(rn, rn.wallOrientation);
                roomsUpdated = true;
            }
        }

        Undo.RecordObjects(targets, "Orientation buttons");
        OrientationButtons(roomNodes, ref roomsUpdated);

        if (GUILayout.Button("UpdateCollision") || roomsUpdated)
        {
            Undo.RecordObjects(targets, "Updating collision meshes");
            foreach (var rn in roomNodes)
                rn.UpdateCollisionMesh();
        }

        if (roomsUpdated)
        {
            foreach (var roomNode in roomNodes)
            {
                EditorUtility.SetDirty(roomNode);
                EditorSceneManager.MarkSceneDirty(roomNode.gameObject.scene);
            }
        }
        
        EditorGUILayout.Space();
        if (GUILayout.Button("Refresh all rooms"))
        {
            var allRooms = FindObjectsOfType<RoomNode>();
            Undo.RecordObjects(allRooms, "Refresh all rooms");
            foreach (var room in allRooms)
            {
                if (room.GetComponent<MeshFilter>()) //This is funky. @TODO: add "None" wall orientation. Rename wall kind? 
                {
                    SetMesh(room, room.wallOrientation, room.meshIndex);
                    room.UpdateCollisionMesh();
                }
                else {
                    RenameNode(room, false);
                }
            }
        } 
    }

    private void DrawSelectMeshPopup(RoomNode[] roomNodes, ref bool roomsUpdated)
    {
        var firstRoomNode = roomNodes[0];
        var firstMeshFilter = firstRoomNode.GetComponent<MeshFilter>();
        var showMeshPopup = roomNodes.Length == 1 && firstMeshFilter && firstMeshFilter.sharedMesh;

        if (!showMeshPopup && firstMeshFilter && firstMeshFilter.sharedMesh)
            showMeshPopup = roomNodes.All(rn =>
            {
                var mf = rn.GetComponent<MeshFilter>();
                if (!mf)
                    return false;
                return mf.sharedMesh == firstMeshFilter.sharedMesh;
            });

        if (showMeshPopup)
        {
            var mesh = firstMeshFilter.GetComponent<MeshFilter>().sharedMesh;
            string[] meshListStrings = null;

            if (mesh.name.StartsWith("corner"))
                meshListStrings = GetMeshStringArray(firstRoomNode.wallPiece.corner);

            else if (mesh.name.StartsWith("straight"))
                meshListStrings = GetMeshStringArray(firstRoomNode.wallPiece.straight);

            else if (mesh.name.StartsWith("diagonal"))
                meshListStrings = GetMeshStringArray(firstRoomNode.wallPiece.diagonal);

            if (meshListStrings == null)
            {
                EditorGUILayout.LabelField($"Unknown mesh: {mesh.name}, can't generate select mesh dropdown");
            }
            else
            {
                var oldMi = firstRoomNode.meshIndex;
                var newMI = EditorGUILayout.Popup(firstRoomNode.meshIndex, meshListStrings);

                if (oldMi != newMI)
                {
                    roomsUpdated = true;
                    foreach (RoomNode rn in roomNodes)
                    {
                        rn.meshIndex = newMI;
                        SetMesh(rn, firstRoomNode.wallOrientation, firstRoomNode.meshIndex);
                    }
                }
            }
        }
    }

    private string[] GetMeshStringArray(List<Mesh> meshList)
    {
        var meshListStrings = new string[meshList.Count];
        for (int i = 0; i < meshList.Count; i++)
            meshListStrings[i] = meshList[i].name;

        return meshListStrings;
    }

    private void OrientationButtons(RoomNode[] roomNodes, ref bool roomsUpdated)
    {
        float buttonWidth = (Screen.width / 3) - 20;
        GUILayoutOption GUIbuttonWidth = GUILayout.Width(buttonWidth);

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Top Left", GUIbuttonWidth) || Event.current.keyCode == KeyCode.Keypad7)
            {
                ChangeWallOrientation(WallOrientation.TopLeft, roomNodes);
                roomsUpdated = true;
            }

            if (GUILayout.Button("Top", GUIbuttonWidth) || Event.current.keyCode == KeyCode.Keypad8)
            {
                ChangeWallOrientation(WallOrientation.Top, roomNodes);
                roomsUpdated = true;
            }

            if (GUILayout.Button("Top Right", GUIbuttonWidth) || Event.current.keyCode == KeyCode.Keypad9)
            {
                ChangeWallOrientation(WallOrientation.TopRight, roomNodes);
                roomsUpdated = true;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Left", GUIbuttonWidth) || Event.current.keyCode == KeyCode.Keypad4)
            {
                ChangeWallOrientation(WallOrientation.Left, roomNodes);
                roomsUpdated = true;
            }

            if (GUILayout.Button("Empty", GUIbuttonWidth) || Event.current.keyCode == KeyCode.Keypad5)
            {
                for (int i = 0; i < Selection.gameObjects.Length; i++)
                {
                    RenameNode(roomNodes[i], false);
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
                
                roomsUpdated = true;
            }

            if (GUILayout.Button("Right", GUIbuttonWidth) || Event.current.keyCode == KeyCode.Keypad6)
            {
                ChangeWallOrientation(WallOrientation.Right, roomNodes);
                roomsUpdated = true;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Bottom Left", GUIbuttonWidth) || Event.current.keyCode == KeyCode.Keypad1)
            {
                ChangeWallOrientation(WallOrientation.BottomLeft, roomNodes);
                roomsUpdated = true;
            }

            if (GUILayout.Button("Bottom", GUIbuttonWidth) || Event.current.keyCode == KeyCode.Keypad2)
            {
                ChangeWallOrientation(WallOrientation.Bottom, roomNodes);
                roomsUpdated = true;
            }

            if (GUILayout.Button("Bottom Right", GUIbuttonWidth) || Event.current.keyCode == KeyCode.Keypad3)
            {
                ChangeWallOrientation(WallOrientation.BottomRight, roomNodes);
                roomsUpdated = true;
            }
        }
        GUILayout.EndHorizontal();
    }

    private void ChangeWallOrientation(WallOrientation orientation, RoomNode[] roomNodes)
    {
        foreach (var roomNode in roomNodes)
        {
            roomNode.wallOrientation = orientation;
            roomNode.meshIndex = 0;
            SetMesh(roomNode, orientation);

            if (!roomNode.GetComponent<MeshRenderer>())
                roomNode.gameObject.AddComponent<MeshRenderer>();

            roomNode.GetComponent<MeshRenderer>().sharedMaterial = roomNode.wallPiece.material;

            roomNode.transform.rotation = Quaternion.identity;
        }
    }

    private void SetMesh(RoomNode roomNode, WallOrientation orientation, int meshIndex = -1)
    {
        RenameNode(roomNode, true);

        var meshFilter = roomNode.GetComponent<MeshFilter>();
        if (!meshFilter)
            meshFilter = roomNode.gameObject.AddComponent<MeshFilter>();

        meshFilter.mesh = roomNode.wallPiece.GetMeshFromOrientation(orientation, roomNode.isDiagonal, meshIndex);
        UpdateWall(meshFilter.gameObject);
    }

    private void RenameNode(RoomNode rn, bool hasMesh) {
        if(hasMesh)
            rn.gameObject.name = "N_" + rn.wallPiece.name + "_" + rn.wallOrientation.ToString();
        else
            rn.gameObject.name = "N_" + "Empty";
    }

    private void UpdateWall(GameObject go)
    {
        if (!go.GetComponent<Wall>())
            go.AddComponent<Wall>();
        var wall = go.GetComponent<Wall>();
        var roomNode = go.GetComponent<RoomNode>();
        if (!EditorApplication.isPlaying)
        {
            wall.integrity = roomNode.wallPiece.integrity;
        }
    }
}