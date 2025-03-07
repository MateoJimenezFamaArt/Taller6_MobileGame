using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GameObject[] tilePrefabs; // Array for different tile prefabs
    public int gridSize = 8;
    public float spacing = 2f;
    
    public Color[] tileColors; // Array for different possible tile colors

    private List<Transform> spawnPoints = new List<Transform>();

    void Start()
    {
        if (tilePrefabs == null || tilePrefabs.Length == 0)
        {
            Debug.LogError("GridManager: No Tile Prefabs assigned!");
            return;
        }

        if (tileColors == null || tileColors.Length == 0)
        {
            Debug.LogError("GridManager: No Tile Colors assigned!");
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

                // Randomly select a tile prefab from the array
                GameObject tilePrefab = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                tile.transform.parent = transform;

                // Randomly select a color from the array
                Renderer tileRenderer = tile.GetComponent<Renderer>();
                if (tileRenderer != null)
                {
                    Color randomColor = tileColors[Random.Range(0, tileColors.Length)];
                    tileRenderer.material.color = randomColor;
                }
                else
                {
                    Debug.LogWarning("GridManager: Tile prefab missing Renderer component!");
                }

                // Create spawn point at tile position
                GameObject point = new GameObject("SpawnPoint_" + (x * gridSize + y));
                point.transform.position = tilePosition;
                point.transform.parent = transform;
                spawnPoints.Add(point.transform);
            }
        }
    }

    public List<Transform> GetSpawnPoints() => spawnPoints;
}
