using System.Collections.Generic;
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

    [SerializeField] private Tilemap _decorationTilemap;
    [SerializeField] private RuleTile _decorationTile;

    private void Awake()
    {
        _floorTilemap = GetComponentInChildren<Tilemap>();
        _floorTilemap.transform.position = new Vector3(-_width / 2, -_height / 2, 0);
    }

    private void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel() 
    { 
        GenerateFloorTiles();
        //GenerateRoadTiles();
        //GenerateWallCubes();
        //GeneratePondTiles();
        //GenerateFenceTiles();
        //GenerateDecorationTiles();
    }

    public void GenerateFloorTiles()
    {
        FillTilemap(_floorTilemap, _floorTile);
        _floorTilemap.RefreshAllTiles();
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
                    Vector3Int pos = new Vector3Int(x, y, 1);
                    tilemap.SetTile(pos, tile);
                }
            }
        }
    }

    public void ClearTileMap()
    {
        _floorTilemap.ClearAllTiles();
    }
}