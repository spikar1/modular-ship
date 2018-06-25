using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WallPiece : ScriptableObject
{
    public GameObject wallBlenderFile;

    public List<Mesh> corner, straight, diagonal;
    public List<Mesh> cornerCol, straightCol, diagonalCol;

    public Material material;

    public int durability, integrity;

    //if index == -1, returns random
    public Mesh GetMeshFromOrientation(WallOrientation wo, bool isDiagonal, int index = -1)
    {
        switch (wo)
        {
            case WallOrientation.TopLeft:
            case WallOrientation.TopRight:
            case WallOrientation.BottomLeft:
            case WallOrientation.BottomRight:
                if (isDiagonal)
                    return GetMeshFromShape(Shape.Diagonal, index);
                else
                    return GetMeshFromShape(Shape.Corner, index);
            case WallOrientation.Top:
            case WallOrientation.Left:
            case WallOrientation.Right:
            case WallOrientation.Bottom:
                return GetMeshFromShape(Shape.Straight, index);
            default:
                throw new System.Exception(wo + " is not a valid enum value");
        }
    }

    private Mesh GetMeshFromShape(Shape shape, int index = -1)
    {
        List<Mesh> meshesToUse;
        
        switch (shape)
        {
            case Shape.Corner:
                meshesToUse = corner;
                break;
            case Shape.Straight:
                meshesToUse = straight;
                break;
            case Shape.Diagonal:
                meshesToUse = diagonal;
                break;
            default:
                throw new System.Exception($"shape type {shape} is invalid");
        }

        if (index == -1)
            index = Random.Range(0, meshesToUse.Count - 1);

        return meshesToUse[index];
    }

    public Mesh GetCollisionMesh(Mesh mesh)
    {
        foreach (var col in cornerCol)
            if (col.name.StartsWith(mesh.name))
                return col;

        foreach (var col in straightCol)
            if (col.name.StartsWith(mesh.name))
                return col;

        foreach (var col in diagonalCol)
            if (col.name.StartsWith(mesh.name))
                return col;

        Debug.LogError($"Can't Find collision mesh for {mesh.name} perhaps collision mesh is not named correctly");
        return straightCol[0];
    }
}