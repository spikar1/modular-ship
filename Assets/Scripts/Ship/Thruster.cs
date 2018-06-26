using UnityEngine;
using System;

[SelectionBase]
public class Thruster : MonoBehaviour, IAttachable {

    private static readonly Vector2[] directions = {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };
    private static readonly float timeToCoolDown = .5f;
    private static readonly Color thrustingColor = Color.red;
    private static readonly Color notThrustingColor = Color.white;

    /// <summary>
    /// The direction this thruster pushes the ship in. So if the thruster is pointing down, this should be up.
    /// NOTE: It's assumed that the ship starts out pointing in the direction y+. 
    /// </summary>
    public Direction direction { get; private set; }
    [SerializeField]
    private float force = 2f;
    [SerializeField]
    private Renderer fireRenderer;

    private float lastThrustTime;
    private Color color = Color.white;
    private Material material;

    private Ship ship;

    private void Awake() {
        fireRenderer.sharedMaterial = material = new Material(fireRenderer.sharedMaterial);
    }

    public Direction GetThrustDirection() => ((Vector2) transform.up).GetClosestDirection();

    public Vector2 Thrust() {
        lastThrustTime = Time.time;
        return direction.ToVector2() * force;
    }

    private void Update() {
        float timeSinceThrusted = Time.time - lastThrustTime;
        Color wantedColor = Color.Lerp(thrustingColor, notThrustingColor, timeSinceThrusted / timeToCoolDown);
        if(wantedColor != color) {
            material.color = color = wantedColor;
        }
    }

    private void OnDrawGizmosSelected() {
        var dir = GetThrustDirection();
        var from = transform.position;
        var to = transform.position - (Vector3)dir.ToVector2();
        Gizmos.DrawLine(from, to);
    }

    public void OnAttachedTo(Wall wall)
    {
        direction = GetThrustDirection();
        ship = wall.roomNode.room.ship;
        ship.RegisterThruster(this);
    }

    public void OnDetach()
    {
        if (ship != null)
        {
            ship.DeregisterThruster(this);
            ship = null;
        }
    }
}