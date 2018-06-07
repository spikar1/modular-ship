using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[System.Serializable]
public class RoomNode : MonoBehaviour {
    public int x, y;
    
    public RoomNodeType roomNodeType;

    public WallOrientation wallOrientation;

    public WallPiece wallPiece;

    public bool isEdgeNode;
    
    public bool isDiagonal = false;
    [HideInInspector]
    public int meshIndex = 0;

    

    static public RoomNode CreateRoomNode(int _x, int _y) {
        GameObject gm = new GameObject("RoomNode " + _x + " " + _y);
        RoomNode rm =  gm.AddComponent<RoomNode>();
        rm.x = _x;
        rm.y = _y;
        return rm;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * .9f);

    }
}

[CanEditMultipleObjects]
[CustomEditor(typeof(RoomNode))]
public class RoomNodeEditor : Editor {

    RoomNode roomNode;

    public override void OnInspectorGUI() {
        roomNode = (RoomNode)target;
        bool diagonalLastValue = roomNode.isDiagonal;
        DrawDefaultInspector();
        //Check if object has MeshFilter and make collision.
        if(roomNode.GetComponent<MeshFilter>())
            if (GUILayout.Button("UpdateCollision")) {
                UpdateCollisionMesh();
            }

        bool showMeshPopup = true;
        for (int i = 0; i < Selection.gameObjects.Length; i++) {
            MeshFilter mf;
            mf = Selection.gameObjects[i].GetComponent<MeshFilter>();
            if (!mf) {
                showMeshPopup = false;
                break;
            }
            Debug.Log(mf.sharedMesh.name.Split('_')[0]);
            if (i != 0) {
                MeshFilter lastmf = Selection.gameObjects[i - 1].GetComponent<MeshFilter>();
                if (mf.sharedMesh.name.Split('_')[0] != lastmf.sharedMesh.name.Split('_')[0])
                    showMeshPopup = false;
            }
        }
        if (showMeshPopup) {
            Mesh mesh = roomNode.GetComponent<MeshFilter>().sharedMesh;
            string[] meshListStrings = new string[0];
            if (mesh.name.StartsWith("corner")) {
                meshListStrings = GetMeshStringArray(roomNode.wallPiece.corner);
            }
            if (mesh.name.StartsWith("straight")) {
                meshListStrings = GetMeshStringArray(roomNode.wallPiece.straight);
            }
            if (mesh.name.StartsWith("diagonal")) {
                meshListStrings = GetMeshStringArray(roomNode.wallPiece.diagonal);
            }
            int mi = EditorGUILayout.Popup(roomNode.meshIndex, meshListStrings);
            for (int i = 0; i < Selection.gameObjects.Length; i++) {
                RoomNode rn;
                MeshFilter mf;
                rn = Selection.gameObjects[i].GetComponent<RoomNode>();
                mf = rn.GetComponent<MeshFilter>();
                rn.meshIndex = mi;
                SetMesh(mf, roomNode.wallOrientation, roomNode.isDiagonal, roomNode.meshIndex);
            }
            
        }
            


        if (diagonalLastValue != roomNode.isDiagonal && roomNode.GetComponent<MeshFilter>()) {
            for (int i = 0; i < Selection.gameObjects.Length; i++) {
                GameObject go = Selection.gameObjects[i];
                RoomNode rn = go.GetComponent<RoomNode>();
                rn.meshIndex = 0;
                SetMesh(go.GetComponent<MeshFilter>(), rn.wallOrientation, rn.isDiagonal);
            }
        }

        OrientationButtons();
            

        if (GUI.changed)
            EditorUtility.SetDirty(roomNode);
    }

    string[] GetMeshStringArray(List<Mesh> meshList) {
        string[] meshListStrings;
        meshListStrings = new string[meshList.Count];
        for (int i = 0; i < meshList.Count; i++) {
            meshListStrings[i] = meshList[i].name;
        }
        return meshListStrings;
        
    }

    void OrientationButtons() {
        float buttonWidth = (Screen.width / 3) - 20;
        GUILayoutOption GUIbuttonWidth = GUILayout.Width(buttonWidth);

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Top Left", GUIbuttonWidth)) {
                ChangeWallOrientation(WallOrientation.TopLeft);
            }
            if (GUILayout.Button("Top", GUIbuttonWidth)) {
                ChangeWallOrientation(WallOrientation.Top);
            }
            if (GUILayout.Button("Top Right", GUIbuttonWidth)) {
                ChangeWallOrientation(WallOrientation.TopRight);
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Left", GUIbuttonWidth)) {
                ChangeWallOrientation(WallOrientation.Left);

            }

