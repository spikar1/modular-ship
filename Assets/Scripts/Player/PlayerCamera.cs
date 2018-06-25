using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	private void Update () {
		var shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
		RotateCamera(shiftHeld);
		ZoomCamera(shiftHeld);
	}

	private void RotateCamera(bool shiftHeld) {
		float rotation = 0f;
		if (Input.GetMouseButton(2))
			rotation -= Input.GetAxis("Mouse X") * 3;
		if (Input.GetKeyDown(KeyCode.Q))
			rotation -= shiftHeld ? 30f : 5f;
		if (Input.GetKeyDown(KeyCode.E))
			rotation += shiftHeld ? 30f : 5f;
		
		transform.Rotate(transform.forward, rotation);
	}
	
	private void ZoomCamera(bool shiftHeld) {
		var zoom = Input.mouseScrollDelta.y;
		if (Input.GetKeyDown(KeyCode.PageUp))
			zoom += shiftHeld ? 5f : 1f;
		if (Input.GetKeyDown(KeyCode.PageDown))
			zoom -= shiftHeld ? 5f : 1f;

		transform.GetChild(0).position += transform.GetChild(0).forward * zoom;
	}
}
