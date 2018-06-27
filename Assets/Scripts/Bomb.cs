using UnityEngine;

public class Bomb : MonoBehaviour, IDamagable {
    public int hp;

    ParticleSystem particle;
    public Explosion explosion;

    private void Awake()
    {
        if (!explosion)
        {
            Debug.LogError("No explosion on Bomb! Destroying it because it can't explode! That's irony!");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        var damageable = collision.collider.GetComponent<IDamagable>();
        if (damageable != null) {
            damageable.Damage(collision.relativeVelocity, 10f);
            Damage(Vector2.zero, 3f);
        }
    }

    public void Damage(Vector2 relativeVelocity, float damage) {
        hp -= (int)damage;
        if (hp <= 0) {
            Invoke(nameof(Explode), 0.01f);
            Destroy(gameObject);
        }
    }

    void Explode() {
        explosion.Explode(transform.position);
    }

    private void OnDrawGizmos() {
        if(explosion)
            MoreGizmos.DrawCircle(transform.position, explosion.radius, 15, Color.red);
    }
}