            if (GUILayout.Button("Empty", GUIbuttonWidth)) {
                for (int i = 0; i < Selection.gameObjects.Length; i++) {
                    GameObject go = Selection.gameObjects[i];
                    if (go.GetComponent<MeshFilter>())
                        DestroyImmediate(go.GetComponent<MeshFilter>());
                    if (go.GetComponent<MeshRenderer>())
                        DestroyImmediate(go.GetComponent<MeshRenderer>());
                    if (go.GetComponent<ColliderCreator>())
                        DestroyImmediate(go.GetComponent<ColliderCreator>());
                    if (go.GetComponent<PolygonCollider2D>())
                        DestroyImmediate(go.GetComponent<PolygonCollider2D>());
                    if(go.GetComponent<Wall>())
                        DestroyImmediate(go.GetComponent<Wall>());
                }

            }

            if (GUILayout.Button("Right", GUIbuttonWidth)) {
                ChangeWallOrientation(WallOrientation.Right);
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        {


            if (GUILayout.Button("Bottom Left", GUIbuttonWidth)) {
                ChangeWallOrientation(WallOrientation.BottomLeft);
            }
            if (GUILayout.Button("Bottom", GUIbuttonWidth)) {
                ChangeWallOrientation(WallOrientation.Bottom);
            }
            if (GUILayout.Button("Bottom Right", GUIbuttonWidth)) {
                ChangeWallOrientation(WallOrientation.BottomRight);

            }
        }
        GUILayout.EndHorizontal();
    }

    void ChangeWallOrientation(WallOrientation wo) {
        for (int i = 0; i < Selection.gameObjects.Length; i++) {
            GameObject go = Selection.gameObjects[i];
            MeshFilter mf;
            RoomNode rn = go.GetComponent<RoomNode>();
            rn.wallOrientation = wo;
            rn.meshIndex = 0;
            if (mf = go.GetComponent<MeshFilter>()) {
                SetMesh(mf, wo, roomNode.isDiagonal);//mf.mesh = roomNode.wallPiece.GetMeshFromOrientation(wo, roomNode.isDiagonal);
            }
            else
                go.AddComponent<MeshFilter>().mesh = roomNode.wallPiece.GetMeshFromOrientation(wo, roomNode.isDiagonal);

            if (!go.GetComponent<MeshRenderer>())
                go.AddComponent<MeshRenderer>();

            go.GetComponent<MeshRenderer>().material = roomNode.wallPiece.material;

            go.transform.rotation = GetRotationFromOrientation(wo);

        }
    }

    void SetMesh(MeshFilter _mf, WallOrientation wo, bool _isDiagonal, int index = -1) {
        _mf.mesh = roomNode.wallPiece.GetMeshFromOrientation(wo, _isDiagonal, index);
        UpdateWall(_mf.gameObject);
    }

    void UpdateWall(GameObject go) {
        if (!go.GetComponent<Wall>())
            go.AddComponent<Wall>();
        Wall w = go.GetComponent<Wall>();
        RoomNode rn = go.GetComponent<RoomNode>();
        w.durability = rn.wallPiece.durability;
        w.integrity = rn.wallPiece.integrity;
    }

    Quaternion GetRotationFromOrientation(WallOrientation wo) {
        switch (wo) {
            case WallOrientation.TopLeft:
            case WallOrientation.Top:
                return Quaternion.Euler(0, 0, 0);
            case WallOrientation.TopRight:
            case WallOrientation.Right:
                return Quaternion.Euler(0, 0, -90);
            case WallOrientation.BottomRight:
            case WallOrientation.Bottom:
                return Quaternion.Euler(0, 0, 180);
            case WallOrientation.BottomLeft:
            case WallOrientation.Left:
                return Quaternion.Euler(0, 0, 90);
            default:
                throw new System.Exception(wo + " is not valid");
        }
    }

    void UpdateCollisionMesh() {
        
        for (int i = 0; i < Selection.gameObjects.Length; i++) {
            GameObject go = Selection.gameObjects[i];
            if (!go.GetComponent<MeshFilter>())
                continue;
            RoomNode rn = go.GetComponent<RoomNode>();
            ColliderCreator cc;
            
            cc = go.GetComponent<ColliderCreator>();
            if (cc == null)
                cc = go.AddComponent<ColliderCreator>();

            cc.collisionMesh = rn.wallPiece.GetCollisionMesh(rn.GetComponent<MeshFilter>().sharedMesh);
            cc.Start();
        }
    }
}