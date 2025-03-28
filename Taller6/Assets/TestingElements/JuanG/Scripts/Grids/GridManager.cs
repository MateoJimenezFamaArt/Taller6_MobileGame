using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GameObject[] tilePrefabs; // Array for multiple tile prefabs
    public int gridSize = 8;
    public float spacing = 2f;

    private List<Transform> spawnPoints = new List<Transform>();
    private List<Transform> borderSpawnPoints = new List<Transform>();

    void Start()
    {
        if (tilePrefabs == null || tilePrefabs.Length == 0)
        {
            Debug.LogError("GridManager: No Tile Prefabs assigned!");
            return;
        }

        GenerateGrid();
    }

    void GenerateGrid()
    {
        int expandedSize = gridSize + 2; // Expand grid size to include borders
        Vector3 startPosition = transform.position - new Vector3(expandedSize / 2 * spacing, 0, expandedSize / 2 * spacing);

        for (int x = 0; x < expandedSize; x++)
        {
            for (int y = 0; y < expandedSize; y++)
            {
                Vector3 tilePosition = startPosition + new Vector3(x * spacing, 0, y * spacing);
                Vector3 bordertilesposition = tilePosition + new Vector3(0, 1, 0);

                GameObject tile;
                GameObject point = new GameObject("SpawnPoint_" + (x * expandedSize + y));
                point.transform.position = tilePosition;
                point.transform.parent = transform;
                GameObject borderpoint = new GameObject("BorderSpawnPoint_" + (x * expandedSize + y));
                borderpoint.transform.position = bordertilesposition;
                borderpoint.transform.parent = transform;

                // Check if it's a border or center tile
                if (x == 0 || x == expandedSize - 1 || y == 0 || y == expandedSize - 1)
                {
                    if (x != 0 || y != 0 && x != 0 || y != expandedSize - 1 && x != expandedSize - 1 || y != 0 && x != expandedSize - 1 || y != expandedSize - 1)
                    {
                        // Border tile logic (optional to instantiate different tiles)
                        borderSpawnPoints.Add(borderpoint.transform); // Save border spawn point
                    }
                }
                else
                {
                    // Randomly select a tile prefab from the array
                    GameObject selectedPrefab = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
                    tile = Instantiate(selectedPrefab, tilePosition, Quaternion.identity);
                    spawnPoints.Add(point.transform); // Save spawn point
                    tile.transform.parent = transform;
                }
            }
        }
    }

    public List<Transform> GetSpawnPoints() => spawnPoints;
    public List<Transform> GetBorderSpawnPoints() => borderSpawnPoints;
}
