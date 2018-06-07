using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Collider2D))]
public class Controller : MonoBehaviour {
    [HideInInspector]
    public BoxCollider2D col;

    public LayerMask collisionMask;

    const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    RaycastOrigins raycastOrigins;

    void Start () {
        col = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();  
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += (transform.up * (horizontalRaySpacing * i)).toVector2();
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, transform.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, transform.right * directionX * rayLength, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth*6) * directionX;
                rayLength = hit.distance;
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += (transform.right * (verticalRaySpacing * i + velocity.x)).toVector2();
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, transform.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, transform.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth*6) * directionY;
                rayLength = hit.distance;
            }
        }
    }

    void UpdateRaycastOrigins()
    {
        raycastOrigins.bottomLeft = transform.TransformPoint(new Vector3(-col.size.x/2, -col.size.y/2)); 
        raycastOrigins.bottomRight = transform.TransformPoint(new Vector3(col.size.x/2, -col.size.y/2)); 
        raycastOrigins.topLeft = transform.TransformPoint(new Vector3(-col.size.x/2, col.size.y/2)); 
        raycastOrigins.topRight = transform.TransformPoint(new Vector3(col.size.x/2, col.size.y/2)); 
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = col.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
