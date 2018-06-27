using UnityEngine;

<<<<<<< HEAD
public class Door : MonoBehaviour, IInteractable {

    public void OnInteract() {

=======
public class Door : MonoBehaviour, IInteractable
{
    public void OnInteractDown()
    {
>>>>>>> 3d3152a450498517ef2803b8a8aaa5bb32c47c36
        GetComponent<PolygonCollider2D>().isTrigger = !GetComponent<PolygonCollider2D>().isTrigger;
        GetComponent<MeshRenderer>().enabled = !GetComponent<MeshRenderer>().enabled;
    }

    public void OnInteractHeld() { }
}