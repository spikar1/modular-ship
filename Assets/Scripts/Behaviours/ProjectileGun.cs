using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : Gun {
    public Rigidbody2D projectilePrefab;
    public float projectileSpeed = 10;
    [Tooltip("The maximum shots per seconds")]
    public float maxTriggerRate = 1;

    [Space(10f)]
    public bool isAutomatic = true;
    [Tooltip("Shots per second")]
    public float rateOfFire = 1;
    float timeSinceLastShot;

    public override void StartShoot() {
        
        if (rateOfFire == 0 || maxTriggerRate == 0)
            return;
        if (isAutomatic)
            InvokeRepeating("Shoot", 0, 1 / rateOfFire);
        else
            Shoot();
    }

    void Shoot() {
        if (timeSinceLastShot >= 1 / maxTriggerRate) {
            Rigidbody2D projectileRb = Instantiate(projectilePrefab, muzzle.position, Quaternion.identity).GetComponent<Rigidbody2D>();
            projectileRb.velocity = transform.up * projectileSpeed;
            timeSinceLastShot = 0;
        }
    }

    public override void EndShoot() {
        CancelInvoke("Shoot");
    }

    private void Update() {
        timeSinceLastShot += Time.deltaTime;
    }
}
