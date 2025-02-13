using UnityEngine;

public class SmoothFollowCamera : MonoBehaviour
{
    public Transform player; // Assign the player transform in the inspector
    public Vector3 offset = new Vector3(0, 2, -5); // Default offset behind the player
    public float smoothSpeed = 0.1f; // Smoothness factor (higher = slower response)

    private void LateUpdate()
    {
        if (player == null) return;

        // Target position based on player position and offset
        Vector3 targetPosition = player.position + offset;
        
        // Smoothly interpolate between current position and target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        // Keep looking at the player
        transform.LookAt(player.position);
    }
}
