using UnityEngine;

public class LaserGun : Gun, IInputReciever {
    bool fireRay = false;

    public override void EndShoot() {
        fireRay = false;
    }

    public override void StartShoot() {
        fireRay = true;
    }

    private void Update() {
        if (!fireRay)
            return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up, transform.up);
        if (hit && hit.collider.GetComponent<IDamagable>() != null) {
            hit.collider.GetComponent<IDamagable>().Damage(transform.up, 1);
        }

        Debug.DrawRay(transform.position + transform.up, transform.up * (hit ? hit.distance : 50), Color.red);
    }
}