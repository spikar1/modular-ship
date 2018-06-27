using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour, IInputReciever
{
    private Rigidbody2D rb;
    private List<Thruster> downThrusters = new List<Thruster>();
    private List<Thruster> leftThrusters = new List<Thruster>();
    private List<Thruster> rightThrusters = new List<Thruster>();
    private List<Thruster> upThrusters = new List<Thruster>();
    private List<List<Thruster>> allThrusters;

    private List<Thruster> thrustersToFire = new List<Thruster>();

    private List<Room> rooms;
    private List<RoomNode> nodes;
    private List<Wall> walls;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        allThrusters = new List<List<Thruster>>
        {
            upThrusters,
            downThrusters,
            leftThrusters,
            rightThrusters
        };

        CreateDebugMarker(Color.black, rb.centerOfMass);
        rb.centerOfMass = rb.centerOfMass.RoundedToInt();
        CreateDebugMarker(Color.red, rb.centerOfMass);

        rooms = GetComponentsInChildren<Room>().ToList();
        nodes = new List<RoomNode>();
        walls = new List<Wall>();
        foreach (var room in rooms)
        {
            room.Initialize(this);
            nodes.AddRange(room.roomNodes);
            foreach (var node in room.roomNodes)
            {
                var wall = node.GetComponent<Wall>();
                if (wall)
                    walls.Add(wall);
            }
        }

        var attachables = GetComponentsInChildren<Attachment>();
        foreach (var attachable in attachables)
        {
            attachable.TryAttachToNearest(walls);
        }
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

    public void ReceiveInput(Inputs inputs)
    {
        var movement = inputs.axis;
        if (movement == default(Vector2))
            return;

        thrustersToFire.Clear();
        if (movement.x > 0)
            thrustersToFire.AddRange(rightThrusters);
        else if (movement.x < 0)
            thrustersToFire.AddRange(leftThrusters);

        if (movement.y > 0)
            thrustersToFire.AddRange(upThrusters);
        else if (movement.y < 0)
            thrustersToFire.AddRange(downThrusters);

        foreach (var thruster in thrustersToFire)
        {
            var thrust = transform.rotation * thruster.Thrust();
            var thrustPos = ((Vector2) thruster.transform.position).RoundedToInt();
            rb.AddForceAtPosition(thrust, thrustPos);
        }
    }

    public void RegisterThruster(Thruster thruster)
    {
        foreach (var thrusterList in allThrusters)
        {
            if (thrusterList.Contains(thruster))
            {
                Debug.LogError($"Already has thruster {thruster} in a list of thrusters when it gets registered!",
                    thruster);
                return;
            }
        }

        switch (thruster.direction)
        {
            case Direction.N:
                upThrusters.Add(thruster);
                break;
            case Direction.S:
                downThrusters.Add(thruster);
                break;
            case Direction.E:
                rightThrusters.Add(thruster);
                break;
            case Direction.W:
                leftThrusters.Add(thruster);
                break;
            default:
                break;
        }
    }

    public void DeregisterThruster(Thruster thruster)
    {
        upThrusters.Remove(thruster);
        downThrusters.Remove(thruster);
        leftThrusters.Remove(thruster);
        rightThrusters.Remove(thruster);
    }
}