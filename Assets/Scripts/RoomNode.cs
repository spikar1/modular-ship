using System;
using UnityEngine;

[Serializable]
public class RoomNode : MonoBehaviour {
    public WallOrientation wallOrientation;
    public WallPiece wallPiece;
    public bool isDiagonal;
    public Room room { get; private set; }

    [NonSerialized]
    public int meshIndex = 0;

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
}
