using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    int player;

    private void Start() {
        player = transform.parent.GetComponent<Player>().player;
    }

    private void Update () {
        print(Input.GetAxisRaw("RightStick X"));
        switch (player) {
            case 0:
                RotateCamera(Input.GetAxisRaw("Mouse X"), Input.GetMouseButton(2));
                ZoomCamera(Input.mouseScrollDelta.y);
                break;
            case 1:
                RotateCamera(Input.GetAxisRaw("RightStick X") * .4f);
                ZoomCamera(-Input.GetAxisRaw("RightStick Y") * .1f);
                break;
            default:
                Debug.LogError("The player int " + player + " is not supported");
                break;
        }
		
	}

	private void RotateCamera(float input, bool shouldRotate = true) {
        if (!shouldRotate)
            return;
		float rotation = 0f;
		rotation -= input * 3;
		transform.Rotate(transform.forward, rotation);
	}
	
	private void ZoomCamera(float input) {
		var zoom = input;

		transform.GetChild(0).position += transform.GetChild(0).forward * zoom;
	}
}
