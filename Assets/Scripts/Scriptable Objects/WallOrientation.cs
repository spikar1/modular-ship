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
    public static Quaternion ToRotation(this WallOrientation wallOrientation)
    {
        switch (wallOrientation)
        {
            case WallOrientation.TopLeft:
            case WallOrientation.Top:
                return Quaternion.Euler(0, 0, 0);
            case WallOrientation.TopRight:
            case WallOrientation.Right:
                return Quaternion.Euler(0, 0, -90);
            case WallOrientation.BottomRight:
            case WallOrientation.Bottom:
                return Quaternion.Euler(0, 0, 180);
            case WallOrientation.BottomLeft:
            case WallOrientation.Left:
                return Quaternion.Euler(0, 0, 90);
            default:
                throw new System.Exception($"{wallOrientation} is not valid");
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