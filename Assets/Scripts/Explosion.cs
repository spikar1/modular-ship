using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : ScriptableObject {

    public float radius = 2;
    public float pushForce = 10;
    public float strength = 2;

    void Explode(Vector3 pos) {
        Collider2D[] cols = Physics2D.OverlapCircleAll(pos, radius);
        List<Rigidbody2D> rbs = new List<Rigidbody2D>();
        List<GameObject> dams = new List<GameObject>();
        for (int i = 0; i < cols.Length; i++) {
            Rigidbody2D rb = cols[i].GetComponentInParent<Rigidbody2D>();
            IDamagable dam = cols[i].GetComponent<IDamagable>();
            if (dam != null) {
                dams.Add(cols[i].gameObject);
            }
            if (rb && !rbs.Contains(rb)) {
                rbs.Add(rb);
            }
        }

        for (int i = 0; i < rbs.Count; i++) {
            rbs[i].AddExplosionForce(pushForce, transform.position, radius);
        }

        for (int i = dams.Count - 1; i >= 0; i--) {
            float damage = Mathf.InverseLerp(radius, 0, Vector2.Distance(transform.position, dams[i].transform.position)) * strength;
            print(damage);
            dams[i].GetComponent<IDamagable>().Damage(Vector2.zero, damage);
        }
        Destroy(gameObject);
    }
}
