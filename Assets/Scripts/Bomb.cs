using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IDamagable {
    public int hp;

    ParticleSystem particle;

    private void OnCollisionEnter2D(Collision2D collision) {
        var damageable = collision.collider.GetComponent<IDamagable>();
        if (damageable != null) {
            damageable.Damage(collision.relativeVelocity, 10f);
            Damage(Vector2.zero, 3f);
        }
    }

    public void Damage(Vector2 relativeVelocity, float damage) {
        hp -= (int)damage;
        if (hp <= 0)
            Explode();
    }

    

    private void OnDrawGizmos() {
        MoreGizmos.DrawCircle(transform.position, radius, 15, Color.red);
    }
}
