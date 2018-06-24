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
            Vector2 thrust = transform.rotation * thruster.Thrust();
            rb.AddForceAtPosition(thrust, thruster.transform.position);
        }
    }
}
