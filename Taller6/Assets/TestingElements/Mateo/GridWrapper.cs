using UnityEngine;

public class GridWrap : MonoBehaviour
{
    public Vector2Int gridSize = new Vector2Int(8, 8); // Grid size in tiles
    public float spacing = 2f; // Distance between tiles

    private float gridMinX, gridMaxX, gridMinZ, gridMaxZ;

    void Start()
    {
        // Calculate grid boundaries based on grid size and spacing
        float gridExtentX = (gridSize.x / 2) * spacing;
        float gridExtentZ = (gridSize.y / 2) * spacing;

        gridMinX = transform.position.x - gridExtentX;
        gridMaxX = transform.position.x + gridExtentX;
        gridMinZ = transform.position.z - gridExtentZ;
        gridMaxZ = transform.position.z + gridExtentZ;
    }

    void Update()
    {
        Vector3 playerPosition = transform.position;

        // Determine wrap-around position
        float newX = playerPosition.x;
        float newZ = playerPosition.z;

        bool wrapped = false;

        if (playerPosition.x > gridMaxX)
        {
            newX = gridMinX;
            wrapped = true;
        }
        else if (playerPosition.x < gridMinX)
        {
            newX = gridMaxX;
            wrapped = true;
        }

        if (playerPosition.z > gridMaxZ)
        {
            newZ = gridMinZ;
            wrapped = true;
        }
        else if (playerPosition.z < gridMinZ)
        {
            newZ = gridMaxZ;
            wrapped = true;
        }

        // Apply the teleport if necessary
        if (wrapped)
        {
            transform.position = new Vector3(newX, playerPosition.y, newZ);
        }
    }
}