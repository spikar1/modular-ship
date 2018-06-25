using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	private void Update () {
		RotateCamera();
		ZoomCamera();
	}

	private void RotateCamera() {
		float rotation = 0f;
		if (Input.GetMouseButton(2))
			rotation -= Input.GetAxis("Mouse X") * 3;
		if (Input.GetKeyDown(KeyCode.Q))
			rotation -= 5;
		if (Input.GetKeyDown(KeyCode.E))
			rotation += 5;
		
		transform.Rotate(transform.forward, rotation);
	}
	
	private void ZoomCamera() {
		var zoom = Input.mouseScrollDelta.y;
		if (Input.GetKeyDown(KeyCode.PageUp))
			zoom += 1f;
		if (Input.GetKeyDown(KeyCode.PageDown))
			zoom -= 1f;

		transform.GetChild(0).position += transform.GetChild(0).forward * zoom;
	}
}
