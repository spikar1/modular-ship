using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour, IInputReciever {

    private Rigidbody2D rb;
    private List<Thruster> thrusters;

    private List<Thruster> downThrusters, leftThrusters, rightThrusters, upThrusters;
    private List<Thruster> thrustersToFire = new List<Thruster>();

    void Start() {
        thrusters = new List<Thruster>(GetComponentsInChildren<Thruster>());
        upThrusters = thrusters.Where(t => t.direction == Direction.Up).ToList();
        downThrusters = thrusters.Where(t => t.direction == Direction.Down).ToList();
        leftThrusters = thrusters.Where(t => t.direction == Direction.Left).ToList();
        rightThrusters = thrusters.Where(t => t.direction == Direction.Right).ToList();

        thrustersToFire = new List<Thruster>();
        rb = GetComponent<Rigidbody2D>();

        CreateDebugMarker(Color.black, rb.centerOfMass);
        rb.centerOfMass = rb.centerOfMass.RoundedToInt();
        CreateDebugMarker(Color.red, rb.centerOfMass);
    }

    private void CreateDebugMarker(Color color, Vector2 rbCenterOfMass)
    {
        var marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(marker.GetComponent<Collider>());
        var mRend = marker.GetComponent<Renderer>();
        Material mMat;
        mRend.sharedMaterial = mMat = new Material(mRend.sharedMaterial);

        mMat.color = color;
        marker.transform.parent = transform;
        marker.transform.localScale = .2f * Vector3.one;

        marker.transform.localPosition = rbCenterOfMass;
    }

    public void ReceiveInput(bool action1Down, bool action1Up, Vector2 movement) {
        if (movement == default(Vector2))
            return;

        thrustersToFire.Clear();
        if (movement.x > 0)
            thrustersToFire.AddRange(rightThrusters);
        else if (movement.x < 0)
            thrustersToFire.AddRange(leftThrusters);

        if (movement.y > 0)
            thrustersToFire.AddRange(upThrusters);
        else if(movement.y < 0)
            thrustersToFire.AddRange(downThrusters);

        foreach (var thruster in thrustersToFire) {
            var thrust = transform.rotation * thruster.Thrust();
            var thrustPos = ((Vector2) thruster.transform.position).RoundedToInt();
            rb.AddForceAtPosition(thrust, thrustPos);
        }
    }
}
