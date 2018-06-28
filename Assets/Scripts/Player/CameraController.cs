using UnityEngine;

public class CameraController : MonoBehaviour, IInputReceiver {
    public Transform cameraTransform;
    public Transform target;

    private Vector3 wantedOffset;

    private void Awake() {
        cameraTransform.parent = null;
        wantedOffset = cameraTransform.position - target.position;
    }

    void LateUpdate() {
        cameraTransform.position = target.position + wantedOffset;
    }

    public void OnUpdate(Inputs inputs) {
        RotateCamera(inputs.cameraRotation);
        ZoomCamera(inputs.cameraZoom);
    }

    private void RotateCamera(float input) {
        var rotation = -input * 3;
        cameraTransform.Rotate(cameraTransform.forward, rotation);
    }

    private void ZoomCamera(float zoom) {
        cameraTransform.position += transform.GetChild(0).forward * zoom;
    }
}