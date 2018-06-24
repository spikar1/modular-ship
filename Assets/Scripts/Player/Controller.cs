using UnityEngine;
using System.Collections;



public class Controller : MonoBehaviour {
    

    Vector2 lastPos;


    private void Start() {

    }
    public void Move(Vector2 vel, LayerMask collidableMask) {
        /*rb.drag = 0;
        if (vel != Vector2.zero)
            rb.AddRelativeForce(vel, ForceMode2D.Impulse);
        else
            rb.drag = 100;*/

        lastPos = transform.position;
        
        if (!Physics2D.OverlapCircle((Vector2)transform.position + vel, .1f, collidableMask)){
            transform.Translate(vel);
        }
    }
}