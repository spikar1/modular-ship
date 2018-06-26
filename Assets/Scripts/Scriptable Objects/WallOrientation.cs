using System;
using UnityEngine;

public enum WallOrientation
{
    TopLeft,
    Top,
    TopRight,
    Left,
    Right,
    BottomLeft,
    Bottom,
    BottomRight
}

public static class WallOrientationExtension
{
    public static Quaternion ToLookRotation(this WallOrientation wallOrientation)
    {
        switch (wallOrientation)
        {
            case WallOrientation.Bottom:
                return Quaternion.Euler(0, 0, 0);
            case WallOrientation.BottomRight:
                return Quaternion.Euler(0, 0, 45);
            case WallOrientation.Right:
                return Quaternion.Euler(0, 0, 90);
            case WallOrientation.TopRight:
                return Quaternion.Euler(0, 0, 135);
            case WallOrientation.Top:
                return Quaternion.Euler(0, 0, 180);
            case WallOrientation.TopLeft:
                return Quaternion.Euler(0, 0, 225);
            case WallOrientation.Left:
                return Quaternion.Euler(0, 0, 270);
            case WallOrientation.BottomLeft:
                return Quaternion.Euler(0, 0, 315);
            default:
                throw new Exception($"{wallOrientation} is not valid");
        }
    }

    private static readonly Vector2 topLeftOffset     = (Vector2.up   + Vector2.left) .normalized;
    private static readonly Vector2 topRightOffset    = (Vector2.up   + Vector2.right).normalized;
    private static readonly Vector2 bottomLeftOffset  = (Vector2.down + Vector2.left) .normalized;
    private static readonly Vector2 bottomRightOffset = (Vector2.down + Vector2.right).normalized;

    public static Vector2 ToOffset(this WallOrientation wallOrientation)
    {
        switch (wallOrientation)
        {
            case WallOrientation.TopLeft:
                return topLeftOffset;
            case WallOrientation.Top:
                return Vector2.up;
            case WallOrientation.TopRight:
                return topRightOffset;
            case WallOrientation.Left:
                return Vector2.left;
            case WallOrientation.Right:
                return Vector2.right;
            case WallOrientation.BottomLeft:
                return bottomLeftOffset;
            case WallOrientation.Bottom:
                return Vector2.down;
            case WallOrientation.BottomRight:
                return bottomRightOffset;
        }

        throw new ArgumentOutOfRangeException(nameof(wallOrientation), wallOrientation, null);
    }
}