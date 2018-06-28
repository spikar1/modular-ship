using UnityEngine;

public class LaserGun : Gun {
    bool fireRay = false;
    RaycastHit2D hit;
    float interval = .1f;

    public override void EndShoot() {
        CancelInvoke("FireLaser");
        fireRay = false;
    }

    public override void StartShoot() {
        fireRay = true;
        InvokeRepeating("FireLaser", 0, interval);
    }

    private void Update() {
        if (!fireRay) {
            
            return;
        }
        Debug.DrawRay(transform.position + transform.up, transform.up * (hit ? hit.distance : 50), Color.red);
    }

    void FireLaser() {
        hit = Physics2D.Raycast(transform.position + transform.up, transform.up);
        if (hit && hit.collider.GetComponent<IDamagable>() != null) {

            Collider2D[] cols = Physics2D.OverlapCircleAll(hit.point, .01f);
            float z = 0;
            IDamagable dam = null;
            for (int i = 0; i < cols.Length; i++) {
                if (cols[i].transform.position.z < z) {
                    print("hello");
                    z = cols[i].transform.position.z;
                    dam = cols[i].GetComponent<IDamagable>();
                }

            }
            print(dam);
            if (dam != null)
                dam.Damage(1);
        }
    }
}