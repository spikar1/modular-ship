﻿using System.Collections;
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
        List<GameObject> dams = new List<GameObject>();
        for (int i = 0; i < cols.Length; i++) {
            if (cols[i].gameObject == gameObject)
                continue;
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
            rbs[i].AddExplosionForce(strength, transform.position, radius);
        }

        for (int i = dams.Count - 1; i >= 0; i--) {
            float damage = Mathf.InverseLerp(radius, 0, Vector2.Distance(transform.position, dams[i].transform.position)) * strength;
            print(damage);
            dams[i].GetComponent<IDamagable>().Damage(Vector2.zero, damage);
        }
        
        Destroy(gameObject);
    }
}
