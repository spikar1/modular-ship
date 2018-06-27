using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public void OnInteractDown()
    {
        GetComponent<PolygonCollider2D>().isTrigger = !GetComponent<PolygonCollider2D>().isTrigger;
        GetComponent<MeshRenderer>().enabled = !GetComponent<MeshRenderer>().enabled;
    }

    public void OnInteractHeld() { }
}