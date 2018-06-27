using UnityEngine;

public class Controller : MonoBehaviour
{
    Vector2 lastPos;

    public void Move(Vector2 vel, LayerMask collidableMask)
    {
        lastPos = transform.position;
        Debug.DrawLine((Vector2) transform.position, (Vector2) transform.position + vel, Color.yellow, .01f);

        var old = Physics2D.queriesHitTriggers;
        Physics2D.queriesHitTriggers = false;
        if (!Physics2D.OverlapCircle((Vector2) transform.position + vel, .2f, collidableMask))
        {
            transform.position += (Vector3) vel;
        }

        if (Physics2D.OverlapCircle(transform.position, .2f, collidableMask))
        {
            transform.position = lastPos;
        }

        Physics2D.queriesHitTriggers = old;
    }
}