using UnityEngine;

public class Obstacle : MonoBehaviour, IDamagable {

    private int hp;
    
    private void OnCollisionEnter2D(Collision2D collision) {
        var damageable = collision.collider.GetComponent<IDamagable>();
        if (damageable != null) {
            damageable.Damage(collision.relativeVelocity, 10f);
        }
    }

    public void Damage(Vector2 relativeVelocity, float damage) {
        
    }
}