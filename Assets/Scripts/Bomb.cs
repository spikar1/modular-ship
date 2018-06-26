using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IDamagable {
    public int hp;
    public float radius = 2;
    public float strength = 2;
    ParticleSystem particle;
    public void Damage(Vector2 relativeVelocity, float damage) {
        hp -= (int)damage;
        if (hp <= 0)
            Explode();
    }

    void Explode() {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius);
        List<Rigidbody2D> rbs = new List<Rigidbody2D>();
        for (int i = 0; i < cols.Length; i++) {
            Rigidbody2D rb = cols[i].GetComponentInParent<Rigidbody2D>();
            print(cols[i].name);
            if (rb && !rbs.Contains(rb)) {
                rbs.Add(rb);
            }
                
        }

        for (int i = 0; i < rbs.Count; i++) {
            rbs[i].AddExplosionForce(strength, transform.position, radius);
            IDamagable dam = rbs[i].GetComponent<IDamagable>();
            if (dam != null)
                dam.Damage(Vector2.zero, Vector2.Distance(transform.position, rbs[i].transform.position) * strength);
            print(rbs[i].name);
        }
        
        Destroy(gameObject);
    }
}
