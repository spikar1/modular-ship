using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDamagable {
    void Damage(Vector3 direction, float damage);
}