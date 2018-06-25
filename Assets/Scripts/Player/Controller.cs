using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class Controller : NetworkBehaviour {
    

    Vector2 lastPos;


    private void Start() {

    }
    public void Move(Vector2 vel, LayerMask collidableMask) {
        /*rb.drag = 0;
        if (vel != Vector2.zero)
            rb.AddRelativeForce(vel, ForceMode2D.Impulse);
        else
            rb.drag = 100;*/

        lastPos = transform.localPosition;
        
        if (!Physics2D.OverlapCircle((Vector2)transform.localPosition + vel, .1f, collidableMask)){
            transform.Translate(vel);
        }
    }
}