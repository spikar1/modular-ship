using System.Collections;
using UnityEngine;

public class RandomObstacles : MonoBehaviour {
    IEnumerator Start() {
        var template = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(template.GetComponent<Collider>());
        yield return null;
        var col = template.AddComponent<CircleCollider2D>();
        
        col.sharedMaterial = new PhysicsMaterial2D {
            friction = 1f,
            bounciness = 1f
        };

        template.AddComponent<Obstacle>();
        var rb = template.AddComponent<Rigidbody2D>();
        rb.drag = .1f;
        

        for (int i = 0; i < 1000; i++) {
            var x = 0f;
            var y = 0f;
            while (x < 10f && y < 10f) {
                x = Random.Range(0, 100f);
                y = Random.Range(0, 100f);
            }

            if (Random.value < .5f)
                x = -x;
            if (Random.value < .5f)
                y = -y;
            var randPos = new Vector3(x, y, 0);

            Instantiate(template, randPos, Quaternion.identity);
        }
        Destroy(template);
    }
}