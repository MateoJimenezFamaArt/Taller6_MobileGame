using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject borderTilePrefab; // Prefab para las plataformas azules
    public int gridSize = 8;
    public float spacing = 2f;

    private List<Transform> spawnPoints = new List<Transform>();
    private List<Transform> borderSpawnPoints = new List<Transform>(); // Lista para los tiles azules

    void Start()
    {
        if (tilePrefab == null || borderTilePrefab == null)
        {
            Debug.LogError("GridManager: No Tile Prefab assigned!");
            return;
        }

        GenerateGrid();
    }

    void GenerateGrid()
    {
        int expandedSize = gridSize + 2; // Tamaño expandido para incluir los bordes
        Vector3 startPosition = transform.position - new Vector3(expandedSize / 2 * spacing, 0, expandedSize / 2 * spacing);

        for (int x = 0; x < expandedSize; x++)
        {
            for (int y = 0; y < expandedSize; y++)
            {
                Vector3 tilePosition = startPosition + new Vector3(x * spacing, 0, y * spacing);

                GameObject tile;
                GameObject point = new GameObject("SpawnPoint_" + (x * expandedSize + y));
                point.transform.position = tilePosition;
                point.transform.parent = transform;

                // Determinar si es borde o centro
                if (x == 0 || x == expandedSize - 1 || y == 0 || y == expandedSize - 1)
                {
                    tile = Instantiate(borderTilePrefab, tilePosition, Quaternion.identity);
                    borderSpawnPoints.Add(point.transform); // Guardar spawn point azul
                }
                else
                {
                    tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                    spawnPoints.Add(point.transform); // Guardar spawn point rojo
                }

                tile.transform.parent = transform;
            }
        }
    }

    public List<Transform> GetSpawnPoints() => spawnPoints;
    public List<Transform> GetBorderSpawnPoints() => borderSpawnPoints;
}
