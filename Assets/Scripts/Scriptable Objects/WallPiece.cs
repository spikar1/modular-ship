using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu]
public class WallPiece : ScriptableObject {
    public GameObject wallBlenderFile;

    public List<Mesh> corner, straight, diagonal;
    public List<Mesh> cornerCol, straightCol, diagonalCol;

    public Material material;

    public int durability, integrity;

    /// <summary>
    /// </summary>
    /// <param name="shape"></param>
    /// <param name="index">if index == -1, returns random</param>
    /// <returns></returns>
    public Mesh GetMeshFromShape(Shape shape, int index = -1) {
        int i;
        switch (shape) {
            case Shape.Corner:
                i = index == -1 ? Random.Range(0, corner.Count - 1) : index;
                return corner[i];
            case Shape.Straight:
                i = index == -1 ? Random.Range(0, straight.Count - 1) : index;
                return straight[i];
            case Shape.Diagonal:
                i = index == -1 ? Random.Range(0, diagonal.Count - 1) : index;
                return diagonal[i];
            default:
                throw new System.Exception(shape + "is invalid");
        }
    }

    public Mesh GetMeshFromOrientation(WallOrientation wo,bool isDiagonal , int index = -1) {
        switch (wo) {
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

    public Mesh GetCollisionMesh(Mesh mesh) {

        for (int i = 0; i < cornerCol.Count; i++) {
            if (cornerCol[i].name.StartsWith(mesh.name))
                return cornerCol[i];
        }

        for (int i = 0; i < straightCol.Count; i++) {
            if (straightCol[i].name.StartsWith(mesh.name))
                return straightCol[i];
        }

        for (int i = 0; i < diagonalCol.Count; i++) {
            if (diagonalCol[i].name.StartsWith(mesh.name))
                return diagonalCol[i];
        }



        Debug.LogWarning("Can't Find collision mesh for " + mesh.name + " perhaps collision mesh is not named correctly");
        return straightCol[0];
    }
}
public enum Shape { Corner, Straight, Diagonal };

