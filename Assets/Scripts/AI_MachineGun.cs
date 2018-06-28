using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_MachineGun : MonoBehaviour {

    ProjectileGun gun;
    public Vector2 target;
    public float range = 15, turnSpeed = 15;

    private void Start() {
        gun = GetComponent<ProjectileGun>();
        InvokeRepeating("FindPlayerShip", 0, .5f);
    }

    private void Update() {
        if(target != Vector2.zero)
            gun.RotateTurretTowards(turnSpeed * Time.deltaTime, target);
    }

    void FindPlayerShip() {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, range);
        Ship ship;
        for (int i = 0; i < cols.Length; i++) {
            if(ship = cols[i].GetComponentInParent<Ship>()) {
                target = ship.transform.position;
                print(ship.transform.position);
                gun.StartShoot();
                return;
            }
        }
        target = Vector2.zero;
        gun.EndShoot();
    }
                
}
