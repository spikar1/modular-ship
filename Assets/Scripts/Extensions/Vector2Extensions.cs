using UnityEngine;
using System.Collections;

static public class Vector2Extensions {

    static public Vector3 toVector3 (this Vector2 _v2, float z = 0)
    {
        return new Vector3(_v2.x, _v2.y, z);
    }
}
