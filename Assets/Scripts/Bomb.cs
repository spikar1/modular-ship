using UnityEngine;

public class Bomb : MonoBehaviour, IDamagable
{
    public int hp;
    public Explosion explosion;
    public float impactDamageMin = 1f;
    public float impactDamageMax = 10f;
    public float minDamageVelocity = .5f;
    public float maxDamageVelocity = 7f;

    private ParticleSystem particle;
    bool hasExploded;

    private void Awake()
    {
        if (!explosion)
        {
            Debug.LogError("No explosion on Bomb! Destroying it because it can't explode! That's irony!");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var damageable = collision.collider.GetComponent<IDamagable>();
        if (damageable != null)
        {
            damageable.Damage(ColisionDamageHelper.ScaleDamageByVelocity(
                impactDamageMin,
                impactDamageMax,
                minDamageVelocity,
                maxDamageVelocity,
                collision.relativeVelocity
            ));
            Damage(3f);
        }
    }

    public void Damage(float damage)
    {
        hp -= (int) damage;
        if (hp <= 0)
        {
            if (!hasExploded)
            {
                hasExploded = true;
                explosion.Explode(transform.position);
            }

            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (explosion)
            MoreGizmos.DrawCircle(transform.position, explosion.radius, 15, Color.red);
    }
}