using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public void ToggleDoor() {
        GetComponent<PolygonCollider2D>().enabled = !GetComponent<PolygonCollider2D>().enabled;
        GetComponent<MeshRenderer>().enabled = !GetComponent<MeshRenderer>().enabled;
    }
}
