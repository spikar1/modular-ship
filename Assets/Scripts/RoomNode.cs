using System;
using UnityEngine;

[Serializable]
public class RoomNode : MonoBehaviour {
    public WallOrientation wallOrientation;
    public WallPiece wallPiece;
    public bool isDiagonal;
    public Room room { get; private set; }

    public int meshIndex;

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * .9f);

    }

    public void Initialize(Room room)
    {
        this.room = room;
        var wall = GetComponent<Wall>();
        if (wall)
            wall.Initialize(this);
    }

    public void UpdateCollisionMesh()
    {
        var meshFilter = GetComponent<MeshFilter>().sharedMesh;
        if (meshFilter == null)
            return;
        
        var cc = GetComponent<ColliderCreator>();
        if (cc == null)
            cc = gameObject.AddComponent<ColliderCreator>();

        cc.collisionMesh = wallPiece.GetCollisionMesh(meshFilter);
        if (!Application.isPlaying)
            cc.Start();
    }
}
