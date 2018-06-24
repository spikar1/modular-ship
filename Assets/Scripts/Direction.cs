using UnityEngine;
using System.Collections;

public enum Direction {
    Up,
    Down,
    Right,
    Left,
}

public static class DirectionExtension {
    public static Vector2 ToVector2(this Direction direction) {
        Vector2 dir = default(Vector2);
        switch (direction) {
            case Direction.Up:
                dir = Vector2.up;
                break;
            case Direction.Down:
                dir = Vector2.down;
                break;
            case Direction.Right:
                dir = Vector2.right;
                break;
            default:
                dir = Vector2.left;
                break;
        }
        return dir;
    }
}