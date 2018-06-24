using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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
