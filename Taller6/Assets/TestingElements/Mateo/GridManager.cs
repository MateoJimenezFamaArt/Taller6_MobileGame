using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int gridSize = 8;
    public float spacing = 2f;

    private List<Transform> spawnPoints = new List<Transform>();

    void Start()
    {
        if (tilePrefab == null)
        {
            Debug.LogError("GridManager: No Tile Prefab assigned!");
            return;
        }

        GenerateGrid();
    }

    void GenerateGrid()
    {
        Vector3 startPosition = transform.position - new Vector3(gridSize / 2 * spacing, 0, gridSize / 2 * spacing);

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 tilePosition = startPosition + new Vector3(x * spacing, 0, y * spacing);
                
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                tile.transform.parent = transform;

                GameObject point = new GameObject("SpawnPoint_" + (x * gridSize + y));
                point.transform.position = tilePosition;
                point.transform.parent = transform;
                spawnPoints.Add(point.transform);
            }
        }
    }

    public List<Transform> GetSpawnPoints() => spawnPoints;
}