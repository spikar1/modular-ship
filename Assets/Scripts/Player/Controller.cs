using UnityEngine;
using System.Collections;



public class Controller : MonoBehaviour {
    

    Vector2 lastPos;


    private void Start() {

    }
    public void Move(Vector2 vel, LayerMask collidableMask) {
        lastPos = transform.position;
        Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position + vel, Color.yellow,.01f);
        if (!Physics2D.OverlapCircle((Vector2)transform.position + vel, .2f, collidableMask)){
            transform.position += (Vector3)vel;
        }
        if(Physics2D.OverlapCircle(transform.position, .2f, collidableMask)) {
            transform.position = lastPos;
        }
    }
    
}