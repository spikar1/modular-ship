﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour, ISeatInputReceiver {
    public bool isFiring;
    float angle = 0;
    public float maxRotation;
    public Transform muzzle;

    public void OnUpdate(Inputs inputs)
    {
        if (inputs.interactDown)
            StartShoot();
        if (inputs.interactUp)
            EndShoot();

        RotateTurret(inputs.axis);
    }

    public void OnSeated(Sitter sitter) {
        sitter.GetComponent<CameraController>().target = transform;
    }

    void RotateTurret(Vector2 axis) {
        angle = Mathf.Clamp(transform.localRotation.eulerAngles.z + -axis.x * 2, 0, maxRotation);
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public abstract void StartShoot();
    public abstract void EndShoot();

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.parent.up * 2);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, maxRotation) * transform.parent.up * 2);
    }
}
