using UnityEngine;

public class Obstacle : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D collision) {
        var damageable = collision.collider.GetComponent<IDamagable>();
        if (damageable != null) {
            damageable.Damage(collision.relativeVelocity, 10f);
            Destroy(gameObject);
        }
    }
}