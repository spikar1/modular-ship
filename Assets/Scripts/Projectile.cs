using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour {
    public Rigidbody2D rb;
    public float damage = 1;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        var damageable = collision.collider.GetComponent<IDamagable>();
        if (damageable != null) {
            damageable.Damage(damage);
        }
        Destroy(gameObject);
    }
}
