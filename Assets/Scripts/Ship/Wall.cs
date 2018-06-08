using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IDamagable {
    public int durability;
    public int integrity;

    public void Damage(Vector3 direction, float damage) {
        print("OUCH!");
        durability -= (Mathf.FloorToInt(damage));
        if (durability <= 0)
            DestroyWall();

    }

    void DestroyWall() {
        Destroy(gameObject);
    }
}   
