using UnityEngine;

public class CameraController : MonoBehaviour, IPlayerInputReceiver {
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
        RotateCamera(inputs.cameraAxis.x);
        ZoomCamera(inputs.cameraAxis.y);
    }

    private void RotateCamera(float input) {
        var rotation = -input * 3;
        cameraTransform.Rotate(cameraTransform.forward, rotation);
    }

    private void ZoomCamera(float zoom) {
        wantedOffset += cameraTransform.GetChild(0).forward * zoom;
    }

    public int InputOrder => InputReceiverOrder.Camera;
    public bool ReceiveInput => true;
}