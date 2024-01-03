using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateLevelOuterWall : MonoBehaviour
{
    [SerializeField] private Tilemap _wallTilemap;

    [SerializeField] private int _width = 100;
    [SerializeField] private int _height = 100;
    [SerializeField] private int _wallWidth = 4;

    [SerializeField]
    private TileBase[] edgesCornersMatrix = new TileBase[4]; // These tiles make up the edges and corners of the wall
    [SerializeField]
    private TileBase[] innerWallMatrix = new TileBase[4]; // These tiles make up the inner wall that repeats

    private List<GameObject> _wallCubes = new List<GameObject>();
    private float _wallRatio = .3f;

    private void Awake()
    {
        _wallTilemap.transform.position = new Vector3(-_width / 2, -_height / 2, 0);
    }

    private void Start()
    {
        CreateGrid();
        GenerateWallCubes();
    }
    private void CreateGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 1);
                TileBase tileToSet = DetermineTile(x, y);
                if (tileToSet != null)
                {
                    _wallTilemap.SetTile(pos, tileToSet);
                }
            }
        }
    }

    private TileBase DetermineTile(int x, int y)
    {
        bool isTopRow = y == _height - 1;
        bool isBottomRow = y == 0;
        bool isFirstColumn = x == 0;
        bool isLastColumn = x == _width - 1;

        bool isVerticalInnerEdge =  (x == _wallWidth - 1 || x == _width - _wallWidth) && (y >= _wallWidth - 1 && y <= _height - _wallWidth);

        bool isHorizontalInnerEdge = (y == _wallWidth - 1 || y == _height - _wallWidth) && (x >= _wallWidth - 1 && x <= _width - _wallWidth);

        if (isTopRow || isBottomRow || isFirstColumn || isLastColumn)
            return edgesCornersMatrix[GetMatrix1Index(x, y, isTopRow, isBottomRow, isFirstColumn, isLastColumn)];

        if (isVerticalInnerEdge)
            return (x % 2 != 0) ? (y % 2 != 0 ? edgesCornersMatrix[1] : edgesCornersMatrix[3]) : (y % 2 != 0 ? edgesCornersMatrix[0] : edgesCornersMatrix[2]);

        if (isHorizontalInnerEdge)
            return (y % 2 == 0) ? (x % 2 == 0 ? edgesCornersMatrix[2] : edgesCornersMatrix[3]) : (x % 2 == 0 ? edgesCornersMatrix[0] : edgesCornersMatrix[1]);

        return isVerticalWall(x) || isHorizontalWall(y) ? innerWallMatrix[GetMatrix2Index(x, y)] : null;
    }

    private bool isVerticalWall(int x)
    {
        return x < _wallWidth - 1 || x > _width - _wallWidth;
    }

    private bool isHorizontalWall(int y)
    {
        return y < _wallWidth - 1 || y > _height - _wallWidth;
    }

    private int GetMatrix1Index(int x, int y, bool isTopRow, bool isBottomRow, bool isFirstColumn, bool isLastColumn)
    {
        if (isTopRow || isBottomRow)
            return (x % 2 == 0) ? (isTopRow ? 0 : 2) : (isTopRow ? 1 : 3);
        else
            return (y % 2 == 0) ? (isFirstColumn ? 2 : 3) : (isFirstColumn ? 0 : 1);
    }

    private int GetMatrix2Index(int x, int y)
    {
        if (x % 2 != 0)
            return y % 2 == 0 ? 0 : 1;
        else
            return y % 2 == 0 ? 2 : 3;
    }

    // These wall cubes are needed to make the camera stay within the bounds of the level
    public void GenerateWallCubes()
    {
        float wallThickness = _wallWidth * _wallRatio;

        CreateWallCube(new Vector3(wallThickness / 2f, _height / 2f, 0), new Vector3(wallThickness, _height, 1), WallDirection.West);
        CreateWallCube(new Vector3(_width - wallThickness / 2f, _height / 2f, 0), new Vector3(wallThickness, _height, 1), WallDirection.East);
        CreateWallCube(new Vector3(_width / 2f, _height - wallThickness / 2f, 0), new Vector3(_width, wallThickness, 1), WallDirection.North);
        CreateWallCube(new Vector3(_width / 2f, wallThickness / 2f, 0), new Vector3(_width, wallThickness, 1), WallDirection.South);
    }

    private void CreateWallCube(Vector3 position, Vector3 scale, WallDirection direction)
    {
        GameObject wallCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wallCube.name = direction.ToString() + " Wall";
        wallCube.GetComponent<MeshRenderer>().material.color = Color.white;
        wallCube.transform.position = position - new Vector3(_width / 2, _height / 2, 0); // Adjust for grid offset
        wallCube.transform.localScale = scale;
        wallCube.transform.SetParent(_wallTilemap.transform);
        wallCube.layer = LayerMask.NameToLayer("Outerwall");
        _wallCubes.Add(wallCube);
    }
    public enum WallDirection
    {
        North,
        East,
        South,
        West
    }
}
