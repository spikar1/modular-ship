using UnityEngine;

public class CarryBot : MonoBehaviour, ISeatInputReceiver, IDamagable
{
    private Rigidbody2D rb;
    public float hp = 10;
    public float acceleration = 1f;
    public float maxVelocity = 10f;

    [Tooltip("Clamps the tilt at a certain angle")]
    public float clampAngle = 10;

    [Tooltip("amplify the tilting")] 
    public float tiltAmplifier = 1;


    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public void OnUpdate(Inputs inputs)
    {
        rb.AddForce(inputs.axis * acceleration);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        
        RotateTowardsVelocity(inputs.axis);
    }

    public void OnSeated(Sitter sitter) {
        sitter.GetComponent<CameraController>().target = transform;
    }


    void RotateTowardsVelocity(Vector2 axis)
    {
        if (!rb)
        {
            return;
        }

        Vector3 locVel = transform.InverseTransformDirection(axis);

        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.Euler(
                Mathf.Clamp(locVel.y * tiltAmplifier, -clampAngle, clampAngle),
                Mathf.Clamp(-locVel.x * tiltAmplifier, -clampAngle, clampAngle),
                transform.rotation.eulerAngles.z
            ),
            3 * Time.deltaTime);
    }

    public void Damage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            print("RepairBot is now dead, make pretend!");
        }
    }
}