using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Door : NetworkBehaviour {

    public void ToggleDoor() {
        if (!isServer)
            return;
        RpcOpenDoors();
    }

    [ClientRpc]
    void RpcOpenDoors() {
        GetComponent<PolygonCollider2D>().enabled = !GetComponent<PolygonCollider2D>().enabled;
        GetComponent<MeshRenderer>().enabled = !GetComponent<MeshRenderer>().enabled;
    }
}
