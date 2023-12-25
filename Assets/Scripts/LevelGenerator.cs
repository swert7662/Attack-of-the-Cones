using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int _width = 100;
    [SerializeField] private int _height = 100;

    [SerializeField, Range(0f, 1f)]
    private float _floorPerlinScale = 0.1f;
    [SerializeField, Range(0f, 1f)]
    private float _floorThreshold = 0.5f;
    [SerializeField] private Tilemap _floorTilemap;
    [SerializeField] private RuleTile _floorTile;

    //[SerializeField, Range(0f, 1f)]
    //private float _roadPerlinScale = 0.1f; // Perlin scale for paths
    //[SerializeField, Range(0f, 1f)]
    //private float _roadThreshold = 0.5f; // Threshold for path generation
    [SerializeField] private Tilemap _roadTilemap;
    [SerializeField] private RuleTile _roadTile;
    [SerializeField] private int _roadWidth = 3; // Width of the road in tiles
    [SerializeField] private int _roadSpacing = 10; // Spacing between roads in tiles
    [SerializeField] private bool _generateXRoads = true;
    [SerializeField] private bool _generateYRoads = true;
    [SerializeField] private int _xOffset = 0;
    [SerializeField] private int _yOffset = 0;

    [SerializeField] private Tilemap _decorationTilemap;
    [SerializeField] private RuleTile _decorationTile;

    private void Awake()
    {
        _floorTilemap = GetComponentInChildren<Tilemap>();
        transform.position = new Vector3(-_width / 2, -_height / 2, 0);
    }

    private void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel() 
    { 
        GenerateFloorTiles();
        GenerateRoadTiles();
        //GeneratePondTiles();
        //GenerateFenceTiles();
        //GenerateDecorationTiles();
    }

    public void GenerateFloorTiles()
    {
        FillTilemap(_floorTilemap, _floorTile);
        _floorTilemap.RefreshAllTiles();
    }

    private void GenerateRoadTiles()
    {
        CreateGrid();
    }

    private void GeneratePondTiles()
    {
        // Generates pond tiles that are form small "blobs" that are a set x and y size
    }
    private void GenerateFenceTiles()
    {
        // Generates fences tiles that can be any length but are only 1 tile wide
    }
    private void GenerateDecorationTiles()
    {
        // Generates decoration tiles that are 1 tile wide and 1 tile tall and can be placed in any empty space
    }

    bool IsTileEmpty(Tilemap tilemap, Vector2Int position)
    {
        return tilemap.GetTile((Vector3Int)position) == null; // Not sure if this is the correct way to check if a tile is empty
    }

    private void FillTilemap(Tilemap tilemap, RuleTile tile)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                float perlinValue = Mathf.PerlinNoise(x * _floorPerlinScale, y * _floorPerlinScale);

                if (perlinValue > _floorThreshold)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    tilemap.SetTile((Vector3Int)pos, tile);
                }
            }
        }
    }

    private void CreateGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                bool placeTile = false;

                if (_generateXRoads && (x + _xOffset) % _roadSpacing < _roadWidth)
                {
                    placeTile = true;
                }

                if (_generateYRoads && (y + _yOffset) % _roadSpacing < _roadWidth)
                {
                    placeTile = true;
                }

                if (placeTile && IsTileEmpty(_roadTilemap, new Vector2Int(x, y)))
                {
                    _roadTilemap.SetTile(new Vector3Int(x, y, 0), _roadTile);
                }
            }
        }
    }

    //private void CreatePerlinRoad()
    //{
    //    for (int x = 0; x < _width; x++)
    //    {
    //        for (int y = 0; y < _height; y++)
    //        {
    //            float perlinValue = Mathf.PerlinNoise(x * _roadPerlinScale, y * _roadPerlinScale);

    //            if (perlinValue > _roadThreshold)
    //            {
    //                Vector2Int pos = new Vector2Int(x, y);
    //                if (IsTileEmpty(_roadTilemap, pos))
    //                {
    //                    _roadTilemap.SetTile((Vector3Int)pos, _roadTile);
    //                }
    //            }
    //        }
    //    }
    //    /*
    //     // Adjustments for gradient calculation
    //        float dx = 0.01f;
    //        float dy = 0.01f;

    //for (int x = 0; x < _width; x++)
    //{
    //    for (int y = 0; y < _height; y++)
    //    {
    //        // Calculate gradient
    //        float perlinValue = Mathf.PerlinNoise(x * pathPerlinScale, y * pathPerlinScale);
    //        float perlinValueX = Mathf.PerlinNoise((x + dx) * pathPerlinScale, y * pathPerlinScale);
    //        float perlinValueY = Mathf.PerlinNoise(x * pathPerlinScale, (y + dy) * pathPerlinScale);

    //        float gradX = perlinValueX - perlinValue;
    //        float gradY = perlinValueY - perlinValue;
    //        float gradMagnitude = Mathf.Sqrt(gradX * gradX + gradY * gradY);

    //        // Adjusted threshold for path generation based on observed gradient magnitudes
    //        if (gradMagnitude > 0.0001f && gradMagnitude < 0.0015f) // Adjust these values as needed
    //        {
    //            for (int px = -pathWidth; px <= pathWidth; px++)
    //            {
    //                for (int py = -pathWidth; py <= pathWidth; py++)
    //                {
    //                    Vector2Int pos = new Vector2Int(x + px, y + py);
    //                    if(IsTileEmpty(_roadTilemap, pos))
    //                    {
    //                        _roadTilemap.SetTile((Vector3Int)pos, _roadTile);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
    //     */
    //}

    public void ClearTileMap()
    {
        _floorTilemap.ClearAllTiles();
        _roadTilemap.ClearAllTiles();
    }
}