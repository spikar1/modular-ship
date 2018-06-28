using UnityEngine;

public class Rotator : MonoBehaviour {

    public float speed = 1;
    public Vector3 axis = Vector3.forward;

    private void Update() {
        transform.Rotate(axis, speed * Time.deltaTime);
    }

}
