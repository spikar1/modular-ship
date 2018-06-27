using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable {

    public void OnInteract() {
        GetComponent<PolygonCollider2D>().isTrigger = !GetComponent<PolygonCollider2D>().isTrigger;
        GetComponent<MeshRenderer>().enabled = !GetComponent<MeshRenderer>().enabled;
    }
}
