using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public List<RoomNode> roomNodes = new List<RoomNode>();

    public int integrity, durability;

    private void Start() {
        EvaluateHealth();
    }

    void EvaluateHealth() {
        integrity = 0;
        durability = 0;
        for (int i = 0; i < roomNodes.Count; i++) {
            Wall wall = roomNodes[i].GetComponent<Wall>();
            if (!wall)
                continue;
            integrity += wall.integrity;
            //durability += wall.durability;
        }
    }
}
