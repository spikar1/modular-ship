using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WallPiece : ScriptableObject
{
    public GameObject wallBlenderFile;

    public List<Mesh> corner, straight, diagonal;
    public List<Mesh> corner90, straight90, diagonal90;
    public List<Mesh> corner180, straight180, diagonal180;
    public List<Mesh> corner270, straight270, diagonal270;

    public List<Mesh> cornerCol, straightCol, diagonalCol;
    public List<Mesh> corner90Col, straight90Col, diagonal90Col;
    public List<Mesh> corner180Col, straight180Col, diagonal180Col;
    public List<Mesh> corner270Col, straight270Col, diagonal270Col;

    private List<List<Mesh>> _meshLists;
    private List<List<Mesh>> _meshColliderLists;

    private List<List<Mesh>> meshLists
    {
        get
        {
            if (_meshLists == null)
            {
                _meshLists = new List<List<Mesh>>
                {
                    corner,
                    straight,
                    diagonal,
                    corner90,
                    straight90,
                    diagonal90,
                    corner180,
                    straight180,
                    diagonal180,
                    corner270,
                    straight270,
                    diagonal270,
                };
            }

            return _meshLists;
        }
    }

    private List<List<Mesh>> meshColliderLists
    {
        get
        {
            if (_meshColliderLists == null)
            {
                _meshColliderLists = new List<List<Mesh>>
                {
                    cornerCol,
                    straightCol,
                    diagonalCol,
                    corner90Col,
                    straight90Col,
                    diagonal90Col,
                    corner180Col,
                    straight180Col,
                    diagonal180Col,
                    corner270Col,
                    straight270Col,
                    diagonal270Col,
                };
            }

            return _meshColliderLists;
        }
    }

    public Material material;

    public int durability, integrity;

    //if index == -1, returns random
    public Mesh GetMeshFromOrientation(WallOrientation orientation, bool isDiagonal, int index = -1)
    {
        List<Mesh> meshList;
        switch (orientation)
        {
            case WallOrientation.TopLeft:
                meshList = isDiagonal ? diagonal : corner;
                break;
            case WallOrientation.BottomLeft:
                meshList = isDiagonal ? diagonal90 : corner90;
                break;
            case WallOrientation.BottomRight:
                meshList = isDiagonal ? diagonal180 : corner180;
                break;
            case WallOrientation.TopRight:
                meshList = isDiagonal ? diagonal270 : corner270;
                break;
            case WallOrientation.Top:
                meshList = straight;
                break;
            case WallOrientation.Left:
                meshList = straight90;
                break;
            case WallOrientation.Bottom:
                meshList = straight180;
                break;
            case WallOrientation.Right:
                meshList = straight270;
                break;
            default:
                throw new System.Exception(orientation + " is not a valid enum value");
        }

        Debug.Log(index);
        if (index == -1)
            index = Random.Range(0, meshList.Count);

        return meshList[index % meshList.Count];
    }

    public Mesh GetCollisionMesh(Mesh mesh)
    {
        for (int i = 0; i < meshLists.Count; i++)
        for (int j = 0; j < meshLists[i].Count; j++)
            if (mesh == meshLists[i][j])
                return meshColliderLists[i][j];

        Debug.LogError($"Can't Find collision mesh for {mesh.name} perhaps collision mesh is not named correctly");
        return straightCol[0];
    }
}