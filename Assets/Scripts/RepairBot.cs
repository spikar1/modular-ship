using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairBot : MonoBehaviour, IInputReceiver, IDamagable {

    Rigidbody2D rb;
    public float hp = 10;

    [Tooltip("Clamps the tilt at a certain angle")]
    public float clampAngle = 10;
    [Tooltip("amplify the tilting")]
    public float tiltAmplifier = 1;


    void Start() {
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();
        rb.mass = .1f;
    }

    private void Update() {
    }

    public void OnUpdate(Inputs inputs) {
         
        rb.AddForce(inputs.axis * .1f);
        RotateTowardsVelocity(inputs.axis);
    }



    void RotateTowardsVelocity(Vector2 axis) {
        if (!rb) {
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

    public void Damage(Vector2 relativeVelocity, float damage) {
        hp -= damage;
        if (hp <= 0) {
            print("RepairBot is now dead, make pretend!");
        }
            
    }
}
