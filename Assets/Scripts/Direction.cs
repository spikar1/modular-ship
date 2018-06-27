using System;
using UnityEngine;
using System.Collections;

public enum Direction
{
    N,
    NE,
    E,
    SE,
    S,
    SW,
    W,
    NW,
}

public static class DirectionExtension
{
    private static readonly Vector2 N = new Vector2(0f, 1f);
    private static readonly Vector2 NE = new Vector2(1f, 1f).normalized;
    private static readonly Vector2 E = new Vector2(1f, 0f);
    private static readonly Vector2 SE = new Vector2(1f, -1f).normalized;
    private static readonly Vector2 S = new Vector2(0f, -1f);
    private static readonly Vector2 SW = new Vector2(-1f, -1f).normalized;
    private static readonly Vector2 W = new Vector2(-1f, 0f);
    private static readonly Vector2 NW = new Vector2(-1f, 1f).normalized;

    private static readonly Vector2[] directionVectors =
    {
        N,
        NE,
        E,
        SE,
        S,
        SW,
        W,
        NW,
    };

    private static readonly Direction[] directions =
    {
        Direction.N,
        Direction.NE,
        Direction.E,
        Direction.SE,
        Direction.S,
        Direction.SW,
        Direction.W,
        Direction.NW,
    };

    public static Direction GetClosestDirection(this Vector2 direction)
    {
        var minAngle = Mathf.Infinity;
        var minIndex = -1;
        for (int i = 0; i < directions.Length; i++) {
            var angle = Vector3.Angle(direction, directionVectors[i]);
            if (angle < minAngle) {
                minAngle = angle;
                minIndex = i;
            }
        }
        return directions[minIndex];
    }


    public static Vector2 ToVector2(this Direction direction)
    {
        switch (direction)
        {
            case Direction.N: return N;
            case Direction.NE: return NE;
            case Direction.E: return E;
            case Direction.SE: return SE;
            case Direction.S: return S;
            case Direction.SW: return SW;
            case Direction.W: return W;
            case Direction.NW: return NW;
            default: throw new ArgumentOutOfRangeException(nameof(direction), direction.ToString());
        }
    }
}