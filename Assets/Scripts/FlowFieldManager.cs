using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FlowFieldManager : MonoBehaviour
{
    public Tilemap flowFieldTilemap; // Assign the new flow field tilemap in inspector
    public Tile baseFlowFieldTile; // Assign your base flow field tile asset in inspector
    public int viewRadius = 10; // Radius around the player to visualize

    void Update()
    {
        CalculateAndVisualizeCosts();
    }

    void CalculateAndVisualizeCosts()
    {
        // Clear previous flow field tiles
        flowFieldTilemap.ClearAllTiles();

        // Player's position in grid coordinates
        Vector3Int playerPos = GetGridPosition(GameManager.Instance._playerTransform.position);

        // Initialize cost map with high default costs
        Dictionary<Vector3Int, int> costMap = new Dictionary<Vector3Int, int>();

        // BFS Queue
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        queue.Enqueue(playerPos);
        costMap[playerPos] = 0;

        while (queue.Count > 0)
        {
            Vector3Int current = queue.Dequeue();

            foreach (Vector3Int dir in new Vector3Int[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right })
            {
                Vector3Int neighbor = current + dir;
                if (!costMap.ContainsKey(neighbor) && IsWithinViewRadius(playerPos, neighbor) && IsValidTile(neighbor))
                {
                    costMap[neighbor] = costMap[current] + 1;
                    queue.Enqueue(neighbor);

                    // Set tile with color based on cost
                    Color tileColor = Color.Lerp(Color.green, Color.red, (float)costMap[neighbor] / viewRadius);
                    SetTileWithColor(neighbor, tileColor);
                }
            }
        }
    }

    bool IsWithinViewRadius(Vector3Int center, Vector3Int position)
    {
        return (position - center).magnitude <= viewRadius;
    }

    bool IsValidTile(Vector3Int position)
    {
        // Add your logic here to determine if the tile is valid for pathfinding
        // For instance, it shouldn't be a wall or an obstacle
        return true; // Placeholder
    }

    void SetTileWithColor(Vector3Int position, Color color)
    {
        flowFieldTilemap.SetTile(position, baseFlowFieldTile);
        flowFieldTilemap.SetColor(position, color);
    }

    Vector3Int GetGridPosition(Vector3 worldPos)
    {
        // Convert world position to grid position
        return flowFieldTilemap.WorldToCell(worldPos);
    }
}
