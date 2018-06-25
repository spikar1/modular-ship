using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDamagable {
    void Damage(Vector2 relativeVelocity, float damage);
}