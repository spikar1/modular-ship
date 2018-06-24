using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour, IInputReciever {

    bool isFiring;

    public void ReceiveInput(bool action1Down, bool action1Up, Vector2 axis) {
        if (action1Down)
            isFiring = true;
        if (action1Up)
            isFiring = false;

        if (isFiring) {
            Shoot();
        }

        transform.Rotate(Vector3.forward, -axis.x * 2);
    }

    void Shoot() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up, transform.up);
        if (hit && hit.collider.GetComponent<IDamagable>() != null) {
            hit.collider.GetComponent<IDamagable>().Damage(transform.up, 1);
        }

        Debug.DrawRay(transform.position + transform.up, transform.up * (hit?hit.distance : 50), Color.red);
    }


}