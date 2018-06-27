using UnityEngine;

public class LaserGun : MonoBehaviour, IInputReciever {

    bool isFiring;
    float angle = 0;
    public float maxRotation;

    public void ReceiveInput(Inputs inputs) {
        if (inputs.action1Down)
            isFiring = true;
        if (inputs.action1Up)
            isFiring = false;

        if (isFiring) {
            Shoot();
        }

        RotateTurret(inputs.axis);
    }

    void RotateTurret(Vector2 axis) {
        angle = Mathf.Clamp(transform.localRotation.eulerAngles.z + -axis.x * 2, 0, maxRotation);
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    void Shoot() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up, transform.up);
        if (hit && hit.collider.GetComponent<IDamagable>() != null) {
            hit.collider.GetComponent<IDamagable>().Damage(transform.up, 1);
        }

        Debug.DrawRay(transform.position + transform.up, transform.up * (hit?hit.distance : 50), Color.red);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.parent.up * 2);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, maxRotation) * transform.parent.up * 2);
    }


}