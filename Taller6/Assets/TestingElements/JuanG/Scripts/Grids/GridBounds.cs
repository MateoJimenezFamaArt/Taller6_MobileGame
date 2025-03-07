using UnityEngine;

public class GridWrap : MonoBehaviour
{
    public Vector2Int gridSize = new Vector2Int(8, 8); // Grid size in tiles
    public float spacing = 2f; // Distance between tiles
    public Transform gridCenter; // Center of the grid

    private float gridMinX, gridMaxX, gridMinZ, gridMaxZ;

    void Start()
    {
        if (gridCenter == null)
        {
            Debug.LogError("GridWrap: Please assign a Grid Center.");
            return;
        }

        // Calculate grid boundaries based on grid size and spacing
        float gridExtentX = (gridSize.x / 2) * spacing;
        float gridExtentZ = (gridSize.y / 2) * spacing;

        gridMinX = gridCenter.position.x - gridExtentX;
        gridMaxX = gridCenter.position.x + gridExtentX;
        gridMinZ = gridCenter.position.z - gridExtentZ;
        gridMaxZ = gridCenter.position.z + gridExtentZ;
    }

    void Update()
    {
        Vector3 playerPosition = transform.position;

        // Clamp position to stay within bounds
        float clampedX = Mathf.Clamp(playerPosition.x, gridMinX, gridMaxX - spacing);
        float clampedZ = Mathf.Clamp(playerPosition.z, gridMinZ, gridMaxZ - spacing);

        // Apply clamped position
        transform.position = new Vector3(clampedX, playerPosition.y, clampedZ);
    }
}