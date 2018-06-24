using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkBullet : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision) {
        var hit = collision.gameObject;
        var health = hit.GetComponent<NetworkHealth>();
        if(health != null) {
            health.TakeDamage(10);
        }
        Destroy(gameObject);
    }
}
