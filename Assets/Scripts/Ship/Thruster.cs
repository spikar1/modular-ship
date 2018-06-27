using UnityEngine;
using System;

[SelectionBase]
public class Thruster : MonoBehaviour, IAttachable, IInteractable
{
    private static readonly float timeToCoolDown = .5f;
    private static readonly Color thrustingColor = Color.red;
    private static readonly Color notThrustingColor = Color.white;

    /// <summary>
    /// The direction this thruster pushes the ship in. So if the thruster is pointing down, this should be up.
    /// NOTE: It's assumed that the ship starts out pointing in the direction y+. 
    /// </summary>
    public Direction direction { get; private set; }

    [SerializeField] private float force = 2f;
    [SerializeField] private Renderer fireRenderer;

    private float lastThrustTime;
    private Color color = Color.white;
    private Material material;

    private Ship ship;

    private void Awake()
    {
        fireRenderer.sharedMaterial = material = new Material(fireRenderer.sharedMaterial);
    }

    public Direction GetThrustDirection() => ((Vector2) transform.up).GetClosestDirection();

    public void Thrust()
    {
        Vector2 thrustLocal = direction.ToVector2() * force;
        Vector2 thrustWorld = ship.transform.rotation * thrustLocal;

        Vector2 thrustPos = transform.position;

        ship.rigidbody2D.AddForceAtPosition(thrustWorld, thrustPos.RoundedToInt());
        Debug.DrawLine(thrustPos, thrustPos + thrustWorld);

        lastThrustTime = Time.time;
    }

    private void Update()
    {
        float timeSinceThrusted = Time.time - lastThrustTime;
        Color wantedColor = Color.Lerp(thrustingColor, notThrustingColor, timeSinceThrusted / timeToCoolDown);
        if (wantedColor != color)
        {
            material.color = color = wantedColor;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!ship)
            return;
        var from = transform.position;

        var thrustLocal = direction.ToVector2() * force;
        var thrustWorld = ship.transform.rotation * thrustLocal;

        var to = transform.position - thrustLocal.ToVector3();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(from, to);
        to = transform.position - thrustWorld;
        Gizmos.color = Color.green;
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

    public void OnInteractDown() { }

    public void OnInteractHeld()
    {
        if(ship != null) // "if attached"
            Thrust();
    }
}