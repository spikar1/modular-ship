using UnityEngine;

public class Obstacle : MonoBehaviour, IDamagable
{
    public float minDamage = 2f;
    public float maxDamage = 5f;
    public float minDamageVelocity = 1f;
    public float maxDamageVelocity = 3f;

    public int hp;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var damageable = collision.collider.GetComponent<IDamagable>();
        if (damageable != null)
        {
            damageable.Damage(ColisionDamageHelper.ScaleDamageByVelocity(
                minDamage,
                maxDamage,
                minDamageVelocity,
                maxDamageVelocity,
                collision.relativeVelocity
            ));
        }
    }

    public void Damage(float damage)
    {
        hp -= (int) damage;
        DamageText.ShowDamageText(gameObject, damage);
        if (hp <= 0)
            Destroy(gameObject);
    }
}

public static class ColisionDamageHelper
{
    public static float ScaleDamageByVelocity(float minD, float maxD, float minV, float maxV, Vector2 velocity)
    {
        var velMag = velocity.magnitude;
        var damageScale = Mathf.InverseLerp(minV, maxV, velMag);
        return Mathf.Lerp(minD, maxD, damageScale);
    }
}