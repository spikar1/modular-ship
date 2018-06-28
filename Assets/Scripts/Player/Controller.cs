using UnityEngine;

public class Controller : MonoBehaviour
{
    Vector3 lastPos;

    public void Move(Vector2 velocity, LayerMask collidableMask)
    {
        lastPos = transform.position;
        Debug.DrawLine((Vector2) transform.position, (Vector2) transform.position + velocity, Color.yellow, .01f);

        var old = Physics2D.queriesHitTriggers;

        velocity = transform.TransformDirection(velocity);
        
        Physics2D.queriesHitTriggers = false;
        if (!Physics2D.OverlapCircle((Vector2) transform.position + velocity, .2f, collidableMask))
        {
            transform.Translate(velocity, Space.World);
        }

        if (Physics2D.OverlapCircle(transform.position, .2f, collidableMask))
        {
            transform.position = lastPos;
        }

        Physics2D.queriesHitTriggers = old;
    }
}