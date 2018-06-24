using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	void Update () {
        if (Input.GetMouseButton(2)) {
            RotateCamera();
        }
        transform.GetChild(0).position += transform.GetChild(0).forward * Input.mouseScrollDelta.y;
	}

    void RotateCamera() {
        transform.Rotate(transform.forward, -Input.GetAxis("Mouse X") * 3);
    }
}
